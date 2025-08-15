using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneManagement : MonoBehaviour
{
    // UI image used for fade effect
    [Header("Fade Settings")]
    public Image fadeImage; 
    // Duration of fade in/out
    public float fadeDuration = 1f; 
    // Scene to teleport to
    [Header("Teleport Settings")]
    public string targetScene = "RETURN_ms"; 
    // Name of spawn point in target scene
    public string spawnPointName = "spawn_return"; 

    void Start()
    {
        // If the current scene is the target scene, move player to the designated spawn point
        if (SceneManager.GetActiveScene().name == targetScene)
        {
            // Fade in from black
            MovePlayerToSpawn();
            StartCoroutine(FadeIn()); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // When player enters trigger, start teleport sequence
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeTeleport());
        }
    }

    IEnumerator FadeTeleport()
    {
        // Fade out to black
        yield return StartCoroutine(FadeOut());

        // Save spawn point name for use in the next scene
        PlayerPrefs.SetString("SpawnPoint", spawnPointName);

        // Load the target scene
        SceneManager.LoadScene(targetScene);
    }

    void MovePlayerToSpawn()
    {
        // Retrieve saved spawn point name or use default
        string spawnName = PlayerPrefs.GetString("SpawnPoint", "spawn_return");
        GameObject spawnPoint = GameObject.Find(spawnName);

        if (spawnPoint != null)
        {
            // Move player to spawn point position and rotation
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = spawnPoint.transform.position;
            player.transform.rotation = spawnPoint.transform.rotation;
        }
    }

    IEnumerator FadeOut()
    {
        // Gradually increase alpha to fade to black
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
        // Gradually decrease alpha to fade from black
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
}
