using UnityEngine;

/// <summary>
/// Continuously rotates a fan blade around its local Z-axis.
/// </summary>
public class FanBlade : MonoBehaviour
{
    /// <summary>
    /// Rotation speed in degrees per second.
    /// </summary>
    public float rotationSpeed = 500f;

    /// <summary>
    /// Rotates the fan blade every frame based on the specified speed.
    /// </summary>
    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
