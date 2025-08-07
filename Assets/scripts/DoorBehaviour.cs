using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = 90f;
    public float speed = 2f;

    private Quaternion initialRotation;
    private Quaternion openRotation;
    private bool doorIsOpen = false;

    void Start()
    {
        initialRotation = transform.rotation;
        openRotation = Quaternion.Euler(0f, openAngle, 0f) * initialRotation;
    }

    void Update()
    {
        Quaternion targetRotation = doorIsOpen ? openRotation : initialRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
    }

    public void ToggleDoor()
{
    doorIsOpen = !doorIsOpen;
    Debug.Log("Door toggled. Now open? " + doorIsOpen);
}
}
