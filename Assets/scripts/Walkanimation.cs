using UnityEngine;

public class Walkanimation : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementThreshold = 0.1f;

    private Animator animator;
    private Rigidbody rb;

    void Start()
    {
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
}
