using UnityEngine;

public class FanBlade : MonoBehaviour
{
    public float rotationSpeed = 500f; // Degrees per second

    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
