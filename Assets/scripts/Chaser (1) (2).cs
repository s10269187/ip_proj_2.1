using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : MonoBehaviour
{
    NavMeshAgent myAgent;

    [SerializeField]
    Transform targetTransform;

    public string currentState;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float idleTime = 2f;

    private int currentPatrolIndex = 0;

    void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
        myAgent.autoBraking = true;
    }

    void Start()
    {
        StartCoroutine(SwitchState("Idle"));
    }

    IEnumerator SwitchState(string newState)
    {
        if (currentState == newState)
            yield break;

        StopAllCoroutines(); // Stop previous state
        currentState = newState;

        StartCoroutine(newState);
    }

    IEnumerator Idle()
    {
        float timer = 0f;

        while (currentState == "Idle")
        {
            if (targetTransform != null)
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
            if (targetTransform != null)
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
                if (targetTransform != null)
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
        while (currentState == "ChaseTarget")
        {
            if (targetTransform == null)
            {
                StartCoroutine(SwitchState("Idle"));
                yield break;
            }

            myAgent.SetDestination(targetTransform.position);
            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            targetTransform = other.transform;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            targetTransform = null;
    }
}
