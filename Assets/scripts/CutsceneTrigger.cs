/// <summary>
/// Triggers a timeline cutscene when the player enters a collider,
/// disables player controls during the cutscene, and teleports the player afterward.
/// </summary>
/// <author> Aralyn Han Zi Ning </author>
/// <date> 11/08/2025 </date>
/// <StudentID> S10267170A </StudentID>
using UnityEngine;
using UnityEngine.Playables;
using StarterAssets;
using UnityEngine.InputSystem;
using System.Collections;


public class CutsceneTrigger : MonoBehaviour
{
    /// <summary>
    /// The timeline cutscene to play.
    /// </summary>
    [Header("References")]
    public PlayableDirector cutsceneDirector;

    /// <summary>
    /// Script that handles ghost typing visual effect.
    /// </summary>
    public GhostTyping ghostTypingScript;

    /// <summary>
    /// Reference to the player's movement controller.
    /// </summary>
    public FirstPersonController playerController;

    /// <summary>
    /// The target position and rotation to teleport the player to after the cutscene.
    /// </summary>
    public Transform teleportTarget;

    /// <summary>
    /// Reference to the player's input system.
    /// </summary>
    private PlayerInput playerInput;

    /// <summary>
    /// Reference to the player's character controller for physics movement.
    /// </summary>
    private CharacterController characterController;

    /// <summary>
    /// Flag to ensure the cutscene is only triggered once.
    /// </summary>
    private bool hasTriggered = false;

    /// <summary>
    /// Initializes references and prepares the cutscene.
    /// </summary>
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

    /// <summary>
    /// Detects when the player enters the trigger zone and starts the cutscene sequence.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered || cutsceneDirector == null) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(TriggerCutsceneSequence());
        }
    }

    /// <summary>
    /// Coroutine that disables player controls, starts the cutscene, and activates ghost typing.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    IEnumerator TriggerCutsceneSequence()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
            if (playerInput != null) playerInput.enabled = false;
            if (characterController != null) characterController.enabled = false;
        }

        yield return new WaitForSeconds(0.1f);

        cutsceneDirector.enabled = true;
        cutsceneDirector.Play();

        if (ghostTypingScript != null)
        {
            ghostTypingScript.StartTyping();
        }
    }

    /// <summary>
    /// Called when the cutscene ends. Teleports the player and re-enables controls.
    /// </summary>
    /// <param name="director">The PlayableDirector that finished playing.</param>
    void OnCutsceneEnded(PlayableDirector director)
    {
        if (teleportTarget != null && playerController != null)
        {
            playerController.transform.position = teleportTarget.position;
            playerController.transform.rotation = teleportTarget.rotation;
        }

        if (playerController != null)
        {
            playerController.enabled = true;
            if (playerInput != null) playerInput.enabled = true;
            if (characterController != null) characterController.enabled = true;
        }
    }
}
