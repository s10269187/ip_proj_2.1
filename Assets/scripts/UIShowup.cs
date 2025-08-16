/// <summary>
/// Displays and updates task UI based on the player's current zone.
/// Tracks task completion and applies strikethrough formatting.
/// </summary>
/// <author> Aralyn Han Zi Ning </author>
/// <date> 09/08/2025 </date>
/// <StudentID> S10267170A </StudentID>
using UnityEngine;
using TMPro;


public class UIShowUp : MonoBehaviour
{
    /// <summary>
    /// Represents a zone with associated UI and task triggers.
    /// </summary>
    [System.Serializable]
    public class ZoneUI
    {
        /// <summary>
        /// Label name for the zone.
        /// </summary>
        public string zoneName;

        /// <summary>
        /// Collider that defines the zone area.
        /// </summary>
        public Collider zoneTrigger;

        /// <summary>
        /// UI element for the zone title.
        /// </summary>
        public TextMeshProUGUI taskText;

        /// <summary>
        /// UI element for the divider line.
        /// </summary>
        public TextMeshProUGUI taskDash;

        /// <summary>
        /// UI element for task 1.
        /// </summary>
        public TextMeshProUGUI task1;

        /// <summary>
        /// UI element for task 2.
        /// </summary>
        public TextMeshProUGUI task2;

        /// <summary>
        /// UI element for task 3.
        /// </summary>
        public TextMeshProUGUI task3;

        /// <summary>
        /// Trigger collider for task 1.
        /// </summary>
        public Collider task1Trigger;

        /// <summary>
        /// Trigger collider for task 2.
        /// </summary>
        public Collider task2Trigger;

        /// <summary>
        /// Trigger collider for task 3.
        /// </summary>
        public Collider task3Trigger;

        /// <summary>
        /// Flag indicating whether task 1 is completed.
        /// </summary>
        [HideInInspector] public bool task1Struck = false;

        /// <summary>
        /// Flag indicating whether task 2 is completed.
        /// </summary>
        [HideInInspector] public bool task2Struck = false;

        /// <summary>
        /// Flag indicating whether task 3 is completed.
        /// </summary>
        [HideInInspector] public bool task3Struck = false;

        /// <summary>
        /// Shows all UI elements for the zone and applies strikethroughs to completed tasks.
        /// </summary>
        public void Show()
        {
            taskText?.gameObject.SetActive(true);
            taskDash?.gameObject.SetActive(true);
            task1?.gameObject.SetActive(true);
            task2?.gameObject.SetActive(true);
            task3?.gameObject.SetActive(true);

            ApplyStrikethroughs();
        }

        /// <summary>
        /// Hides all UI elements for the zone.
        /// </summary>
        public void Hide()
        {
            taskText?.gameObject.SetActive(false);
            taskDash?.gameObject.SetActive(false);
            task1?.gameObject.SetActive(false);
            task2?.gameObject.SetActive(false);
            task3?.gameObject.SetActive(false);
        }

        /// <summary>
        /// Applies strikethrough formatting to completed tasks.
        /// </summary>
        public void ApplyStrikethroughs()
        {
            if (task1Struck && task1 != null)
                task1.text = "<s>" + StripTags(task1.text) + "</s>";

            if (task2Struck && task2 != null)
                task2.text = "<s>" + StripTags(task2.text) + "</s>";

            if (task3Struck && task3 != null)
                task3.text = "<s>" + StripTags(task3.text) + "</s>";
        }

        /// <summary>
        /// Removes strikethrough tags from a string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>Cleaned string without formatting tags.</returns>
        private string StripTags(string input)
        {
            return input.Replace("<s>", "").Replace("</s>", "");
        }
    }

    /// <summary>
    /// List of all defined zones with their UI and triggers.
    /// </summary>
    public ZoneUI[] zones;

    /// <summary>
    /// The currently active zone the player is in.
    /// </summary>
    private ZoneUI currentZone = null;

    /// <summary>
    /// Reference to the player's transform used for zone detection.
    /// </summary>
    public Transform playerTransform;

    /// <summary>
    /// Hides all zone UI elements at the start of the game.
    /// </summary>
    void Start()
    {
        foreach (var zone in zones)
        {
            zone.Hide();
        }
    }

    /// <summary>
    /// Checks which zone the player is currently in and updates the UI accordingly.
    /// </summary>
    void Update()
    {
        ZoneUI activeZone = null;

        foreach (var zone in zones)
        {
            if (zone.zoneTrigger != null && zone.zoneTrigger.bounds.Contains(playerTransform.position))
            {
                activeZone = zone;
                break;
            }
        }

        if (activeZone != currentZone)
        {
            currentZone?.Hide();
            activeZone?.Show();
            currentZone = activeZone;
        }
    }

    /// <summary>
    /// Detects when the player enters a task trigger and marks the corresponding task as completed.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    void OnTriggerEnter(Collider other)
    {
        foreach (var zone in zones)
        {
            if (zone.task1Trigger != null && other == zone.task1Trigger && !zone.task1Struck)
            {
                zone.task1Struck = true;
                zone.ApplyStrikethroughs();
            }

            if (zone.task2Trigger != null && other == zone.task2Trigger && !zone.task2Struck)
            {
                zone.task2Struck = true;
                zone.ApplyStrikethroughs();
            }

            if (zone.task3Trigger != null && other == zone.task3Trigger && !zone.task3Struck)
            {
                zone.task3Struck = true;
                zone.ApplyStrikethroughs();
            }
        }
    }
}
