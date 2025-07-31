using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    Transform spawnPoint; // Reference to the spawn point in the scene
    [SerializeField]
    private float interactionDistance = 2f; // Distance within which the player can interact with doors
    DoorBehaviour currentDoor=null; // Reference to the DoorBehaviour script
    bool canInteract = false; // Flag to check if the player can interact with the door
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            
            Debug.Log("Raycast hit: " + hitInfo.collider.name);
            Debug.DrawLine(spawnPoint.position, hitInfo.point, Color.red); // Visualize the raycast in the scene view
            if (hitInfo.collider.CompareTag("Door"))
            {
                Debug.Log("Looking at door: " + hitInfo.collider.name);
                currentDoor = hitInfo.collider.GetComponent<DoorBehaviour>();
                canInteract = true; // Set a flag to indicate that the player can interact with the door
            }
            else
            {
                currentDoor = null; // Reset the door reference if not looking at a door
                canInteract = false; // Reset the interaction flag
            }
        }
            
    }

    

}
