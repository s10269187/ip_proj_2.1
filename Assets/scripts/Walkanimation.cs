using UnityEngine;

// Controls walking and idle animation based on movement
public class Walkanimation : MonoBehaviour
{
    // Minimum speed to trigger walking
    [Header("Movement Settings")]
    public float movementThreshold = 0.1f; 
    // Animation controller
    private Animator animator; 
     // Physics body
    private Rigidbody rb;     

    void Start()
    {
        // Get Animator
        // Get Rigidbody
        animator = GetComponent<Animator>(); 
        rb = GetComponent<Rigidbody>();      
    }

    void Update()
    {
        // Skip if missing
        if (animator == null || rb == null) return; 

        // Get horizontal movement only
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        // Check if moving
        bool isWalking = horizontalVelocity.magnitude > movementThreshold;

        // Set animation states
        // Walking animation
        // Idle animation
        animator.SetBool("iswalking", isWalking);   
        animator.SetBool("isidle", !isWalking);     
    }
}
