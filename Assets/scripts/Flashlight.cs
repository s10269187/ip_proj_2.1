using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("Pickup Settings")]
    [Tooltip("Maximum distance to pick up the flashlight")]
    public float pickupRange = 3f;

    [Tooltip("Layer mask for objects that can be picked up")]
    public LayerMask pickupLayer;

    [Tooltip("Transform where the flashlight will be held")]
    public Transform hand;

    [Tooltip("Tag used to identify pickable objects")]
    public string pickupTag = "canPickUp";

    [Tooltip("Key used to pick up and drop the flashlight")]
    public KeyCode pickupKey = KeyCode.R;

    private GameObject heldItem;
    private Rigidbody heldRb;

    void Update()
    {
        // Use customizable key to pick up or drop
        if (Input.GetKeyDown(pickupKey))
        {
            if (heldItem == null)
            {
                TryPickup();
            }
            else
            {
                Drop();
            }
        }
    }

    void TryPickup()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, pickupLayer))
        {
            // Check for custom tag
            if (hit.collider.CompareTag(pickupTag))
            {
                heldItem = hit.collider.gameObject;
                heldRb = heldItem.GetComponent<Rigidbody>();

                if (heldRb != null)
                {
                    // Attach to hand
                    heldRb.isKinematic = true;
                    heldRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                    heldRb.interpolation = RigidbodyInterpolation.Interpolate;

                    heldItem.transform.SetParent(hand);
                    heldItem.transform.localPosition = Vector3.zero;
                    heldItem.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    Debug.LogWarning("Picked object has no Rigidbody.");
                }
            }
        }
    }

    void Drop()
    {
        if (heldItem != null && heldRb != null)
        {
            // Detach and enable physics
            heldItem.transform.SetParent(null);
            heldRb.isKinematic = false;
            heldRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            heldRb.interpolation = RigidbodyInterpolation.Interpolate;
            heldRb.linearVelocity = Vector3.zero;

            // Slightly raise to avoid clipping into ground
            heldItem.transform.position += Vector3.up * 0.1f;
        }

        heldItem = null;
        heldRb = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * pickupRange);
        Gizmos.DrawWireSphere(transform.position + transform.forward * pickupRange, 0.1f);
    }
}
