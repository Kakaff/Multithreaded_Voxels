using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets
{
    public class MouseLook : MonoBehaviour
    {
        public float sensitivity = 3F;
        public float minimumX = -360F;
        public float maximumX = 360F;
        public float minimumY = -89f;
        public float maximumY = 89f;
        float rotationX = 0F;
        float rotationY = 0F;

        float lastXAxisVal,lastYAxisVal;
        
        void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (Cursor.visible)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }

            PlayerLook();
        }
        void PlayerLook()
        {
            float xVal = Input.GetAxis("Mouse X") + lastXAxisVal;
            float yVal = Input.GetAxis("Mouse Y") + lastYAxisVal;

            lastXAxisVal = Input.GetAxis("Mouse X");
            lastYAxisVal = Input.GetAxis("Mouse Y");

            if (!Cursor.visible)
            {
                rotationX += xVal * sensitivity;
                rotationY -= yVal * sensitivity;
                rotationX = ClampAngle(rotationX, minimumX, maximumX);
                rotationY = ClampAngle(rotationY, minimumY, maximumY);

                transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);
            }
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }
    }
}
