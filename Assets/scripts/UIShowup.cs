using UnityEngine;
using TMPro;

// Show and update task UI based on player zone
public class UIShowUp : MonoBehaviour
{
    [System.Serializable]
    public class ZoneUI
    {
        // Zone label
        public string zoneName;       
        // Zone area          
        public Collider zoneTrigger;

        // Shared UI
        // Zone title
        public TextMeshProUGUI taskText;   
        // Divider     
        public TextMeshProUGUI taskDash;

        // Zone-specific tasks
        // Task 1 text
        public TextMeshProUGUI task1;           
        // Task 2 text
        public TextMeshProUGUI task2; 
        // Task 3 text          
        public TextMeshProUGUI task3;

        // Task triggers
        // Task 1 trigger
        public Collider task1Trigger; 
        // Task 2 trigger          
        public Collider task2Trigger;  
        // Task 3 trigger         
        public Collider task3Trigger;

        // Task completion flags
        // Task 1 done
        [HideInInspector] public bool task1Struck = false; 
        // Task 2 done
        [HideInInspector] public bool task2Struck = false; 
        // Task 3 done
        [HideInInspector] public bool task3Struck = false;

        // Show all UI elements
        // Show zone title, divider, and tasks
        // Apply strikethroughs
        public void Show()
        {
            taskText?.gameObject.SetActive(true);
            taskDash?.gameObject.SetActive(true);
            task1?.gameObject.SetActive(true);
            task2?.gameObject.SetActive(true);
            task3?.gameObject.SetActive(true);

            ApplyStrikethroughs();
        }

        // Hide all UI elements
        // Hide zone title
        // Hide divider
        // Hide task 1
        // Hide task 2
        // Hide task 3
        public void Hide()
        {
            taskText?.gameObject.SetActive(false);
            taskDash?.gameObject.SetActive(false);
            task1?.gameObject.SetActive(false);
            task2?.gameObject.SetActive(false);    
            task3?.gameObject.SetActive(false);    
        }

        // Apply strikethroughs to completed tasks]
        // Strike task 1
        // Strike task 2
        // Strike task 3
        public void ApplyStrikethroughs()
        {
            if (task1Struck && task1 != null)
                task1.text = "<s>" + StripTags(task1.text) + "</s>";

            if (task2Struck && task2 != null)
                task2.text = "<s>" + StripTags(task2.text) + "</s>";

            if (task3Struck && task3 != null)
                task3.text = "<s>" + StripTags(task3.text) + "</s>";
        }

        // Remove strikethrough tags
        // Clean text
        private string StripTags(string input)
        {
            return input.Replace("<s>", "").Replace("</s>", "");
        }
    }

    // List of zones
    public ZoneUI[] zones;

    // Currently active zone
    private ZoneUI currentZone = null;

    // Player position reference
    public Transform playerTransform;

    void Start()
    {
        // Hide all zone UI at start
        foreach (var zone in zones)
        {
            zone.Hide();
        }
    }

    void Update()
    {
        ZoneUI activeZone = null;

        // Check which zone player is in
        // Set active zone
        foreach (var zone in zones)
        {
            if (zone.zoneTrigger != null && zone.zoneTrigger.bounds.Contains(playerTransform.position))
            {
                activeZone = zone;
                break;
            }
        }

        // Switch UI if zone changed
        // Hide previous zone UI
        // Show new zone UI
        if (activeZone != currentZone)
        {
            currentZone?.Hide();
            activeZone?.Show();
            currentZone = activeZone;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check task triggers
        // Mark tasks done
        // Update UI
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
