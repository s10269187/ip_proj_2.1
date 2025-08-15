using UnityEngine;
using System.Collections;

// Controls sleep and wake-up interaction with a sofa
public class SofaInteraction : MonoBehaviour
{
    // Where the player lies down
    [Header("Positions")]
    public Transform sleepPosition;       
    // Where the player wakes up
    public Transform teleportTarget;      
    // UI fade effect
    [Header("Fade Settings")]
    public CanvasGroup fadeCanvas;        
    // Time to fade in/out
    public float fadeDuration = 2f;       
    // Rotation angle
    [Header("Rotation Settings")]
    public Vector3 lookUpEuler = new Vector3(60f, 0f, 0f); 
    // Time to rotate
    public float lookUpDuration = 2f;                      
    // Controls prompt display
    [Header("References")]
    public SofaTrigger sofaTrigger;       
    // Tracks sleep state
    private bool isSleeping = false;      
    // Music before sleep
    [SerializeField] public GameObject bgmAudio1; 
    // Music after sleep
    [SerializeField] public GameObject bgmAudio2; 

    // Called when player interacts with the sofa
    public void TriggerSleep(GameObject player)
    {
        if (!isSleeping)
        {
            StartCoroutine(SleepSequence(player));
            bgmAudio1.SetActive(false);
            bgmAudio2.SetActive(true);
        }
    }

    // Runs the sleep and wake-up steps
    IEnumerator SleepSequence(GameObject player)
    {
        isSleeping = true;

        // Stop player movement
        var controller = player.GetComponent<PlayerBehaviour>();
        if (controller) controller.enabled = false;

        // Hide the prompt
        if (sofaTrigger != null)
            sofaTrigger.HidePrompt();

        // Move player to sleep position
        if (sleepPosition != null)
        {
            var cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.transform.position = sleepPosition.position;
            player.transform.rotation = sleepPosition.rotation;

            if (cc != null) cc.enabled = true;
        }

        // Play sleep animation
        var anim = player.GetComponent<Animator>();
        if (anim) anim.SetTrigger("Sleep");

        // Rotate player upward
        yield return StartCoroutine(RotatePlayerUp(player));

        // Fade screen to black
        yield return StartCoroutine(FadeOut());

        // Move player to wake-up position
        if (teleportTarget != null)
        {
            var cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.transform.position = teleportTarget.position;
            player.transform.rotation = teleportTarget.rotation;

            if (cc != null) cc.enabled = true;
        }

        // Fade screen back in
        yield return StartCoroutine(FadeIn());

        // Allow player to move again
        if (controller) controller.enabled = true;

        isSleeping = false;
    }

    // Smoothly rotates the player
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

    // Fades screen to black
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

    // Fades screen from black
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
