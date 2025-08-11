using UnityEngine;
using TMPro;
public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] Transform spawnPoint;

        [Header("UI Settings")]
        public TextMeshProUGUI promptText;

        private DoorBehaviour currentDoor;
        public KeyCode interactKey = KeyCode.E;
        public Camera playerCamera;
        public float interactDistance = 3f;

        private PickUp pickUpScript;

        void Start()
        {
            pickUpScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PickUp>();
            if (promptText != null)
                promptText.text = "";
            Debug.Log("PlayerBehaviour started. Camera assigned.");

            string spawnName = PlayerPrefs.GetString("SpawnPointName", "");

            if (spawnName != "")
            {
                GameObject spawnPoint = GameObject.Find(spawnName);
                if (spawnPoint != null)
                {
                    transform.position = spawnPoint.transform.position;
                }

                // Clear it so it doesn't affect future spawns
                PlayerPrefs.DeleteKey("SpawnPointName");
            }

        }

        void Update()
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                GameObject hitObject = hit.collider.gameObject;
                Debug.Log("Raycast hit: " + hitObject.name);

                if (hitObject.CompareTag("Door"))
                {
                    promptText.enabled = true;
                    promptText.text = "Press E to Interact";
                    currentDoor = hitObject.GetComponent<DoorBehaviour>();

                    if (Input.GetKeyDown(interactKey) && currentDoor != null)
                    {
                        currentDoor.ToggleDoor();
                    }
                }
                else if (hitObject.CompareTag("Placeable"))
                {
                    promptText.enabled = true;
                    promptText.text = "Press Q to Place item";

                    PlaceAndTransferMaterial placeScript = hitObject.GetComponent<PlaceAndTransferMaterial>();

                    if (Input.GetKeyDown(KeyCode.Q) && placeScript != null && pickUpScript.heldObj != null)
                    {   
                        Debug.Log("heldObj: " + pickUpScript.heldObj);
                        placeScript.PlaceObject(pickUpScript.heldObj);
                    }
                }
                else
                {
                    ClearPrompt();
                }
                 
            }
            
            else
            {
                ClearPrompt();
            }
            
            
        }

        void ClearPrompt()
            {
                if (promptText != null)
                {
                    promptText.enabled = false;
                    promptText.text = "";
                }

                currentDoor = null;
            }
}
