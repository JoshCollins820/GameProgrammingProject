using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public bool inView = false;

    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    private Transform target;

    private void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        StartCoroutine(FindPlayer());
    }

    public bool IsInView() { return inView; }

    public Transform GetTarget() { return target; }

    IEnumerator FindPlayer()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            TargetInView();
        }
    }

    void TargetInView()
    {
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        float angleToTarget = Vector3.Angle(transform.forward, dirToTarget);
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // if player is within line of sight
        if (angleToTarget < viewAngle / 2 && distanceToTarget < viewRadius && target.GetComponent<PlayerController>().hiding == false)
        {
            // if there are no objects blocking line of sight to player
            if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget))
            {
                if (inView == false)
                {
                    inView = true;
                    Debug.Log("Player is in view.");
                }
            }
            else
            {
                if (inView == true)
                {
                    inView = false;
                    Debug.Log("Player is no longer in view.");
                }
            }
        }
        else
        {
            if (inView == true)
            {
                inView = false;
                Debug.Log("Player is no longer in view.");
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

}
