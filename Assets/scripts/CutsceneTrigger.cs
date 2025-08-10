using UnityEngine;
using UnityEngine.Playables;
using StarterAssets;
using UnityEngine.InputSystem;
using System.Collections;


public class CutsceneTrigger : MonoBehaviour
{
    [Header("References")]
    public PlayableDirector cutsceneDirector;              // Assign in Inspector
    public GhostTyping ghostTypingScript;                  // Assign in Inspector
    public FirstPersonController playerController;         // Assign in Inspector
    public Transform teleportTarget;                       // Assign in Inspector

    private PlayerInput playerInput;
    private CharacterController characterController;
    private bool hasTriggered = false;

    void Start()
    {
        if (cutsceneDirector != null)
        {
            cutsceneDirector.Stop();
            cutsceneDirector.time = 0;
            cutsceneDirector.enabled = false;
            cutsceneDirector.stopped += OnCutsceneEnded;
        }

        if (playerController != null)
        {
            playerInput = playerController.GetComponent<PlayerInput>();
            characterController = playerController.GetComponent<CharacterController>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered || cutsceneDirector == null) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(TriggerCutsceneSequence());
        }
    }

    IEnumerator TriggerCutsceneSequence()
    {
        // Disable player movement and input
        if (playerController != null)
        {
            playerController.enabled = false;
            if (playerInput != null) playerInput.enabled = false;
            if (characterController != null) characterController.enabled = false;
            Debug.Log("🚫 Player controls disabled.");
        }

        yield return new WaitForSeconds(0.1f); // Small delay to ensure disable takes effect

        // Start cutscene
        cutsceneDirector.enabled = true;
        cutsceneDirector.Play();
        Debug.Log("🎬 Cutscene started.");

        // Start ghost typing
        if (ghostTypingScript != null)
        {
            ghostTypingScript.StartTyping();
            Debug.Log("👻 GhostTyping started.");
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

        // Re-enable player controls
        if (playerController != null)
        {
            playerController.enabled = true;
            if (playerInput != null) playerInput.enabled = true;
            if (characterController != null) characterController.enabled = true;
            Debug.Log("✅ Player controls re-enabled after cutscene.");
        }
    }
}
