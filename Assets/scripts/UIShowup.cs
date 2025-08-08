using UnityEngine;
using TMPro;

public class UIShowUp : MonoBehaviour
{
    [System.Serializable]
    public class ZoneUI
    {
        public string zoneName;
        
        public Collider triggerZone;

        [Header("Shared UI")]
        public TextMeshProUGUI taskText;
        public TextMeshProUGUI taskDash;

        [Header("Zone-Specific Tasks")]
        public TextMeshProUGUI task1;
        public TextMeshProUGUI task2;

        public void Show()
        {
            taskText?.gameObject.SetActive(true);
            taskDash?.gameObject.SetActive(true);
            task1?.gameObject.SetActive(true);
            task2?.gameObject.SetActive(true);
        }

        public void Hide()
        {
            taskText?.gameObject.SetActive(false);
            taskDash?.gameObject.SetActive(false);
            task1?.gameObject.SetActive(false);
            task2?.gameObject.SetActive(false);
        }
    }

    

    [Header("Zones and Their UI")]
    public ZoneUI[] zones;

    private ZoneUI currentZone = null;

    void Start()
    {
        foreach (var zone in zones)
        {
            zone.Hide();
        }
    }

    public Transform playerTransform; // Assign in Inspector


    void Update()
    {
        ZoneUI activeZone = null;

        foreach (var zone in zones)
        {
            if (zone.triggerZone != null && zone.triggerZone.bounds.Contains(transform.position))
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
}
