using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TeleportOnTrigger : MonoBehaviour
{
    public Transform teleport;
    public Image fadeImage; // Assign in Inspector
    public float fadeDuration = 1f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeTeleport(other));
        }
    }

    IEnumerator FadeTeleport(Collider other)
    {
        yield return StartCoroutine(FadeOut());

        Respawn(other);

        yield return StartCoroutine(FadeIn());
    }

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

    public void Respawn(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (teleport != null)
        {
            other.transform.position = teleport.position;
            Debug.Log("Teleporting to: " + teleport.position);

            if (rb != null)
            {
                //rb.linearVelocity = Vector3.zero;
                //rb.angularVelocity = Vector3.zero;
                rb.Sleep();
            }

            Physics.SyncTransforms();
        }
        else
        {
            Debug.LogWarning("Spawn location not assigned!");
        }
    }
}
