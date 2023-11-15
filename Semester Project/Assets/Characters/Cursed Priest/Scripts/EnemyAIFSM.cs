using System.Collections;
using UnityEngine;
using UnityEngine.AI;
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

public class EnemyAIFSM : BaseFSM
{
    public enum FSMState
    {
        Patrol,
        Alert,
        RadiusPatrol,
        Chase,
        Silent,
        Dead
    }

    public FSMState currentState;

    private Transform player;
    private NavMeshAgent agent;

    private EnemyEarshot earshot;
    private LineOfSight eyesight;
    private AnimationHandler animations;

    private AudioSource screamAudio;
    private Vector3 lastPos;

    private float minMoveInterval = 2f;
    private float maxMoveInterval = 5f;

    private float lerpDuration;
    int i = 0;

    protected override void Initialize()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        earshot = gameObject.GetComponentInChildren<EnemyEarshot>();
        eyesight = gameObject.GetComponentInChildren<LineOfSight>();
        animations = gameObject.GetComponentInChildren<AnimationHandler>();
        screamAudio = GetComponent<AudioSource>();
        pointList = GameObject.FindGameObjectsWithTag("PatrolPoint");
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
        Debug.Log("Transitioned to patrol state");
    }
    public void SetStateToAlert()
    {
        currentState = FSMState.Alert;
        agent.isStopped = true;
        // set idle animation
        StartCoroutine(AlertCoroutine());
        Debug.Log("Transitioned to alert state");
    }
    public void SetStateToRadiusPatrol()
    {
        currentState = FSMState.RadiusPatrol;
        agent.isStopped = false;
        // set walking animation
        StartCoroutine(RadiusPatrolCoroutine());
        Debug.Log("Transitioned to radius patrol state");
    }
    public void SetStateToChase()
    {
        currentState = FSMState.Chase;
        agent.isStopped = false;
        // set walking/running animation
        StartCoroutine(ChaseCoroutine());
        Debug.Log("Transitioned to chase state");
    }
    public void SetStateToSilent()
    {
        currentState = FSMState.Silent;
        agent.isStopped = true;
        // set idle animation
        StartCoroutine(SilentCoroutine());
        Debug.Log("Transitioned to silent state");
    }
    public void SetStateToDead()
    {
        currentState = FSMState.Dead;
        agent.isStopped = true;
        // animation (?)
        Debug.Log("Transitioned to dead state");
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
                StopCoroutine(PatrolMovementCoroutine());
                SetStateToChase(); // transition to chase state
            }
            else if (earshot.IsInEarshot() == true)
            {
                StopCoroutine(PatrolMovementCoroutine());
                SetStateToAlert();  // transition to alert state
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
            yield return new WaitForSeconds(Random.Range(minMoveInterval, maxMoveInterval));
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
        yield return new WaitForSeconds(1.0f);
        float elapsedTime = 0.0f;
        animations.PlayIdleAnimation(); // change animation to "idle"

        while (currentState == FSMState.Alert && elapsedTime < 5f) //4 seconds default
        {
            // change state if sound is heard from player movement
            if (earshot.IsInEarshot() == true && player.GetComponent<InputsManager>().move != Vector2.zero)
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
    //   - Transition to Chase if the player is seen for more than a specified time.
    //   - Transition to Patrol if patrol is completed and the player is not seen.
    private IEnumerator RadiusPatrolCoroutine()
    {
        StartCoroutine(RadiusPatrolMovementCoroutine());    // handle movement separately

        float seenTime = 0.0f;      // amount of time player is in view of enemy

        while (currentState == FSMState.RadiusPatrol)
        {
            if (eyesight.IsInView() == true && !player.GetComponent<PlayerController>().hiding)
            {
                seenTime += Time.deltaTime;
            }
            // transition to chase state if player is seen for longer than 2s
            if (seenTime >= 2.0f)
            {
                StopCoroutine(RadiusPatrolMovementCoroutine());
                //screamAudio.Play();
                SetStateToChase();  // transition to chase state
            }
            yield return null;
        }
    }

    // handle radius patrol movement so transition checks happen alongside enemy movement
    private IEnumerator RadiusPatrolMovementCoroutine()
    {
        animations.PlayWalkAnimation(); // change animation to "walk"
        // move to last heard position
        agent.SetDestination(earshot.GetLastHeardPos());
        yield return new WaitForSeconds(Random.Range(minMoveInterval, maxMoveInterval));
        while (agent.pathPending || agent.remainingDistance > 0.1f) 
        {
            yield return null; 
        }

        Vector3[] patrolPoints = FindRPatrolDest();

        // move between patrol destination points
        foreach (Vector3 destination in patrolPoints)
        {
            agent.SetDestination(destination);
            yield return new WaitForSeconds(Random.Range(minMoveInterval, maxMoveInterval));
            while (agent.pathPending || agent.remainingDistance > 0.1f)
            {
                yield return null;
            }
        }

        // if radius patrol is complete and player was not seen

        // stop radius patrol coroutine
        StopCoroutine(RadiusPatrolCoroutine());

        // TODO --> blind spot - enemy is neither listening nor looking for player from here
        //          until transition to patrol state

        // turn around
        /////////////////////////yield return StartCoroutine(Rotate180()); temp

        // return to last position
        agent.SetDestination(lastPos);
        while (agent.pathPending || agent.remainingDistance > 0.1f)
        {
            yield return null;
        }

        // transition to patrol state
        SetStateToPatrol();
    }

    // TODO calculate radius patrol destination points
    private Vector3[] FindRPatrolDest()
    {
        // temp test values
        Vector3 position = earshot.GetLastHeardPos();
        //return new Vector3[] { position + new Vector3(0, 0, 3), position + new Vector3(0, 0, -3) };

        Vector3[] points = { }; // will hold the radius patrol points to be returned
        for(int i = 0; i < 4; i++)
        {
            int x = Random.Range(0, 5);
            int z = Random.Range(0, 5);
            points[i] = new Vector3(x, position.y, z);
        }

        return points;
    }

    // Chase State: Scream and chase the player.
    //   - Transition to Silent if line of sight is broken for a specified time.
    private IEnumerator ChaseCoroutine()
    {
        animations.PlayRunAnimation(); // change animation to "run"
        float notSeenTime = 0.0f;

        while (currentState == FSMState.Chase)
        {
            agent.SetDestination(player.position);  // chase player
                // something about changing the animation + speed
            
            // if player is not in view
            if (eyesight.IsInView() == false)
            {
                notSeenTime += Time.deltaTime;

                // transition to silent state if line of sight is broken for 8s
                if (notSeenTime >= 8.0f)
                {
                    SetStateToSilent(); // transition to silent state
                    yield break;
                }
            }
            // if player is in view
            else if (notSeenTime > 0)
            {
                notSeenTime = 0;        // reset time counter
            }
            yield return null;
        }
    }

    // Silent: Pause after chasing the player.
    //   - Transition to Chase if the player is seen again for more than a specified time.
    //   - Transition to Patrol if a specified time has passed and the player was not seen.
    private IEnumerator SilentCoroutine()
    {
        animations.PlayIdleAnimation(); // change animation to "idle"
        yield return new WaitForSeconds(1.0f);
        float elapsedTime = 0.0f;

        while (currentState == FSMState.Silent && elapsedTime < 3.0f)
        {
            if (eyesight.IsInView() == true)
            {
                SetStateToChase();  // change state
                yield break;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // if time elapses and player is no longer seen, transition to patrol state
        SetStateToPatrol();
    }

    //------------------------------ Helper ------------------------------

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

    private IEnumerator MoveRandomly()
    {
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