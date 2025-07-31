using UnityEngine;
using TMPro;

public class BathroomDoor : MonoBehaviour
{
    public Transform player;
    public float interactDistance = 3f;
    public float openAngle = 90f;
    public float speed = 2f;
    public KeyCode interactKey = KeyCode.E;
    public TextMeshProUGUI promptText;

    private Quaternion initialRotation;
    private Quaternion openRotation;
    private bool doorIsOpen = false;

    void Start()
    {
        initialRotation = transform.rotation;
        openRotation = Quaternion.Euler(0f, openAngle, 0f) * initialRotation;
        if (promptText != null)
            promptText.enabled = false;
    }

    void Update()
    {
    float distance = Vector3.Distance(player.position, transform.position);

    if (distance <= interactDistance)
    {
        if (promptText != null)
            promptText.enabled = true;

        if (Input.GetKeyDown(interactKey))
            doorIsOpen = !doorIsOpen;
    }
    else
    {
        if (promptText != null)
            promptText.enabled = false;
    }

    Quaternion targetRotation = doorIsOpen ? openRotation : initialRotation;
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
    }
}
