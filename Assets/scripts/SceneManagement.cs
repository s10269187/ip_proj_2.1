using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneManagement : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage; // Assign in Inspector
    public float fadeDuration = 1f;

    [Header("Teleport Settings")]
    public string targetScene = "RETURN_ms";
    public string spawnPointName = "spawn_return";

    void Start()
    {
        // If we're in the RETURN_ms scene, move player to spawn point
        if (SceneManager.GetActiveScene().name == targetScene)
        {
            MovePlayerToSpawn();
            StartCoroutine(FadeIn());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeTeleport());
        }
    }

    IEnumerator FadeTeleport()
    {
        yield return StartCoroutine(FadeOut());

        // Save spawn point name
        PlayerPrefs.SetString("SpawnPoint", spawnPointName);

        // Load target scene
        SceneManager.LoadScene(targetScene);
    }

    void MovePlayerToSpawn()
    {
        string spawnName = PlayerPrefs.GetString("SpawnPoint", "spawn_return");
        GameObject spawnPoint = GameObject.Find(spawnName);

        if (spawnPoint != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = spawnPoint.transform.position;
            player.transform.rotation = spawnPoint.transform.rotation;
        }
        else
        {
            Debug.LogWarning("Spawn point not found: " + spawnName);
        }
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
}