using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Teleports the player to a target location when entering a trigger zone,
/// using a fade-to-black and fade-in visual effect.
/// </summary>
public class TeleportOnTrigger : MonoBehaviour
{
    /// <summary>
    /// The target position to teleport the player to.
    /// </summary>
    public Transform teleport;

    /// <summary>
    /// UI image used for screen fade effects.
    /// </summary>
    public Image fadeImage;

    /// <summary>
    /// Duration of the fade in and fade out effects.
    /// </summary>
    public float fadeDuration = 1f;

    /// <summary>
    /// Called when another collider enters the trigger zone.
    /// Starts the teleport sequence if the collider is the player.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeTeleport(other));
        }
    }

    /// <summary>
    /// Coroutine that fades out the screen, teleports the player, and fades back in.
    /// </summary>
    /// <param name="other">The player's collider.</param>
    /// <returns>Coroutine enumerator.</returns>
    IEnumerator FadeTeleport(Collider other)
    {
        yield return StartCoroutine(FadeOut());
        Respawn(other);
        yield return StartCoroutine(FadeIn());
    }

    /// <summary>
    /// Coroutine that fades the screen to black by increasing the alpha of the fade image.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }

    /// <summary>
    /// Coroutine that fades the screen from black by decreasing the alpha of the fade image.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }

    /// <summary>
    /// Moves the player to the teleport position and resets their physics state.
    /// </summary>
    /// <param name="other">The player's collider.</param>
    public void Respawn(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (teleport != null)
        {
            other.transform.position = teleport.position;

            if (rb != null)
            {
                rb.Sleep();
            }

            Physics.SyncTransforms();
        }
    }
}


