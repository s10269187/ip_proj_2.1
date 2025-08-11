using UnityEngine;
using TMPro;

public class UIShowUp : MonoBehaviour
{
    [System.Serializable]
    public class ZoneUI
    {
        public string zoneName;
        public Collider zoneTrigger;

        [Header("Shared UI")]
        public TextMeshProUGUI taskText;
        public TextMeshProUGUI taskDash;

        [Header("Zone-Specific Tasks")]
        public TextMeshProUGUI task1;
        public TextMeshProUGUI task2;
        public TextMeshProUGUI task3;

        [Header("Task Colliders")]
        public Collider task1Trigger;
        public Collider task2Trigger;
        public Collider task3Trigger;

        [HideInInspector] public bool task1Struck = false;
        [HideInInspector] public bool task2Struck = false;
        [HideInInspector] public bool task3Struck = false;

        public void Show()
        {
            taskText?.gameObject.SetActive(true);
            taskDash?.gameObject.SetActive(true);
            task1?.gameObject.SetActive(true);
            task2?.gameObject.SetActive(true);
            task3?.gameObject.SetActive(true);

            ApplyStrikethroughs();
        }

        public void Hide()
        {
            taskText?.gameObject.SetActive(false);
            taskDash?.gameObject.SetActive(false);
            task1?.gameObject.SetActive(false);
            task2?.gameObject.SetActive(false);
            task3?.gameObject.SetActive(false);
        }

        public void ApplyStrikethroughs()
        {
            if (task1Struck && task1 != null)
                task1.text = "<s>" + StripTags(task1.text) + "</s>";

            if (task2Struck && task2 != null)
                task2.text = "<s>" + StripTags(task2.text) + "</s>";

            if (task3Struck && task3 != null)
                task3.text = "<s>" + StripTags(task3.text) + "</s>";
        }

        private string StripTags(string input)
        {
            return input.Replace("<s>", "").Replace("</s>", "");
        }
    }

    [Header("Zones and Their UI")]
    public ZoneUI[] zones;

    private ZoneUI currentZone = null;

    [Header("Player Reference")]
    public Transform playerTransform; // Assign in Inspector

    void Start()
    {
        foreach (var zone in zones)
        {
            zone.Hide();
        }
    }

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

    private void OnTriggerEnter(Collider other)
    {
        foreach (var zone in zones)
        {
            if (zone.task1Trigger != null && other == zone.task1Trigger && !zone.task1Struck)
            {
                zone.task1Struck = true;
                zone.ApplyStrikethroughs();
                Debug.Log($"Task 1 struck in zone: {zone.zoneName}");
            }

            if (zone.task2Trigger != null && other == zone.task2Trigger && !zone.task2Struck)
            {
                zone.task2Struck = true;
                zone.ApplyStrikethroughs();
                Debug.Log($"Task 2 struck in zone: {zone.zoneName}");
            }

            if (zone.task3Trigger != null && other == zone.task3Trigger && !zone.task3Struck)
            {
                zone.task3Struck = true;
                zone.ApplyStrikethroughs();
                Debug.Log($"Task 3 struck in zone: {zone.zoneName}");
            }
        }
    }
}
