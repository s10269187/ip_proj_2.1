using System.Collections;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{       
    public Transform door1;
        public Transform door2;
        public Transform player;
        public float interactionDistance = 3f;
        public Vector3 openRotation = new Vector3(0, 90, 0);
        public float openSpeed = 5f;

        private bool isOpen = false;

        void Update()
        {
            float distance = Vector3.Distance(player.position, transform.position);

            if (distance <= interactionDistance && Input.GetKeyDown(KeyCode.E))
            {
                isOpen = !isOpen;
            }

            Quaternion targetRot = isOpen ? Quaternion.Euler(openRotation) : Quaternion.identity;
            door1.localRotation = Quaternion.Slerp(door1.localRotation, targetRot, Time.deltaTime * openSpeed);
            door2.localRotation = Quaternion.Slerp(door2.localRotation, targetRot, Time.deltaTime * openSpeed);
        }
}
