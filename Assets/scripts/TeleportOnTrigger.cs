using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Teleport player with fade effect
public class TeleportOnTrigger : MonoBehaviour
{
    // Target position
    public Transform teleport;      
    // Fade overlay
    public Image fadeImage;         
    // Fade time
    public float fadeDuration = 1f; 

    void OnTriggerEnter(Collider other)
    {
        // Start teleport if player enters
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeTeleport(other));
        }
    }
    // Teleport player with fade effect
    // Fade to black
    // Move player
    // Fade in
    IEnumerator FadeTeleport(Collider other)
    {
        yield return StartCoroutine(FadeOut());   
        Respawn(other);                           
        yield return StartCoroutine(FadeIn());    
    }

    //Screen fade
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
    // Screen fade
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
    // Move player to teleport position and stop motion
    public void Respawn(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (teleport != null)
        {
            // Move player to teleport position
            other.transform.position = teleport.position; 
            // Stop motion
            if (rb != null)
            {
                rb.Sleep(); 
            }
            // Refresh physics
            Physics.SyncTransforms(); 
        }
    }
}
