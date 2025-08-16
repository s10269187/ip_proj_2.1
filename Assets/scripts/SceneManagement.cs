using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Handles scene transitions with fade effects and player teleportation to designated spawn points.
/// </summary>
public class SceneManagement : MonoBehaviour
{
    /// <summary>
    /// UI image used to perform fade in/out effects.
    /// </summary>
    [Header("Fade Settings")]
    public Image fadeImage;

    /// <summary>
    /// Duration of the fade effect in seconds.
    /// </summary>
    public float fadeDuration = 1f;

    /// <summary>
    /// Name of the scene to load when teleporting.
    /// </summary>
    [Header("Teleport Settings")]
    public string targetScene = "RETURN_ms";

    /// <summary>
    /// Name of the spawn point in the target scene.
    /// </summary>
    public string spawnPointName = "spawn_return";

    /// <summary>
    /// Loads the main scene immediately.
    /// </summary>
    public void LoadMainScene()
    {
        SceneManager.LoadScene("mainscene");
    }

    /// <summary>
    /// Called on start. If the current scene matches the target scene,
    /// moves the player to the designated spawn point and fades in.
    /// </summary>
    void Start()
    {
        if (SceneManager.GetActiveScene().name == targetScene)
        {
            MovePlayerToSpawn();
            StartCoroutine(FadeIn());
        }
    }

    /// <summary>
    /// Detects when the player enters the trigger and initiates the teleport sequence.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeTeleport());
        }
    }

    /// <summary>
    /// Coroutine that fades out, saves the spawn point name, and loads the target scene.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    IEnumerator FadeTeleport()
    {
        yield return StartCoroutine(FadeOut());

        PlayerPrefs.SetString("SpawnPoint", spawnPointName);
        SceneManager.LoadScene(targetScene);
    }

    /// <summary>
    /// Moves the player to the saved or default spawn point in the scene.
    /// </summary>
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
}
