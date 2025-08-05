using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    Transform spawnPoint; // Reference to the spawn point in the scene
    [SerializeField]
    private float interactionDistance = 2f; // Distance within which the player can interact with doors

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider != null && hit.collider.GetComponent<BoxCollider>() != null && hit.collider.CompareTag("Door"))
            {
                Debug.Log("You are looking at a door! Press E to interact.");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Door interacted!");
                    // Add your door opening logic here
                }
            }
        }
    }
}