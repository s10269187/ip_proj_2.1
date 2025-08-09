using UnityEngine;

public class lavaobstacle : MonoBehaviour
{
    public Transform spawnPoint;                // Current respawn point
    public string hazardTag = "Lava";
    public string checkpointTag = "Checkpoint"; // Tag for checkpoints

    private void OnTriggerEnter(Collider other)
    {
        // If player touches a hazard, reset to current spawn point
        if (other.CompareTag(hazardTag))
        {
            ResetPlayer();
        }

        // If player touches a checkpoint, update spawn point
        if (other.CompareTag(checkpointTag))
        {
            UpdateSpawnPoint(other.transform);
        }
    }

    void ResetPlayer()
    {
        transform.position = spawnPoint.position;

        // Optional: reset Rigidbody velocity
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("Player reset to spawn point.");
    }

    void UpdateSpawnPoint(Transform newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
        Debug.Log("Checkpoint reached. Spawn point updated.");
    }
}
