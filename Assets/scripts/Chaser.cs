using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : MonoBehaviour
{
    NavMeshAgent myAgent;
    private Coroutine stateCoroutine;

    public string currentState;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float idleTime = 2f;

    private int currentPatrolIndex = 0;
    private Animator animator;

    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        myAgent.autoBraking = true;

        if (patrolPoints.Length > 0)
            StartCoroutine(SwitchState("Patrol"));
        else
            StartCoroutine(SwitchState("Idle"));
    }

    IEnumerator SwitchState(string newState)
    {
        if (currentState == newState)
            yield break;

        if (stateCoroutine != null)
            StopCoroutine(stateCoroutine);

        currentState = newState;

        switch (newState)
        {
            case "Idle":
                stateCoroutine = StartCoroutine(Idle());
                break;
            case "Patrol":
                stateCoroutine = StartCoroutine(Patrol());
                break;
        }
    }

    IEnumerator Idle()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("Idle", true);

        yield return new WaitForSeconds(idleTime);

        yield return StartCoroutine(SwitchState("Patrol"));
    }

    IEnumerator Patrol()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("isWalking", true);

        if (patrolPoints.Length == 0)
        {
            Debug.LogWarning("No patrol points assigned.");
            yield return StartCoroutine(SwitchState("Idle"));
            yield break;
        }

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        myAgent.SetDestination(targetPoint.position);

        // Wait until the agent reaches the destination
        while (myAgent.pathPending || myAgent.remainingDistance > myAgent.stoppingDistance)
        {
            yield return null;
        }

        // Move to the next point
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;

        // Switch to idle state
        yield return StartCoroutine(SwitchState("Idle"));
    }
}
