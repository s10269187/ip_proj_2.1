using UnityEngine;

public class PlaceAndTransferMaterial : MonoBehaviour
{
    [SerializeField]
    Material newMat;

    [Header("Target Settings")]
    public GameObject targetObject; // The object to receive the material

    [Header("Allowed Item")]
    [SerializeField] private GameObject allowedItem; // Assign the only item that can transfer material

    public void PlaceObject(GameObject heldObject)
    {
        if (heldObject == null || targetObject == null)
        {
            Debug.LogWarning("Held or target object is null.");
            return;
        }

        // Only allow the assigned item to transfer material and be destroyed
        if (heldObject != allowedItem)
        {
            Debug.Log("This item is not allowed to transfer material.");
            return;
        }

        Renderer heldRenderer = heldObject.GetComponent<Renderer>();
        Renderer targetRenderer = gameObject.GetComponent<Renderer>();

        if (heldRenderer != null && targetRenderer != null)
        {
            targetRenderer.material = newMat;
            Debug.Log("Material transferred from " + heldObject.name + " to " + targetObject.name);
        }
        else
        {
            Debug.LogWarning("Missing Renderer on held or target object.");
        }

        // Drop and destroy the allowed item
        PickUp pickUp = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PickUp>();
        if (pickUp != null)
        {
            pickUp.ForceDropHeldObject(); // Unparent and clear reference
        }

        Destroy(heldObject);
        Debug.Log("Allowed item destroyed.");
    }
}