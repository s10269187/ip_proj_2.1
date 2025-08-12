using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : MonoBehaviour
{
    NavMeshAgent myAgent;
    private Rigidbody rb;
    public float movementThreshold = 0.1f;
    [SerializeField]
    Transform targetTransform;
    private Animator animator;
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
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (animator == null)
            Debug.LogWarning("Animator not found on player.");
        if (rb == null)
            Debug.LogWarning("Rigidbody not found on player.");
    }
    void Update()
    {
        if (animator == null || rb == null) return;

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        bool isWalking = horizontalVelocity.magnitude > movementThreshold;

        animator.SetBool("iswalking", isWalking);
        animator.SetBool("isidle", !isWalking);
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
