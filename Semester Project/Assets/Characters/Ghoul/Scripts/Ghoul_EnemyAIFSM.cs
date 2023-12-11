using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

/// <summary>
/// Enemy AI Finite State Machine (FSM)
/// 
/// States:
/// - Patrol: Patrol between set points.
///   - Transition to Alert if sound is heard.
/// 
/// - Alert: Stop briefly and listen.
///   - Transition to RPatrol if sound is heard.
///   - Transition to Patrol if an amount of time has passed and no sound was heard.
/// 
/// - RPatrol: Walk to sound source, patrol in radius.
///   - Transition to Chase if the player is seen for more than a specified time.
///   - Transition to Patrol if patrol is completed and the player is not seen.
/// 
/// - Chase: Scream and chase the player.
///   - Transition to Silent if line of sight is broken for a specified time.
/// 
/// - Silent: Pause after chasing the player.
///   - Transition to Chase if the player is seen again for more than a specified time.
///   - Transition to Patrol if a specified time has passed and the player was not seen.
/// </summary>

public class Ghoul_EnemyAIFSM : BaseFSM
{
    public enum FSMState
    {
        Patrol,
        Alert,
        RadiusPatrol,
        Scream,
        Dead
    }

    public FSMState currentState;

    private Transform player;
    private NavMeshAgent agent;

    private Ghoul_EnemyEarshot earshot;
    private Ghoul_LineOfSight eyesight;
    private Ghoul_AnimationHandler animations;

    public Animator animator;

    private GameObject blythe;

    private AudioSource screamAudio;
    private Vector3 lastPos;
    private Vector3 lastSeen;

    private float minMoveInterval = 2f;
    private float maxMoveInterval = 5f;

    private float lerpDuration;
    int i = 0;

    protected override void Initialize()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        earshot = gameObject.GetComponentInChildren<Ghoul_EnemyEarshot>();
        eyesight = gameObject.GetComponentInChildren<Ghoul_LineOfSight>();
        animations = gameObject.GetComponentInChildren<Ghoul_AnimationHandler>();
        screamAudio = transform.GetChild(5).gameObject.GetComponent<AudioSource>();
        pointList = GameObject.FindGameObjectsWithTag("GhoulPatrol");

        animator = GetComponentInChildren<Animator>();

        blythe = GameObject.Find("Priest");

        SetStateToPatrol();
    }

    //------------------------------ Transitions ------------------------------
    // handle: state transitions, start/stop agent, animation transitions
    // (transitions between states from coroutine IEnumerator methods without update())

    public void SetStateToPatrol()
    {
        currentState = FSMState.Patrol;
        agent.isStopped = false;
        // set walking animation
        StartCoroutine(PatrolCoroutine());
        Debug.Log("Ghoul: Transitioned to patrol state");
    }
    public void SetStateToAlert()
    {
        currentState = FSMState.Alert;
        agent.isStopped = true;
        // set idle animation
        StartCoroutine(AlertCoroutine());
        Debug.Log("Ghoul: Transitioned to alert state");
    }
    public void SetStateToRadiusPatrol()
    {
        currentState = FSMState.RadiusPatrol;
        agent.isStopped = false;
        // set walking animation
        StartCoroutine(RadiusPatrolCoroutine());
        Debug.Log("Ghoul: Transitioned to radius patrol state");
    }
    public void SetStateToScream()
    {
        currentState = FSMState.Scream;
        agent.isStopped = false;
        StartCoroutine(ScreamCoroutine());
        Debug.Log("Ghoul: Transitioned to scream state");
    }
    public void SetStateToDead()
    {
        currentState = FSMState.Dead;
        agent.isStopped = true;
        // animation (?)
        Debug.Log("Ghoul: Transitioned to dead state");
    }

    //------------------------------ States ------------------------------

    // Patrol: Patrol between set points.
    //   - Transition to Alert if sound is heard.
    IEnumerator PatrolCoroutine()
    {
        StartCoroutine(PatrolMovementCoroutine());  // handle movement separately

        while (currentState == FSMState.Patrol)
        {
            if (eyesight.IsInView() == true)
            {
                Debug.Log("in view");
                if (!player.GetComponent<PlayerController>().hiding)
                {
                    Debug.Log("Player is in view and not hiding.");
                    lastSeen = player.GetComponent<Transform>().position;
                    StopCoroutine(PatrolMovementCoroutine());
                    SetStateToScream(); // transition to scream state
                    yield break;
                }
            }
            else if (earshot.IsInEarshot() == true && player.GetComponent<InputsManager>().move != Vector2.zero)
            {
                Debug.Log("Player is in earshot");
                StopCoroutine(PatrolMovementCoroutine());
                SetStateToAlert();  // transition to alert state
                yield break;
            }
            yield return null;
        }
    }

    // handle patrol movement so transition checks happen alongside enemy movement
    IEnumerator PatrolMovementCoroutine()
    {
        while (currentState == FSMState.Patrol)
        {
            animations.PlayWalkAnimation(); // change animation to "walk"
            GameObject destination = pointList[i];
            agent.SetDestination(destination.transform.position);
            //yield return new WaitForSeconds(Random.Range(minMoveInterval, maxMoveInterval));
            while (agent.pathPending || agent.remainingDistance > 0.1f)
            {
                yield return null;
            }
            i = (i + 1) % pointList.Length;
            animations.PlayIdleAnimation(); // change animation to "idle"
            yield return new WaitForSeconds(4.0f);  // pause briefly at the destination (2 default)
        }
    }

    // Alert: Stop briefly and listen.
    //   - Transition to Radius Patrol if sound is heard.
    //   - Transition to Patrol if an amount of time has passed and no sound was heard.
    private IEnumerator AlertCoroutine()
    {
        animations.PlayIdleAnimation(); // change animation to "idle"
        yield return new WaitForSeconds(1.0f);
        float elapsedTime = 0.0f;

        while (currentState == FSMState.Alert && elapsedTime < 5f) //4 seconds default
        {
            if (eyesight.IsInView() == true && !player.GetComponent<PlayerController>().hiding)
            {
                lastSeen = player.GetComponent<Transform>().position;
                SetStateToScream(); // transition to scream state
                yield break;
            }
            // change state if sound is heard from player movement
            else if (earshot.IsInEarshot() == true && player.GetComponent<InputsManager>().move != Vector2.zero)
            {
                lastPos = GetComponent<Transform>().position;   // save enemy location
                SetStateToRadiusPatrol();   // transition to radius patrol state
                yield break;                // exit coroutine
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // if time elapses and no other sounds were heard, transition to patrol state
        SetStateToPatrol();
    }

    // Radius Patrol: Walk to sound source, patrol in radius.
    //   - Transition to Scream if the player is seen for more than a specified time.
    //   - Transition to Patrol if patrol is completed and the player is not seen.
    private IEnumerator RadiusPatrolCoroutine()
    {
        StartCoroutine(RadiusPatrolMovementCoroutine());    // handle movement separately

        //float seenTime = 0.0f;      // amount of time player is in view of enemy

        while (currentState == FSMState.RadiusPatrol)
        {
            if (eyesight.IsInView() == true && !player.GetComponent<PlayerController>().hiding)
            {
                //seenTime += Time.deltaTime;
                StopCoroutine(RadiusPatrolMovementCoroutine());
                lastSeen = player.GetComponent<Transform>().position;
                SetStateToScream();
                yield break;
            }
            /*
            // transition to chase state if player is seen for longer than 2s
            if (seenTime >= 0.5f) // default 2
            {
                lastSeen = player.GetComponent<Transform>().position;
                StopCoroutine(RadiusPatrolMovementCoroutine());
                //screamAudio.Play();
                SetStateToScream();  // transition to scream state
            }
            */
            yield return null;
        }
    }

    // handle radius patrol movement so transition checks happen alongside enemy movement
    private IEnumerator RadiusPatrolMovementCoroutine()
    {
        // move to last heard position
        agent.SetDestination(earshot.GetLastHeardPos());
        animations.PlayWalkAnimation(); // change animation to "walk"
        //yield return new WaitForSeconds(Random.Range(minMoveInterval, maxMoveInterval));
        while (agent.pathPending || agent.remainingDistance > 0.1f) 
        {
            yield return null; 
        }
        Debug.Log("Player's last heard location reached.");
        yield return new WaitForSeconds(5);

        /*
        Vector3[] patrolPoints = FindRPatrolDest();

        // move between patrol destination points
        foreach (Vector3 destination in patrolPoints)
        {
            agent.SetDestination(destination);
            animations.PlayWalkAnimation(); // change animation to "walk"
            Debug.Log("Moving to: " + destination);
            yield return new WaitForSeconds(Random.Range(minMoveInterval, maxMoveInterval)); // wait a random amount of time at the point
            while (agent.pathPending || agent.remainingDistance > 0.1f)
            {
                yield return null;
            }
            animations.PlayIdleAnimation(); // change animation to "idle"
            yield return new WaitForSeconds(Random.Range(minMoveInterval, maxMoveInterval)); // wait a random amount of time at the point
        }

        // if radius patrol is complete and player was not seen

        // stop radius patrol coroutine
        //StopCoroutine(RadiusPatrolCoroutine());

        // TODO --> blind spot - enemy is neither listening nor looking for player from here
        //          until transition to patrol state

        // turn around
        //yield return StartCoroutine(Rotate180()); //temp
        */

        // return to last position before noise was heard
        agent.SetDestination(lastPos);
        animations.PlayWalkAnimation(); // change animation to "walk"
        while (agent.pathPending || agent.remainingDistance > 0.1f)
        {
            yield return null;
        }
        animations.PlayIdleAnimation(); // change animation to "idle"
        yield return new WaitForSeconds(5); // wait a random amount of time at the point

        // transition to patrol state
        SetStateToPatrol();
    }

    // TODO calculate radius patrol destination points
    private Vector3[] FindRPatrolDest()
    {
        Vector3 position = earshot.GetLastHeardPos();

        int numOfPoints = 3; // number of points the enemy will patrol in that location
        Vector3[] points = new Vector3[numOfPoints]; // will hold the radius patrol points to be returned
        for(int i = 0; i < numOfPoints; i++)
        {
            float x = Random.Range(-4, 4);
            float z = Random.Range(-4, 4);
            Vector3 point = new Vector3(position.x + x, position.y, position.z + z);
            points[i] = point;
        }

        // debugging
        for (int i = 0; i < numOfPoints; i++)
        {
            Debug.Log(points[i]);
        }

        return points;
    }

    private IEnumerator ScreamCoroutine()
    {
        StartCoroutine(StareAtPlayer()); // keep staring at player
        //blythe.GetComponent<EnemyAIFSMTest>().StopAllCoroutines(); // stop whatever Blythe is doing
        //StartCoroutine(BlytheKeepMoving());
        //blythe.GetComponent<EnemyAIFSMTest>().StopCoroutines();
        //StartCoroutine(blythe.GetComponent<EnemyAIFSMTest>().GoToPoint(lastSeen));
        blythe.GetComponent<EnemyAIFSMTest>().StopCoroutines();
        StartCoroutine(BlytheKeepMoving());

        /*
        int count = 0;
        while (count < 2) // play scream 2 times
        {
            animations.PlayScreamAnimation(); // change animation to "scream"
            screamAudio.Play(); // play scream audio
            yield return new WaitForSeconds(7); // wait 7 seconds (scream is 7 seconds long)

            count++;
            yield return null;
        }
        */

        animations.PlayScreamAnimation(); // change animation to "scream"
        screamAudio.Play(); // play scream audio
        yield return new WaitForSeconds(7); // wait 7 seconds (scream is 7 seconds long)

        StopCoroutine(StareAtPlayer()); // stop staring at player
        SetStateToPatrol(); // go back to patrolling
    }

    //------------------------------ Helper ------------------------------

    private IEnumerator BlytheKeepMoving()
    {
        blythe.GetComponent<AnimationHandler>().PlayRunAnimation(); // make Blythe run

        bool reached = false;
        while (reached == false)
        {
            blythe.GetComponent<NavMeshAgent>().SetDestination(lastSeen); // move Blythe to player's last seen position

            // if player is seen while moving to ghoul
            if (blythe.GetComponent<LineOfSight>().inView == true && !player.GetComponent<PlayerController>().hiding)
            {
                blythe.GetComponent<EnemyAIFSMTest>().SetStateToChase(); // chase player
                yield break; // stop this coroutine
            }

            if (blythe.GetComponent<NavMeshAgent>().remainingDistance < 0.5)
            {
                Debug.Log("Player's last seen position reached.");
                reached = true;
            }

            yield return null;
        }

        //blythe.transform.GetChild(2).gameObject.GetComponent<EnemyEarshot>().lastHeard = lastSeen; // helper for patrol radius
        //blythe.GetComponent<EnemyAIFSMTest>().SetStateToRadiusPatrol(); // go to where player was seen, patrol radius
        //yield return new WaitForSeconds(0.5f);
        //blythe.GetComponent<AnimationHandler>().PlayIdleAnimation(); // change animation to "idle"

        blythe.GetComponent<EnemyAIFSMTest>().SetStateToSilent();

        yield return null;
    }

    private Vector3 GetRandomPositionOnNavMesh()
    {
        NavMeshHit hit;
        Vector3 randomPosition = Vector3.zero;
        if (NavMesh.SamplePosition(new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f)), out hit, 20f, NavMesh.AllAreas))
        {
            randomPosition = hit.position;
        }
        return randomPosition;
    }

    private IEnumerator Rotate180()
    {
        float timeElapsed = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        while (timeElapsed < lerpDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
    }

    private IEnumerator StareAtPlayer()
    {
        while(currentState == FSMState.Scream)
        {
            agent.SetDestination(player.position); // "rotate" to face player (walking very slowly)
            yield return null;
        }
    }

    private IEnumerator MoveRandomly()
    {
        animations.PlayWalkAnimation(); // change animation to "walk"
        while (true)
        {
            Vector3 randomPosition = GetRandomPositionOnNavMesh();
            agent.SetDestination(randomPosition);

            yield return new WaitForSeconds(Random.Range(minMoveInterval, maxMoveInterval));

            // Wait until the agent reaches the destination
            while (agent.pathPending || agent.remainingDistance > 0.1f)
            {
                yield return null;
            }
        }
    }

}