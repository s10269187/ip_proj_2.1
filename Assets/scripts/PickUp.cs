using UnityEngine;

public class PickUp : MonoBehaviour
{
    // Reference to player object
   public GameObject player; 
   // Position where held object stays
    public Transform holdPos;

    [Header("Pickup Settings")]
    // Force applied when throwing
    public float throwForce = 500f; 
    // Max distance to pick up objects
    public float pickUpRange = 5f;

    [Header("Held Object Positioning")]
    // Local position offset
    public Vector3 heldLocalPosition = Vector3.zero; 
    // Local rotation offset
    public Vector3 heldLocalRotation = Vector3.zero; 

    // Currently held object
    public GameObject heldObj; 
    // Rigidbody of held object
    private Rigidbody heldObjRb; 
    // Whether object can be dropped
    private bool canDrop = true; 
    // Layer used for held object
    private int LayerNumber; 

    void Start()
    {
        // Get layer index
        LayerNumber = LayerMask.NameToLayer("holdLayer"); 
    }

    void Update()
    {
        // Press R to pick up or drop
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (heldObj == null)
            {
                // Raycast forward to find object
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    if (hit.transform.gameObject.tag == "canPickUp")
                    {
                        // Pick up object
                        PickUpObject(hit.transform.gameObject); 
                    }
                }
            }
            else
            {
                if (canDrop)
                {
                    // Prevent clipping
                    // Drop object
                    StopClipping(); 
                    DropObject(); 
                }
            }
        }

        // If holding an object
        if (heldObj != null)
        {
            // Keep object in hand position
            MoveObject();

            // Left click to throw
            // Prevent clipping
            // Throw object
            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop)
            {
                StopClipping();
                ThrowObject();
            }
        }
    }

    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>())
        {
            // Disable physics
            heldObj = pickUpObj;
            heldObjRb = pickUpObj.GetComponent<Rigidbody>();
            heldObjRb.isKinematic = true; 

            // Parent to hold position
            heldObj.transform.SetParent(holdPos); 
            heldObj.transform.localPosition = heldLocalPosition;
            heldObj.transform.localRotation = Quaternion.Euler(heldLocalRotation);

            // Set layer to Ignore Raycast
            heldObj.layer = LayerMask.NameToLayer("Ignore Raycast"); 
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true); // Ignore player collision

            Debug.Log("Picked up: " + heldObj.name + " and set to Ignore Raycast layer.");
        }
        else
        {
            Debug.LogWarning("Tried to pick up object without Rigidbody: " + pickUpObj.name);
        }
    }

    void DropObject()
    {
        // Re-enable collision
        // Default layer
        // Re-enable physics
        // Unparent
        // Clear reference
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false); 
        heldObj.layer = 0; 
        heldObjRb.isKinematic = false; 
        heldObj.transform.parent = null; 
        heldObj = null; 
    }

    void MoveObject()
    {
        // Match hold position
        // Match hold rotation
        heldObj.transform.position = holdPos.position; 
        heldObj.transform.rotation = holdPos.rotation; 
    }

    void ThrowObject()
    {
        // Re-enable collision
        // Default layer
        // Re-enable physics
        // Unparent
        // Apply force in forward direction
        // Clear reference
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false); 
        heldObj.layer = 0; 
        heldObjRb.isKinematic = false; 
        heldObj.transform.parent = null; 
        heldObjRb.AddForce(transform.forward * throwForce); 
        heldObj = null; 
    }

    void StopClipping()
    {
        // Distance to held object
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); 
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);

        if (hits.Length > 1)
        {
            // Move object slightly down to avoid clipping
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);
        }
    }

    public void ForceDropHeldObject()
    {
        if (heldObj != null)
        {
            // Re-enable collision
            // Default layer
            // Re-enable physics
            // Unparent
            // Clear reference
            // Clear rigidbody reference
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false); 
            heldObj.layer = 0; 
            heldObjRb.isKinematic = false; 
            heldObj.transform.parent = null; 
            heldObj = null; 
            heldObjRb = null; 
        }
    }
}
