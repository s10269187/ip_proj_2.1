using UnityEngine;

public class PlaceAndTransferMaterial : MonoBehaviour
{
    [Header("Held Object Settings")]
    public GameObject heldObject; // The object currently in hand

    [Header("Target Settings")]
    public GameObject targetObject; // The object to receive the material
    public KeyCode placeKey = KeyCode.R;

    private bool isHolding = true;

    void Update()
    {
        if (isHolding && Input.GetKeyDown(placeKey))
        {
            PlaceObject();
        }
    }

    void PlaceObject()
    {
        if (heldObject == null || targetObject == null) return;

        Renderer heldRenderer = heldObject.GetComponent<Renderer>();
        Renderer targetRenderer = targetObject.GetComponent<Renderer>();

        if (heldRenderer != null && targetRenderer != null)
        {
            // Clone the material to avoid shared reference issues
            Material newMat = new Material(heldRenderer.material);
            targetRenderer.material = newMat;
        }

        // Destroy the held object
        Destroy(heldObject);
        isHolding = false;

        Debug.Log("Object placed and material transferred.");
    }
}