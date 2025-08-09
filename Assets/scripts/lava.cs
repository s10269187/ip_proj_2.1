using UnityEngine;

public class lava : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
            void OnTriggerStay(Collider other)
    {
        // Check cooldown
        if (Time.time - lastDamageTime < damageCooldown) return;

        if (other.CompareTag("Lava"))
        {
            health -= 10;
            lastDamageTime = Time.time;
        }
        
        // Clamp health and check for respawn
        if (health < 0) health = 0;
        if (health == 0)
        {
            Respawn();
        }
    // Update is called once per frame
        void Update()
        {
            
        }
    }
}
