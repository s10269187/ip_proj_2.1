using UnityEngine;
using TMPro;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    Transform spawnPoint;
    bool canInteract = false;

    [Header("UI Settings")]
    public TextMeshProUGUI promptText;

    private BedroomDoor currentDoor;
    public KeyCode interactKey = KeyCode.E;
    public Camera playerCamera;
    public float interactDistance = 3f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (promptText != null)
            promptText.text = "";
        Debug.Log("PlayerBehaviour started. Camera assigned.");
    }

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

            // Check if the hit object has the tag "Door"
            if (hit.collider.CompareTag("Door"))
            {
                Debug.Log("Object with tag 'Door' detected: " + hit.collider.gameObject.name);
                if (promptText != null)
                {
                    promptText.enabled = true;
                    promptText.text = "Press E to Open/lose the door";
                }

                // Try to get BedroomDoor component (optional, for actual door logic)
                BedroomDoor door = hit.collider.GetComponent<BedroomDoor>();
                currentDoor = door;

                if (Input.GetKeyDown(interactKey) && door != null)
                {
                    Debug.Log("Interact key pressed. Toggling door: " + door.gameObject.name);
                    door.ToggleDoor();
                }
            }
            else
            {
                Debug.Log("Hit object is not tagged 'Door': " + hit.collider.gameObject.name);
                ClearPrompt();
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
            ClearPrompt();
        }
    }

    void ClearPrompt()
    {
        if (promptText != null)
        {
            promptText.enabled = false;
            promptText.text = "";
        }
        currentDoor = null;
    }
}