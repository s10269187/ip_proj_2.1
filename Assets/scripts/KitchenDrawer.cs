using UnityEngine;
using TMPro;

public class SlidingDrawerWithPrompt : MonoBehaviour
{
    public Transform playerCamera;
    public float rayDistance = 6f;
    public float interactDistance = 4f;
    public KeyCode interactKey = KeyCode.E;
    public Vector3 slideOffset = new Vector3(0.5f, 0f, 0f);
    public float slideSpeed = 2f;
    public TextMeshProUGUI promptText;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;

    void Start()
    {
        closedPosition = transform.localPosition;
        openPosition = closedPosition + slideOffset;

        if (promptText != null)
            promptText.enabled = false;
    }

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
