using UnityEngine;

public class Matchbox : MonoBehaviour
{
    public Transform cabinetPart; // The moving door or drawer
    public Vector3 localOffset = new Vector3(0.2f, 0f, 0f); // Offset from the cabinet part

    void Update()
    {
        transform.position = cabinetPart.TransformPoint(localOffset);
    }
}
