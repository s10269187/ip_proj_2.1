using UnityEngine;

public class BathroomDoor : MonoBehaviour
{
   public Transform door;                // Assign your door Transform in inspector
    public float openAngle = 105f;        // Rotation angle for open
    public float speed = 2f;              // Speed of rotation

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openedRotation;

    void Start()
    {
        if (door == null)
            door = transform;

        closedRotation = door.rotation;
        openedRotation = Quaternion.Euler(door.eulerAngles + new Vector3(0, openAngle, 0));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen; // Toggle state
        }

        Quaternion targetRotation = isOpen ? openedRotation : closedRotation;
        door.rotation = Quaternion.Lerp(door.rotation, targetRotation, Time.deltaTime * speed);
    }
}
