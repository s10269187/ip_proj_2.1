using UnityEngine;

public class Matchbox : MonoBehaviour
{
    [Header("Held Positioning")]
    public Transform handAnchor;
    public Vector3 heldLocalPosition = Vector3.zero;
    public Vector3 heldLocalRotation = Vector3.zero;

    private void Start()
    {
        // Attach matchbox to hand and apply initial transform
        transform.SetParent(handAnchor);
        ApplyHeldTransform();
    }

    private void Update()
    {
        // Continuously apply position and rotation in case values are changed in Inspector
        ApplyHeldTransform();
    }

    private void ApplyHeldTransform()
    {
        transform.localPosition = heldLocalPosition;
        transform.localRotation = Quaternion.Euler(heldLocalRotation);
    }
}
