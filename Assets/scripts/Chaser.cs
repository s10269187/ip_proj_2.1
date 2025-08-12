using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : MonoBehaviour
{
    NavMeshAgent myAgent;

    private Coroutine stateCoroutine;

    [SerializeField]
    Transform playerTransform;

    public string currentState;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float idleTime = 2f;

    private int currentPatrolIndex = 0;

    public float minIdleTime = 5f;

    public float maxIdleTime = 8f;

    private Animator animator;
    
    [Header("Detection Settings")]
    public float detectionRadius = 5f;

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



    void Update()
    {
        if (playerTransform == null)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    playerTransform = hit.transform;
                    StartCoroutine(SwitchState("ChaseTarget"));
                    break;
                }
            }
        }
    }

    IEnumerator SwitchState(string newState)
    {
        if (currentState == newState)
            yield break;

        animator.SetBool("isWalking", true); 

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
            case "ChaseTarget":
                stateCoroutine = StartCoroutine(ChaseTarget());
                break;
        }
    }

    IEnumerator Idle()
    {
        float timer = 0f;
        animator.SetTrigger("isidle");

        while (currentState == "Idle")
        {
            if (playerTransform != null)
            {
                StartCoroutine(SwitchState("ChaseTarget")); 
                yield break;
            }

            timer += Time.deltaTime;
            if (timer >= idleTime)
            {
                StartCoroutine(SwitchState("Patrol"));
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator Patrol()
    {
        while (currentState == "Patrol")
        {
            animator.SetTrigger("iswalking");
            myAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
            if (playerTransform != null)
            {
                StartCoroutine(SwitchState("ChaseTarget"));
                yield break;
            }

            if (patrolPoints.Length == 0)
            {
                Debug.LogWarning("No patrol points assigned.");
                StartCoroutine(SwitchState("Idle"));
                yield break;
            }

            Transform targetPoint = patrolPoints[currentPatrolIndex];
            myAgent.SetDestination(targetPoint.position);

            // Wait until the chaser reaches the patrol point
            while (myAgent.pathPending || myAgent.remainingDistance > myAgent.stoppingDistance)
            {
                if (playerTransform != null)
                {
                    StartCoroutine(SwitchState("ChaseTarget"));
                    yield break;
                }
                yield return null;
            }

            // Once reached the point
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            StartCoroutine(SwitchState("Idle"));
            yield break;
        }
    }

    IEnumerator ChaseTarget()
    {
        animator.SetTrigger("iswalking");
        while (currentState == "ChaseTarget")
        {
            if (playerTransform == null)
            {
                StartCoroutine(SwitchState("Patrol"));
                yield break;
            }

            myAgent.SetDestination(playerTransform.position);
            yield return null;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            playerTransform = other.transform;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            playerTransform = null;
    }
}
