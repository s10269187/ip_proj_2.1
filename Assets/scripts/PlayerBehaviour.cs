using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    Transform spawnPoint; // Reference to the spawn point in the scene
    [SerializeField]
    private float interactionDistance = 2f; // Distance within which the player can interact with doors
    
    bool canInteract = false; // Flag to check if the player can interact with the door
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    

    

}
