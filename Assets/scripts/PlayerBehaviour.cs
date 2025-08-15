using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBehaviour : MonoBehaviour
{
    // Reference to the default spawn point (not used in Respawn)
    [SerializeField] Transform spawnPoint;

    // Location to teleport the player when respawning
    [SerializeField] Transform spawnLocation;
    // UI text shown when player can interact
    [Header("UI Settings")]
    public TextMeshProUGUI promptText; 
    // Reference to the dialogue system
    [Header("Dialogue Settings")]
    public Dialogue dialogueBox; 
    // Tracks the door currently in focus
    private DoorBehaviour currentDoor; 
    // Key used for interaction
    public KeyCode interactKey = KeyCode.E;
    // Camera used for raycasting
    public Camera playerCamera; 
    // Max distance for interaction
    public float interactDistance = 3f; 
    // Reference to the item pickup system
    private PickUp pickUpScript; 
    // Tracks current dialogue (not used here)
    private Dialogue currentDialogue; 

    void Start()
    {
        // Get reference to the PickUp script from the MainCamera
        pickUpScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PickUp>();

        // Clear any prompt text at the start
        if (promptText != null)
            promptText.text = "";

        // Check if a spawn point name was saved in PlayerPrefs
        string spawnName = PlayerPrefs.GetString("SpawnPointName", "");
        if (spawnName != "")
        {
            // Find the spawn point by name and move the player there
            GameObject spawnPoint = GameObject.Find(spawnName);
            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
            }

            // Clear the saved spawn point name
            PlayerPrefs.DeleteKey("SpawnPointName");
        }
    }

    void Update()
    {
        // Cast a ray from the camera forward to detect interactable objects
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            // If the ray hits a door
            if (hitObject.CompareTag("Door"))
            {
                promptText.enabled = true;
                promptText.text = "Press E to Interact";
                currentDoor = hitObject.GetComponent<DoorBehaviour>();

                // If player presses interact key, toggle the door
                if (Input.GetKeyDown(interactKey) && currentDoor != null)
                {
                    currentDoor.ToggleDoor();
                }
            }
            // If the ray hits a placeable surface
            else if (hitObject.CompareTag("Placeable"))
            {
                promptText.enabled = true;
                promptText.text = "Press Q to Place item";

                PlaceAndTransferMaterial placeScript = hitObject.GetComponent<PlaceAndTransferMaterial>();

                // If player presses Q and is holding an object, place it
                if (Input.GetKeyDown(KeyCode.Q) && placeScript != null && pickUpScript.heldObj != null)
                {
                    placeScript.PlaceObject(pickUpScript.heldObj);
                }
            }
            // If the ray hits an NPC
            else if (hitObject.CompareTag("NPC"))
            {
                promptText.enabled = true;
                promptText.text = "Press E to Talk";

                // If player presses interact key, start dialogue
                // Show dialogue UI
                // Enable dialogue script
                // Begin dialogue sequence
                if (Input.GetKeyDown(interactKey) && dialogueBox != null)
                {
                    dialogueBox.gameObject.SetActive(true);
                    dialogueBox.enabled = true;             
                    dialogueBox.StartDialogue();            
                }
            }
            // If the ray hits something else
            // Hide interaction prompt
            else
            {
                ClearPrompt();
            }
        }
        // Hide prompt if nothing is hit
        else
        {
            ClearPrompt();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If player collides with a ghost, trigger respawn
        if (other.CompareTag("Ghost"))
        {
            Respawn();
        }
    }

    void ClearPrompt()
    {
        // Hide and clear the interaction prompt
        if (promptText != null)
        {
            promptText.enabled = false;
            promptText.text = "";
        }

        // Reset interaction references
        currentDoor = null;
        currentDialogue = null;
    }

    public void Respawn()
    {
        // Move player to the designated spawn location
        Rigidbody rb = GetComponent<Rigidbody>();

        if (spawnLocation != null)
        {
            transform.position = spawnLocation.position;

            // Reset physics to avoid unwanted motion
            // Ensure physics state is updated
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.Sleep();
                Physics.SyncTransforms();
            }
        }
    }
}
