using UnityEngine;
using UnityEngine.Playables;
using StarterAssets;

public class CutsceneTrigger : MonoBehaviour
{
    [Header("References")]
    public PlayableDirector cutsceneDirector;              // Assign in Inspector
    public GhostTyping ghostTypingScript;                  // Assign in Inspector
    public FirstPersonController playerController;         // Assign in Inspector
    public Transform teleportTarget;                       // Assign in Inspector (where to teleport)

    private bool hasTriggered = false;
    private bool waitingForMovement = false;

    void Start()
    {
        if (cutsceneDirector != null)
        {
            cutsceneDirector.Stop();
            cutsceneDirector.time = 0;
            cutsceneDirector.enabled = false;

            // Subscribe to cutscene end event
            cutsceneDirector.stopped += OnCutsceneEnded;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered || cutsceneDirector == null) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (playerController != null)
            {
                playerController.enabled = false;
                Debug.Log("🚫 FirstPersonController disabled.");
            }

            cutsceneDirector.enabled = true;
            cutsceneDirector.Play();
            Debug.Log("🎬 Cutscene started after trigger.");

            if (ghostTypingScript != null)
            {
                ghostTypingScript.StartTyping();
                Debug.Log("👻 GhostTyping started.");
            }

            waitingForMovement = true;
        }
    }

    void Update()
    {
        if (waitingForMovement && Input.anyKeyDown)
        {
            if (playerController != null)
            {
                playerController.enabled = true;
                Debug.Log("✅ FirstPersonController re-enabled.");
            }

            waitingForMovement = false;
        }
    }

    void OnCutsceneEnded(PlayableDirector director)
    {
        // Teleport player after cutscene ends
        if (teleportTarget != null && playerController != null)
        {
            playerController.transform.position = teleportTarget.position;
            playerController.transform.rotation = teleportTarget.rotation;
            Debug.Log("📍 Player teleported after cutscene.");
        }
    }
}
