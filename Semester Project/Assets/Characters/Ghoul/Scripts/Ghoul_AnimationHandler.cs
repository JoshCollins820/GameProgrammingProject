using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghoul_AnimationHandler : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;


    // Start is called before the first frame update
    private void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();
        // Get the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
}

    public void PlayIdleAnimation()
    {
        animator.SetFloat("speed", 0);
        agent.speed = 0f;
    }

    public void PlayWalkAnimation()
    {
        animator.SetFloat("speed", 1);
        agent.speed = 1.0f;//1.5 blythe, 2 player
    }

    public void PlayScreamAnimation()
    {
        animator.SetFloat("speed", 2);
        agent.speed = 0.1f;
    }
}
