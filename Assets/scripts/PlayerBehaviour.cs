/// <summary>
/// Handles player interactions with doors, NPCs, and placeable objects.
/// Also manages UI prompts and respawn behavior.
/// </summary>
/// <author> Aralyn Han Zi Ning </author>
/// <date> 06/08/2025 </date>
/// <StudentID> S10267170A </StudentID>

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBehaviour : MonoBehaviour
{
    /// <summary>
    /// Optional reference to the default spawn point (not used in Respawn).
    /// </summary>
    [SerializeField] Transform spawnPoint;

    /// <summary>
    /// The location to teleport the player to when respawning.
    /// </summary>
    [SerializeField] Transform spawnLocation;

    /// <summary>
    /// UI text shown when the player can interact with an object.
    /// </summary>
    [Header("UI Settings")]
    public TextMeshProUGUI promptText;

    /// <summary>
    /// Reference to the task UI text that will be updated upon NPC interaction.
    /// </summary>
    [Header("Task UI")]
    public TextMeshProUGUI taskText;

    /// <summary>
    /// Reference to the dialogue system used for NPC interactions.
    /// </summary>
    [Header("Dialogue Settings")]
    public Dialogue dialogueBox;

    /// <summary>
    /// Tracks the door currently in focus for interaction.
    /// </summary>
    private DoorBehaviour currentDoor;

    /// <summary>
    /// Key used to trigger interactions.
    /// </summary>
    public KeyCode interactKey = KeyCode.E;

    /// <summary>
    /// Camera used to cast rays for detecting interactable objects.
    /// </summary>
    public Camera playerCamera;

    /// <summary>
    /// Maximum distance from the player to detect interactable objects.
    /// </summary>
    public float interactDistance = 3f;

    /// <summary>
    /// Reference to the item pickup system.
    /// </summary>
    private PickUp pickUpScript;

    /// <summary>
    /// Tracks the current dialogue target (not used in this script).
    /// </summary>
    private Dialogue currentDialogue;

    /// <summary>
    /// Initializes references, clears UI, and handles spawn point loading from PlayerPrefs.
    /// </summary>
    void Start()
    {
        pickUpScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PickUp>();

        if (promptText != null)
            promptText.text = "";

        string spawnName = PlayerPrefs.GetString("SpawnPointName", "");
        if (spawnName != "")
        {
            GameObject spawnPoint = GameObject.Find(spawnName);
            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
            }

            PlayerPrefs.DeleteKey("SpawnPointName");
        }
    }

    /// <summary>
    /// Casts a ray from the camera to detect interactable objects and handles interaction logic.
    /// </summary>
    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Door"))
            {
                promptText.enabled = true;
                promptText.text = "Press E to Interact";
                currentDoor = hitObject.GetComponent<DoorBehaviour>();

                if (Input.GetKeyDown(interactKey) && currentDoor != null)
                {
                    currentDoor.ToggleDoor();
                }
            }
            else if (hitObject.CompareTag("Placeable"))
            {
                promptText.enabled = true;
                promptText.text = "Press Q to Place item";

                PlaceAndTransferMaterial placeScript = hitObject.GetComponent<PlaceAndTransferMaterial>();

                if (Input.GetKeyDown(KeyCode.Q) && placeScript != null && pickUpScript.heldObj != null)
                {
                    placeScript.PlaceObject(pickUpScript.heldObj);
                }
            }
            else if (hitObject.CompareTag("NPC"))
            {
                promptText.enabled = true;
                promptText.text = "Press E to Talk";

                if (Input.GetKeyDown(interactKey) && dialogueBox != null)
                {
                    dialogueBox.gameObject.SetActive(true);
                    dialogueBox.enabled = true;
                    dialogueBox.StartDialogue();

                    // Strike through the task text
                    if (taskText != null)
                    {
                        taskText.text = "<s>" + StripTags(taskText.text) + "</s>";
                        taskText.color = Color.gray;
                    }
                }
            }
            else
            {
                ClearPrompt();
            }
        }
        else
        {
            ClearPrompt();
        }
    }

    /// <summary>
    /// Handles collision with ghost objects and triggers respawn.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            Respawn();
        }
    }

    /// <summary>
    /// Clears the interaction prompt and resets interaction references.
    /// </summary>
    void ClearPrompt()
    {
        if (promptText != null)
        {
            promptText.enabled = false;
            promptText.text = "";
        }

        currentDoor = null;
        currentDialogue = null;
    }

    /// <summary>
    /// Teleports the player to the spawn location and resets physics state.
    /// </summary>
    public void Respawn()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (spawnLocation != null)
        {
            transform.position = spawnLocation.position;

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.Sleep();
                Physics.SyncTransforms();
            }
        }
    }

    /// <summary>
    /// Removes strikethrough tags from a string.
    /// </summary>
    private string StripTags(string input)
    {
        return input.Replace("<s>", "").Replace("</s>", "");
    }
}
