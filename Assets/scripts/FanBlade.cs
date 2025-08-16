/// <summary>
/// Continuously rotates a fan blade around its local Z-axis.
/// </summary>
/// <author> Aralyn Han Zi Ning </author>
/// <date> 02/08/2025 </date>
/// <StudentID> S10267170A </StudentID>
using UnityEngine;
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
