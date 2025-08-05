using UnityEngine;
using TMPro;

public class UIShowUp : MonoBehaviour
{
    [Header("Task Settings")]
    public string taskDescription = "Go to sleep";
    public KeyCode completionKey = KeyCode.E;
    public TextMeshProUGUI taskTextUI;

    [Header("Progress Control")]
    public bool taskCompleted = false;
    public bool playerInside = false;
    public bool isCurrentTask = true; // Set this manually or link to a manager if needed

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isCurrentTask)
        {
            playerInside = true;
            ShowTaskUI();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    void Update()
    {
        if (playerInside && !taskCompleted && Input.GetKeyDown(completionKey))
        {
            CompleteTask();
        }
    }

    void ShowTaskUI()
    {
        if (taskTextUI != null)
        {
            taskTextUI.text = taskDescription;
        }
    }

    void CompleteTask()
    {
        taskCompleted = true;
        if (taskTextUI != null)
        {
            taskTextUI.text = $"<s>{taskDescription}</s>"; // Strike-through
        }

        // Optional: trigger next task or unlock movement
        Debug.Log("Task completed!");
    }
}
