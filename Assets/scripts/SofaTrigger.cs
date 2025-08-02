using UnityEngine;
using UnityEngine.UI;

public class SofaTrigger : MonoBehaviour
{
   public GameObject sleepPromptUI;
    public SofaInteraction sofaInteraction; // Assign this manually in Inspector

    private bool playerInRange = false;
    private GameObject currentPlayer;

    public void HidePrompt()
    {
    if (sleepPromptUI != null)
        sleepPromptUI.SetActive(false);
    }


    void Start()
    {
        if (sleepPromptUI != null)
            sleepPromptUI.SetActive(false);
    }

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
