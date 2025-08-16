/// <summary>
/// Controls the sleep and wake-up interaction with a sofa, including player movement,
/// rotation, fade effects, and background music transitions.
/// </summary>
/// <author> Aralyn Han Zi Ning </author>       
/// <date> 02/08/2025 </date>
/// <StudentID> S10267170A </StudentID>
using UnityEngine;
using System.Collections;

public class SofaInteraction : MonoBehaviour
{
    /// <summary>
    /// The position where the player lies down to sleep.
    /// </summary>
    [Header("Positions")]
    public Transform sleepPosition;

    /// <summary>
    /// The position where the player wakes up after sleeping.
    /// </summary>
    public Transform teleportTarget;

    /// <summary>
    /// Canvas group used for screen fade effects.
    /// </summary>
    [Header("Fade Settings")]
    public CanvasGroup fadeCanvas;

    /// <summary>
    /// Duration of fade in and fade out effects.
    /// </summary>
    public float fadeDuration = 2f;

    /// <summary>
    /// Euler angles used to rotate the player upward during sleep.
    /// </summary>
    [Header("Rotation Settings")]
    public Vector3 lookUpEuler = new Vector3(60f, 0f, 0f);

    /// <summary>
    /// Duration of the upward rotation animation.
    /// </summary>
    public float lookUpDuration = 2f;

    /// <summary>
    /// Reference to the sofa trigger used to control prompt visibility.
    /// </summary>
    [Header("References")]
    public SofaTrigger sofaTrigger;

    /// <summary>
    /// Tracks whether the player is currently sleeping.
    /// </summary>
    private bool isSleeping = false;

    /// <summary>
    /// Background music to play before sleep.
    /// </summary>
    [SerializeField] public GameObject bgmAudio1;

    /// <summary>
    /// Background music to play after waking up.
    /// </summary>
    [SerializeField] public GameObject bgmAudio2;

    /// <summary>
    /// Initiates the sleep sequence when the player interacts with the sofa.
    /// </summary>
    /// <param name="player">The player GameObject.</param>
    public void TriggerSleep(GameObject player)
    {
        if (!isSleeping)
        {
            StartCoroutine(SleepSequence(player));
            bgmAudio1.SetActive(false);
            bgmAudio2.SetActive(true);
        }
    }

    /// <summary>
    /// Coroutine that handles the full sleep and wake-up sequence.
    /// </summary>
    /// <param name="player">The player GameObject.</param>
    /// <returns>Coroutine enumerator.</returns>
    IEnumerator SleepSequence(GameObject player)
    {
        isSleeping = true;

        var controller = player.GetComponent<PlayerBehaviour>();
        if (controller) controller.enabled = false;

        if (sofaTrigger != null)
            sofaTrigger.HidePrompt();

        if (sleepPosition != null)
        {
            var cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.transform.position = sleepPosition.position;
            player.transform.rotation = sleepPosition.rotation;

            if (cc != null) cc.enabled = true;
        }

        var anim = player.GetComponent<Animator>();
        if (anim) anim.SetTrigger("Sleep");

        yield return StartCoroutine(RotatePlayerUp(player));
        yield return StartCoroutine(FadeOut());

        if (teleportTarget != null)
        {
            var cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.transform.position = teleportTarget.position;
            player.transform.rotation = teleportTarget.rotation;

            if (cc != null) cc.enabled = true;
        }

        yield return StartCoroutine(FadeIn());

        if (controller) controller.enabled = true;

        isSleeping = false;
    }

    /// <summary>
    /// Smoothly rotates the player upward using the specified Euler angles.
    /// </summary>
    /// <param name="player">The player GameObject.</param>
    /// <returns>Coroutine enumerator.</returns>
    IEnumerator RotatePlayerUp(GameObject player)
    {
        Quaternion startRot = player.transform.rotation;
        Quaternion endRot = Quaternion.Euler(lookUpEuler);

        float t = 0;
        while (t < lookUpDuration)
        {
            t += Time.deltaTime;
            player.transform.rotation = Quaternion.Slerp(startRot, endRot, t / lookUpDuration);
            yield return null;
        }
    }

    /// <summary>
    /// Fades the screen to black by increasing the canvas alpha.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    IEnumerator FadeOut()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            if (fadeCanvas != null)
                fadeCanvas.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
    }

    /// <summary>
    /// Fades the screen from black by decreasing the canvas alpha.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    IEnumerator FadeIn()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            if (fadeCanvas != null)
                fadeCanvas.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
    }
}
