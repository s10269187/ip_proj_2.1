using UnityEngine;
using TMPro;

public class PlaceAndTransferMaterial : MonoBehaviour
{
    [SerializeField] Material newMat;

    [Header("Target Settings")]
    public GameObject targetObject;

    [Header("Allowed Item")]
    [SerializeField] private GameObject allowedItem;

    [Header("Task UI")]
    public TextMeshProUGUI taskToStrike; // Assign in Inspector

    public void PlaceObject(GameObject heldObject)
    {
        if (heldObject == null || targetObject == null)
        {
            Debug.LogWarning("Held or target object is null.");
            return;
        }

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

        PickUp pickUp = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PickUp>();
        if (pickUp != null)
        {
            pickUp.ForceDropHeldObject();
        }

        Destroy(heldObject);
        Debug.Log("Allowed item destroyed.");

        // ✅ Strike through the task
        if (taskToStrike != null)
        {
            taskToStrike.text = $"<s>{taskToStrike.text}</s>";
            taskToStrike.color = Color.gray; // Optional: dim the text
        }
    }
}
