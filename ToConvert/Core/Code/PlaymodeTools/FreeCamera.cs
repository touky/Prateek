namespace Mayfair.Core.Code.PlaymodeTools
{
    using UnityEngine;

    /// <summary>
    ///     Controls:
    ///     wasd / arrows	- movement
    ///     q/e 			- up/down (local space)
    ///     r/f 			- up/down (world space)
    ///     pageup/pagedown	- up/down (world space)
    ///     hold shift		- enable fast movement mode
    ///     right mouse  	- enable free look
    ///     mouse			- free look / rotation
    /// </summary>
    public class FreeCamera : MonoBehaviour
    {
        [Tooltip("Normal speed of camera movement.")]
        public float baseMovementSpeed = 10f;

        [Tooltip("Speed of camera movement when shift is held down.")]
        public float fastMovementSpeed = 100f;

        [Tooltip("Sensitivity for free look.")]
        public float freeLookSensitivity = 3f;

        [Tooltip("Mouse wheel zoom step.")]
        public float baseZoomSensitivity = 10f;

        [Tooltip("Mouse wheel zoom step when shift is held down.")]
        public float fastZoomSensitivity = 50f;

        private bool isFreeLooking;

        #region  Unity Methods

        private void Update()
        {
            bool fastModeEnabled = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float movementSpeed = fastModeEnabled ? this.fastMovementSpeed : this.baseMovementSpeed;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position = transform.position + -transform.right * movementSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.position = transform.position + transform.right * movementSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                transform.position = transform.position + transform.forward * movementSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                transform.position = transform.position + -transform.forward * movementSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.E))
            {
                transform.position = transform.position + transform.up * movementSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                transform.position = transform.position + -transform.up * movementSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.PageUp))
            {
                transform.position = transform.position + Vector3.up * movementSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.PageDown))
            {
                transform.position = transform.position + -Vector3.up * movementSpeed * Time.deltaTime;
            }

            if (this.isFreeLooking)
            {
                float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * this.freeLookSensitivity;
                float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * this.freeLookSensitivity;
                transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
            }

            float axis = Input.GetAxis("Mouse ScrollWheel");
            if (axis != 0)
            {
                float zoomSensitivity = fastModeEnabled ? this.fastZoomSensitivity : this.baseZoomSensitivity;
                transform.position = transform.position + transform.forward * axis * zoomSensitivity;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                StartFreeLooking();
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                StopFreeLooking();
            }
        }

        private void OnDisable()
        {
            StopFreeLooking();
        }

        #endregion

        public void StartFreeLooking()
        {
            this.isFreeLooking = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void StopFreeLooking()
        {
            this.isFreeLooking = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}