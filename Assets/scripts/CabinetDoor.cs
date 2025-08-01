using UnityEngine;
using TMPro;

public class CabinetDoor : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;

    public Vector3 leftOpenRotation = new Vector3(0, -90, 0);
    public Vector3 rightOpenRotation = new Vector3(0, 90, 0);
    public float rotationSpeed = 2f;

    private Quaternion leftClosedRot;
    private Quaternion rightClosedRot;
    private Quaternion leftOpenRot;
    private Quaternion rightOpenRot;

    private bool isOpen = false;
    private bool isPlayerNearby = false;

    void Start()
    {
        leftClosedRot = leftDoor.localRotation;
        rightClosedRot = rightDoor.localRotation;

        leftOpenRot = Quaternion.Euler(leftOpenRotation);
        rightOpenRot = Quaternion.Euler(rightOpenRotation);

        Debug.Log("CabinetDoorController initialized.");
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
            Debug.Log("E key pressed. Door state: " + (isOpen ? "Open" : "Closed"));
        }

        Quaternion leftTarget = isOpen ? leftOpenRot : leftClosedRot;
        Quaternion rightTarget = isOpen ? rightOpenRot : rightClosedRot;

        leftDoor.localRotation = Quaternion.Lerp(leftDoor.localRotation, leftTarget, Time.deltaTime * rotationSpeed);
        rightDoor.localRotation = Quaternion.Lerp(rightDoor.localRotation, rightTarget, Time.deltaTime * rotationSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Player entered trigger zone.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Player exited trigger zone.");
        }
    }
}
