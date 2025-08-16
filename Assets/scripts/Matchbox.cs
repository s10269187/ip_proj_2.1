using UnityEngine;

/// <summary>
/// Attaches the matchbox to a hand anchor and maintains its local position and rotation offsets.
/// </summary>
public class Matchbox : MonoBehaviour
{
    /// <summary>
    /// The transform of the hand to which the matchbox will be attached.
    /// </summary>
    [Header("Held Positioning")]
    public Transform handAnchor;

    /// <summary>
    /// The local position offset of the matchbox when held.
    /// </summary>
    public Vector3 heldLocalPosition = Vector3.zero;

    /// <summary>
    /// The local rotation offset of the matchbox when held.
    /// </summary>
    public Vector3 heldLocalRotation = Vector3.zero;

    /// <summary>
    /// Attaches the matchbox to the hand anchor and applies the initial transform.
    /// </summary>
    private void Start()
    {
        transform.SetParent(handAnchor);
        ApplyHeldTransform();
    }

    /// <summary>
    /// Continuously updates the matchbox's local position and rotation to stay aligned with the hand.
    /// </summary>
    private void Update()
    {
        ApplyHeldTransform();
    }

    /// <summary>
    /// Applies the specified local position and rotation offsets to the matchbox.
    /// </summary>
    private void ApplyHeldTransform()
    {
        transform.localPosition = heldLocalPosition;
        transform.localRotation = Quaternion.Euler(heldLocalRotation);
    }
}
