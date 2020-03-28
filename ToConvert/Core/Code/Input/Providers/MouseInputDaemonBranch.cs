namespace Mayfair.Core.Code.Input.Providers
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Utils;
    using UnityEngine;

    public class MouseInputDaemonBranch : InputDaemonBranch
    {
        #region Static and Constants
        private static Vector2 lastMousePosition = Vector3.zero;
        private List<Touch> fakeTouches = new List<Touch>();
        private bool oneFrameDelay = false;
        #endregion

        #region Properties
        protected override bool IsAliveInternal
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }

        public override int Priority
        {
            get { return Consts.SECOND_ITEM; }
        }
        #endregion

        #region Class Methods
        /// <summary>
        ///     Uses your pc mouse to simulate touches on a device
        /// </summary>
        /// <remarks>
        ///     - Hold ctrl key to simulate a second touch at center screen
        ///     - Hold alt to simulate a second touch on the opposite side of the x-axis
        ///     - Hold shift to simulate a second touch adjacent on the right
        ///     - Hold more to test behaviors with 3 simultaneous touch
        /// </remarks>
        public override void GatherInput(List<Touch> touches)
        {
            const int MouseFingerId = -1;
            const int CtrlFingerId = 1;
            const int AltFingerId = 2;
            const int ShiftFingerId = 3;

            Touch fakeTouch = new Touch
            {
                deltaTime = Time.deltaTime,
                tapCount = 1 //unhandled
            };

            fakeTouches.Clear();

            //hold ctrl key to simulate a second finger at center screen
            if (Input.GetKey(KeyCode.LeftControl))
            {
                fakeTouch.fingerId = CtrlFingerId;
                fakeTouch.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                fakeTouch.deltaPosition = Vector2.zero;
                fakeTouch.phase = GetMousePhase(Input.GetKeyDown(KeyCode.LeftControl), fakeTouch.deltaPosition);

                fakeTouches.Add(fakeTouch);
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                fakeTouch.fingerId = CtrlFingerId;
                fakeTouch.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                fakeTouch.deltaPosition = Vector2.zero;
                fakeTouch.phase = TouchPhase.Ended;

                fakeTouches.Add(fakeTouch);
            }

            //hold alt key to simulate a second finger on the opposite side of the x-axis
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                fakeTouch.fingerId = AltFingerId;
                fakeTouch.position = new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y, Input.mousePosition.z);
                fakeTouch.deltaPosition = fakeTouch.position - new Vector2(lastMousePosition.x, Screen.height - lastMousePosition.y);
                fakeTouch.phase = GetMousePhase(Input.GetKeyDown(KeyCode.LeftAlt), fakeTouch.deltaPosition);

                fakeTouches.Add(fakeTouch);
            }
            else if (Input.GetKeyUp(KeyCode.LeftAlt))
            {
                fakeTouch.fingerId = AltFingerId;
                fakeTouch.position = new Vector2(lastMousePosition.x, Screen.height - lastMousePosition.y);
                fakeTouch.deltaPosition = Vector2.zero;
                fakeTouch.phase = TouchPhase.Ended;

                fakeTouches.Add(fakeTouch);
            }

            //hold shift key to simulate a second finger adjacent on the right
            if (Input.GetKey(KeyCode.LeftShift))
            {
                fakeTouch.fingerId = ShiftFingerId;
                fakeTouch.position = new Vector3(Input.mousePosition.x + 50, Input.mousePosition.y, Input.mousePosition.z);
                fakeTouch.deltaPosition = fakeTouch.position - new Vector2(lastMousePosition.x + 50, lastMousePosition.y);
                fakeTouch.phase = GetMousePhase(Input.GetKeyDown(KeyCode.LeftShift), fakeTouch.deltaPosition);

                fakeTouches.Add(fakeTouch);
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                fakeTouch.fingerId = ShiftFingerId;
                fakeTouch.position = new Vector2(lastMousePosition.x + 50, lastMousePosition.y);
                fakeTouch.deltaPosition = Vector2.zero;
                fakeTouch.phase = TouchPhase.Ended;

                fakeTouches.Add(fakeTouch);
            }

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
            {
                if (Input.GetMouseButton(0))
                {
                    fakeTouch.fingerId = MouseFingerId;
                    fakeTouch.position = Input.mousePosition;
                    fakeTouch.deltaPosition = fakeTouch.position - lastMousePosition;
                    fakeTouch.phase = GetMousePhase(Input.GetMouseButtonDown(0), fakeTouch.deltaPosition);

                    fakeTouches.Add(fakeTouch);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    fakeTouch.fingerId = MouseFingerId;
                    fakeTouch.position = lastMousePosition;
                    fakeTouch.deltaPosition = Vector2.zero;
                    fakeTouch.phase = TouchPhase.Ended;

                    fakeTouches.Add(fakeTouch);
                }

                touches.AddRange(fakeTouches);
            }

            lastMousePosition = Input.mousePosition;
        }

        private static TouchPhase GetMousePhase(bool gotKeyDown, Vector2 delta)
        {
            return gotKeyDown ? TouchPhase.Began :
                delta.sqrMagnitude >= 1f ? TouchPhase.Moved :
                TouchPhase.Stationary;
        }
        #endregion
    }
}
