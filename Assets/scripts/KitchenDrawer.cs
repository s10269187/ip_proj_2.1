using UnityEngine;
using TMPro;

/// <summary>
/// Allows the player to interact with a sliding drawer using a key press,
/// showing a UI prompt when within range and smoothly animating the drawer open or closed.
/// </summary>
public class SlidingDrawerWithPrompt : MonoBehaviour
{
    /// <summary>
    /// Reference to the player's camera used for raycasting.
    /// </summary>
    public Transform playerCamera;

    /// <summary>
    /// Maximum distance the raycast can reach.
    /// </summary>
    public float rayDistance = 6f;

    /// <summary>
    /// Distance within which the player can interact with the drawer.
    /// </summary>
    public float interactDistance = 4f;

    /// <summary>
    /// Key used to toggle the drawer open or closed.
    /// </summary>
    public KeyCode interactKey = KeyCode.E;

    /// <summary>
    /// The local position offset applied when the drawer is opened.
    /// </summary>
    public Vector3 slideOffset = new Vector3(0.5f, 0f, 0f);

    /// <summary>
    /// Speed at which the drawer slides open or closed.
    /// </summary>
    public float slideSpeed = 2f;

    /// <summary>
    /// UI text component used to display the interaction prompt.
    /// </summary>
    public TextMeshProUGUI promptText;

    /// <summary>
    /// The drawer's original closed position.
    /// </summary>
    private Vector3 closedPosition;

    /// <summary>
    /// The drawer's target open position.
    /// </summary>
    private Vector3 openPosition;

    /// <summary>
    /// Indicates whether the drawer is currently open.
    /// </summary>
    private bool isOpen = false;

    /// <summary>
    /// Initializes the drawer's position and hides the prompt.
    /// </summary>
    void Start()
    {
        closedPosition = transform.localPosition;
        openPosition = closedPosition + slideOffset;

        if (promptText != null)
            promptText.enabled = false;
    }

    /// <summary>
    /// Handles raycasting for interaction detection and animates the drawer's movement.
    /// </summary>
    void Update()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            float hitDistance = hit.distance;

            if (hit.transform == transform && hitDistance <= interactDistance)
            {
                if (promptText != null)
                    promptText.enabled = true;

                if (Input.GetKeyDown(interactKey))
                    isOpen = !isOpen;
            }
            else
            {
                if (promptText != null)
                    promptText.enabled = false;
            }
        }
        else
        {
            if (promptText != null)
                promptText.enabled = false;
        }

        Vector3 targetPosition = isOpen ? openPosition : closedPosition;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * slideSpeed);
    }
}
