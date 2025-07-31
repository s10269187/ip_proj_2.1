using UnityEngine;

public class BedroomDoor : MonoBehaviour
{
    public Transform player;
    public float interactDistance = 3f;
    public float openAngle = 90f;
    public float speed = 2f;
    public KeyCode interactKey = KeyCode.E;

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
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactDistance && Input.GetKeyDown(interactKey))
        {
            doorIsOpen = !doorIsOpen;
        }

        Quaternion targetRotation = doorIsOpen ? openRotation : initialRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
    }
}
