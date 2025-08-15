using UnityEngine;
using UnityEngine.UI;

// Handles sofa interaction and prompt display
public class SofaTrigger : MonoBehaviour
{
    // UI shown when player is near
    public GameObject sleepPromptUI; 
    // Link to sleep logic
    public SofaInteraction sofaInteraction; 
    // Tracks if player is close
    private bool playerInRange = false; 
    // Stores player object
    private GameObject currentPlayer;   

    // Hide prompt from other scripts
    public void HidePrompt()
    {
        if (sleepPromptUI != null)
            sleepPromptUI.SetActive(false);
    }

    void Start()
    {
        // Hide prompt at game start
        if (sleepPromptUI != null)
            sleepPromptUI.SetActive(false);
    }

    void Update()
    {
        // If player is near and presses E
        // Start sleep sequence
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (sofaInteraction != null && currentPlayer != null)
            {
                sofaInteraction.TriggerSleep(currentPlayer);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // If player enters trigger zone:
        // Save player reference
        // Mark player as nearby
        // Show sleep prompt
        if (other.CompareTag("Player"))
        {
            currentPlayer = other.gameObject;
            playerInRange = true;

            if (sleepPromptUI != null)
                sleepPromptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If player leaves trigger zone:
        // Clear player reference
        // Mark player as gone
        // Hide sleep prompt
        if (other.CompareTag("Player"))
        {
            currentPlayer = null;
            playerInRange = false;

            if (sleepPromptUI != null)
                sleepPromptUI.SetActive(false);
        }
    }
}
