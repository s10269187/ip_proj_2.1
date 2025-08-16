/// <summary>
/// Handles placing an item, transferring its material to a target object,
/// triggering a cutscene, and teleporting the player with a fade effect.
/// </summary>
/// <author> Aralyn Han Zi Ning </author>
/// <date> 07/08/2025 </date>
/// <StudentID> S10267170A </StudentID>
using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.UI;
using System.Collections;


public class PlaceAndTransferMaterial : MonoBehaviour
{
    /// <summary>
    /// The new material to apply to the target object.
    /// </summary>
    [Header("Material Settings")]
    [SerializeField] private Material newMat;

    /// <summary>
    /// The object that will receive the new material.
    /// </summary>
    [Header("Target Settings")]
    [SerializeField] private GameObject targetObject;

    /// <summary>
    /// The only item allowed to trigger the material transfer.
    /// </summary>
    [Header("Allowed Item")]
    [SerializeField] private GameObject allowedItem;

    /// <summary>
    /// The UI element representing the task to be marked as completed.
    /// </summary>
    [Header("Task UI")]
    [SerializeField] private TextMeshProUGUI taskToStrike;

    /// <summary>
    /// The cutscene to play after placing the item.
    /// </summary>
    [Header("Cutscene")]
    [SerializeField] private PlayableDirector cutsceneDirector;

    [Header("Teleport Settings")]
    /// <summary>
    /// The destination to teleport the player to.
    /// </summary>
    [SerializeField] private Transform teleportDestination;

    /// <summary>
    /// The player GameObject to be teleported.
    /// </summary>
    [SerializeField] private GameObject player;

    [Header("Fade Settings")]
    /// <summary>
    /// The canvas group used for screen fade effects.
    /// </summary>
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    /// <summary>
    /// Duration of the fade effect in seconds.
    /// </summary>
    [SerializeField] private float fadeDuration = 1f;

    /// <summary>
    /// Flag to prevent multiple teleportations.
    /// </summary>
    private bool hasTeleported = false;

    /// <summary>
    /// Ensures this GameObject persists across scene loads.
    /// </summary>
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Initializes cutscene and fade canvas settings.
    /// </summary>
    private void Awake()
    {
        if (cutsceneDirector != null)
        {
            cutsceneDirector.stopped += OnCutsceneEnded;
            cutsceneDirector.Stop();
        }

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
            fadeCanvasGroup.interactable = false;
        }
    }

    /// <summary>
    /// Places the held object, transfers its material to the target,
    /// updates the UI, plays the cutscene, and starts teleport sequence.
    /// </summary>
    /// <param name="heldObject">The object being placed by the player.</param>
    public void PlaceObject(GameObject heldObject)
    {
        if (heldObject == null || targetObject == null)
            return;

        if (heldObject != allowedItem)
            return;

        Renderer heldRenderer = heldObject.GetComponent<Renderer>();
        Renderer targetRenderer = targetObject.GetComponent<Renderer>();

        if (heldRenderer != null && targetRenderer != null)
        {
            targetRenderer.material = newMat;
        }

        PickUp pickUp = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<PickUp>();
        pickUp?.ForceDropHeldObject();
        Destroy(heldObject);

        if (taskToStrike != null)
        {
            taskToStrike.text = $"<s>{taskToStrike.text}</s>";
            taskToStrike.color = Color.gray;
        }

        if (cutsceneDirector != null)
        {
            cutsceneDirector.Play();
            StartCoroutine(WaitAndForceTeleport());
        }
    }

    /// <summary>
    /// Called when the cutscene ends. Triggers teleport if cutscene completed normally.
    /// </summary>
    /// <param name="director">The PlayableDirector that finished playing.</param>
    private void OnCutsceneEnded(PlayableDirector director)
    {
        if (hasTeleported || teleportDestination == null || player == null)
            return;

        if (Mathf.Approximately((float)director.time, (float)director.duration))
        {
            StartCoroutine(FadeAndTeleport());
        }
    }

    /// <summary>
    /// Waits for the cutscene duration and triggers teleport if not already done.
    /// </summary>
    private IEnumerator WaitAndForceTeleport()
    {
        yield return new WaitForSeconds((float)cutsceneDirector.duration + 0.5f);

        if (!hasTeleported)
        {
            StartCoroutine(FadeAndTeleport());
        }
    }

    /// <summary>
    /// Fades the screen out, teleports the player, then fades back in.
    /// </summary>
    private IEnumerator FadeAndTeleport()
    {
        hasTeleported = true;

        yield return StartCoroutine(Fade(0f, 1f));
        TeleportPlayer();
        yield return StartCoroutine(Fade(1f, 0f));
    }

    /// <summary>
    /// Teleports the player to the destination and resets their physics velocity.
    /// </summary>
    private void TeleportPlayer()
    {
        if (player == null || teleportDestination == null) return;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            player.transform.position = teleportDestination.position;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Performs a fade effect by interpolating the canvas group's alpha.
    /// </summary>
    /// <param name="from">Starting alpha value.</param>
    /// <param name="to">Target alpha value.</param>
    /// <returns>Coroutine for fade transition.</returns>
    private IEnumerator Fade(float from, float to)
    {
        if (fadeCanvasGroup == null) yield break;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = to;
    }

    /// <summary>
    /// Cleans up event subscriptions when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (cutsceneDirector != null)
        {
            cutsceneDirector.stopped -= OnCutsceneEnded;
        }
    }
}
