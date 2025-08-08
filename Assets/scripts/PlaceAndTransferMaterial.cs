using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.UI;
using System.Collections;

public class PlaceAndTransferMaterial : MonoBehaviour
{
    [Header("Material Settings")]
    [SerializeField] private Material newMat;

    [Header("Target Settings")]
    [SerializeField] private GameObject targetObject;

    [Header("Allowed Item")]
    [SerializeField] private GameObject allowedItem;

    [Header("Task UI")]
    [SerializeField] private TextMeshProUGUI taskToStrike;

    [Header("Cutscene")]
    [SerializeField] private PlayableDirector cutsceneDirector;

    [Header("Teleport Settings")]
    [SerializeField] private Transform teleportDestination;
    [SerializeField] private GameObject player;

    [Header("Fade Settings")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private bool hasTeleported = false;

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
        }
    }

    public void PlaceObject(GameObject heldObject)
    {
        if (heldObject == null || targetObject == null)
        {
            Debug.LogWarning("Held or target object is null.");
            return;
        }

        if (heldObject != allowedItem)
        {
            Debug.Log("This item is not allowed to transfer material.");
            return;
        }

        Renderer heldRenderer = heldObject.GetComponent<Renderer>();
        Renderer targetRenderer = targetObject.GetComponent<Renderer>();

        if (heldRenderer != null && targetRenderer != null)
        {
            targetRenderer.material = newMat;
            Debug.Log($"Material transferred from {heldObject.name} to {targetObject.name}");
        }
        else
        {
            Debug.LogWarning("Missing Renderer on held or target object.");
        }

        PickUp pickUp = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<PickUp>();
        pickUp?.ForceDropHeldObject();

        Destroy(heldObject);
        Debug.Log("Allowed item destroyed.");

        if (taskToStrike != null)
        {
            taskToStrike.text = $"<s>{taskToStrike.text}</s>";
            taskToStrike.color = Color.gray;
        }

        if (cutsceneDirector != null)
        {
            cutsceneDirector.Play();
            StartCoroutine(WaitAndForceTeleport());
            Debug.Log("🎬 Cutscene triggered after placing teapot.");
        }
    }

    private void OnCutsceneEnded(PlayableDirector director)
    {
        if (hasTeleported || teleportDestination == null || player == null)
            return;

        Debug.Log($"Cutscene ended. Time: {director.time}, Duration: {director.duration}");

        if (Mathf.Approximately((float)director.time, (float)director.duration))
        {
            StartCoroutine(FadeAndTeleport());
        }
        else
        {
            Debug.LogWarning("Cutscene ended prematurely. Teleport skipped.");
        }
    }

    private IEnumerator WaitAndForceTeleport()
    {
        yield return new WaitForSeconds((float)cutsceneDirector.duration + 0.5f);

        if (!hasTeleported)
        {
            Debug.Log("⏱ Fallback teleport triggered.");
            StartCoroutine(FadeAndTeleport());
        }
    }

    private IEnumerator FadeAndTeleport()
    {
        hasTeleported = true;

        yield return StartCoroutine(Fade(0f, 1f));

        player.transform.SetPositionAndRotation(teleportDestination.position, teleportDestination.rotation);
        Debug.Log("🚀 Player teleported after cutscene.");

        yield return StartCoroutine(Fade(1f, 0f));
    }

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

    private void OnDestroy()
    {
        if (cutsceneDirector != null)
        {
            cutsceneDirector.stopped -= OnCutsceneEnded;
        }
    }
}
