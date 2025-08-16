using UnityEngine;

/// <summary>
/// Controls the rotation of a door between open and closed states.
/// </summary>
public class DoorBehaviour : MonoBehaviour
{
    /// <summary>
    /// The angle in degrees to rotate the door when opening.
    /// </summary>
    [Header("Door Settings")]
    public float openAngle = 90f;

    /// <summary>
    /// The speed at which the door rotates.
    /// </summary>
    public float speed = 2f;

    /// <summary>
    /// The initial rotation of the door when the scene starts.
    /// </summary>
    private Quaternion initialRotation;

    /// <summary>
    /// The target rotation of the door when fully open.
    /// </summary>
    private Quaternion openRotation;

    /// <summary>
    /// Indicates whether the door is currently open.
    /// </summary>
    private bool doorIsOpen = false;

    /// <summary>
    /// Initializes the door's rotation settings.
    /// </summary>
    void Start()
    {
        initialRotation = transform.rotation;
        openRotation = Quaternion.Euler(0f, openAngle, 0f) * initialRotation;
    }

    /// <summary>
    /// Smoothly rotates the door toward its target rotation based on its open state.
    /// </summary>
    void Update()
    {
        Quaternion targetRotation = doorIsOpen ? openRotation : initialRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
    }

    /// <summary>
    /// Toggles the door between open and closed states.
    /// </summary>
    public void ToggleDoor()
    {
        doorIsOpen = !doorIsOpen;
    }
}
