using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : MonoBehaviour
{
    public string targetSceneName;
    public string spawnPointNameInTargetScene;  // Name of the spawn point in the target scene

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetString("SpawnPointName", spawnPointNameInTargetScene);
            SceneManager.LoadScene(targetSceneName);
        }
    }
}