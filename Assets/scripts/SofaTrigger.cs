/// <summary>
/// Detects when the player is near the sofa and displays a sleep prompt.
/// Initiates the sleep interaction when the player presses the interaction key.
/// </summary>
/// /// <author> Aralyn Han Zi Ning </author>       
/// <date> 02/08/2025 </date>
/// <StudentID> S10267170A </StudentID>
using UnityEngine;
using UnityEngine.UI;


public class SofaTrigger : MonoBehaviour
{
    /// <summary>
    /// UI element shown when the player is near the sofa.
    /// </summary>
    public GameObject sleepPromptUI;

    /// <summary>
    /// Reference to the sofa interaction logic.
    /// </summary>
    public SofaInteraction sofaInteraction;

    /// <summary>
    /// Tracks whether the player is currently within the trigger zone.
    /// </summary>
    private bool playerInRange = false;

    /// <summary>
    /// Stores a reference to the player GameObject.
    /// </summary>
    private GameObject currentPlayer;

    /// <summary>
    /// Hides the sleep prompt UI. Can be called externally.
    /// </summary>
    public void HidePrompt()
    {
        if (sleepPromptUI != null)
            sleepPromptUI.SetActive(false);
    }

    /// <summary>
    /// Initializes the trigger by hiding the sleep prompt at game start.
    /// </summary>
    void Start()
    {
        if (sleepPromptUI != null)
            sleepPromptUI.SetActive(false);
    }

    /// <summary>
    /// Checks for player input while in range and triggers sleep interaction.
    /// </summary>
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (sofaInteraction != null && currentPlayer != null)
            {
                sofaInteraction.TriggerSleep(currentPlayer);
            }
        }
    }

    /// <summary>
    /// Called when a collider enters the trigger zone.
    /// Displays the sleep prompt if the player enters.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayer = other.gameObject;
            playerInRange = true;

            if (sleepPromptUI != null)
                sleepPromptUI.SetActive(true);
        }
    }

    /// <summary>
    /// Called when a collider exits the trigger zone.
    /// Hides the sleep prompt if the player leaves.
    /// </summary>
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayer = null;
            playerInRange = false;

            if (sleepPromptUI != null)
                sleepPromptUI.SetActive(false);
        }
    }
}
