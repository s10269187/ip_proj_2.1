using UnityEngine;
using System.Collections;

public class SofaInteraction : MonoBehaviour
{
    [Header("Positions")]
    public Transform sleepPosition;       // Where player lies down
    public Transform teleportTarget;      // Where player wakes up

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 2f;

    [Header("Rotation Settings")]
    public Vector3 lookUpEuler = new Vector3(60f, 0f, 0f);
    public float lookUpDuration = 2f;

    [Header("References")]
    public SofaTrigger sofaTrigger;

    private bool isSleeping = false;

    public void TriggerSleep(GameObject player)
    {
        if (!isSleeping)
        {
            StartCoroutine(SleepSequence(player));
        }
    }

    IEnumerator SleepSequence(GameObject player)
    {
        isSleeping = true;

        // Disable player movement
        var controller = player.GetComponent<PlayerBehaviour>();
        if (controller) controller.enabled = false;

        // Hide prompt
        if (sofaTrigger != null)
            sofaTrigger.HidePrompt();

        // ✅ STEP 1: Teleport player to sleep position immediately
        if (sleepPosition != null)
        {
            var cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.transform.position = sleepPosition.position;
            player.transform.rotation = sleepPosition.rotation;

            if (cc != null) cc.enabled = true;

            Debug.Log("Player teleported to sleep position");
        }
        else
        {
            Debug.LogWarning("Sleep position not assigned!");
        }

        // ✅ STEP 2: Play sleep animation
        var anim = player.GetComponent<Animator>();
        if (anim) anim.SetTrigger("Sleep");

        // ✅ STEP 3: Rotate player upward
        yield return StartCoroutine(RotatePlayerUp(player));

        // ✅ STEP 4: Fade out
        yield return StartCoroutine(FadeOut());

        // ✅ STEP 5: Teleport to wake-up location
        if (teleportTarget != null)
        {
            var cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.transform.position = teleportTarget.position;
            player.transform.rotation = teleportTarget.rotation;

            if (cc != null) cc.enabled = true;

            Debug.Log("Player teleported to wake-up position");
        }

        // ✅ STEP 6: Fade in
        yield return StartCoroutine(FadeIn());

        // Re-enable player movement
        if (controller) controller.enabled = true;

        isSleeping = false;
    }

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
