/// <summary>
/// lavaobstacle.cs
/// This script handles the lava obstacle in the final stage of our haunted house
/// When player steps onto the lava, the player automatically dies and respawn
/// at the spawn point set in Unity as an empty GameObject
/// </summary>
/// <author> Lee Jia Lu </author>
/// <date> 09/08/2025 </date>
/// <StudentID> S10269187E </StudentID>

using UnityEngine;

/// <summary>
/// Enables the trigger for the lava
/// </summary>
public class lavaobstacle : MonoBehaviour
{
    public Transform spawnPoint;

    public AudioSource lava;
    
    /// <summary>
    /// When player touch it, they respawn to spawn point
    /// </summary>
    
    public void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (spawnPoint != null)
            {
                other.transform.position = spawnPoint.position;
                Debug.Log("Teleporting to: " + spawnPoint.position);
                lava.Play();

                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
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

}
