using UnityEngine;

public class Flashlight: MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public LayerMask pickupLayer;
    public Transform hand;

    private GameObject heldItem;
    private Rigidbody heldRb;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
            if (hit.collider.CompareTag("Pickup"))
            {
                heldItem = hit.collider.gameObject;
                heldRb = heldItem.GetComponent<Rigidbody>();

                // Attach to hand
                heldRb.isKinematic = true;
                heldRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                heldRb.interpolation = RigidbodyInterpolation.Interpolate;

                heldItem.transform.SetParent(hand);
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;
            }
        }
    }

    void Drop()
    {
        // Detach and enable physics
        heldItem.transform.SetParent(null);
        heldRb.isKinematic = false;
        heldRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        heldRb.interpolation = RigidbodyInterpolation.Interpolate;
        heldRb.linearVelocity = Vector3.zero;

        // Slightly raise to avoid clipping into ground
        heldItem.transform.position += Vector3.up * 0.1f;

        heldItem = null;
        heldRb = null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * pickupRange);
    }
}
