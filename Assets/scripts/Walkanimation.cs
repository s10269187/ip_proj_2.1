using UnityEngine;

/// <summary>
/// Controls walking and idle animations based on the player's movement velocity.
/// </summary>
public class Walkanimation : MonoBehaviour
{
    /// <summary>
    /// Minimum horizontal speed required to trigger the walking animation.
    /// </summary>
    [Header("Movement Settings")]
    public float movementThreshold = 0.1f;

    /// <summary>
    /// Reference to the Animator component controlling animations.
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Reference to the Rigidbody component used to measure movement.
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Initializes references to Animator and Rigidbody components.
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Updates animation states based on the player's horizontal movement.
    /// </summary>
    void Update()
    {
        if (animator == null || rb == null) return;

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        bool isWalking = horizontalVelocity.magnitude > movementThreshold;

        animator.SetBool("iswalking", isWalking);
        animator.SetBool("isidle", !isWalking);
    }
}
