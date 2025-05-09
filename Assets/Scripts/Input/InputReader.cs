using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SwedishApp.Input
{
    /// <summary>
    /// This class handles reading the program's inputs.
    /// </summary>
    [CreateAssetMenu (menuName = "InputReader")]
    public class InputReader : ScriptableObject, InputMap.IUIActions
    {
        private InputMap inputMap;
        public bool InputsEnabled => inputMap.UI.enabled;

        //UI events
        public event Action<Vector2> NavigateEvent;
        public event Action<Vector2> PointEvent;
        public event Action<Vector2> ScrollEvent;
        public event Action<Vector3> PositionChangeEvent;
        public event Action<Quaternion> RotationChangeEvent;
        public event Action SubmitEvent;
        public event Action SubmitEventCancelled;
        public event Action CancelEvent;
        public event Action CancelEventCancelled;
        public event Action TabEvent;
        public event Action TabEventCancelled;
        public event Action ClickEvent;
        public event Action ClickEventCancelled;
        public event Action RightClickEvent;
        public event Action RightClickEventCancelled;
        public event Action MiddleClickEvent;
        public event Action MiddleClickEventCancelled;

        /// <summary>
        /// This method initializes an InputMap if one isn't already initialized,
        /// and sets input callbacks to use it. The app's controls are also enabled.
        /// </summary>
        private void OnEnable()
        {
            if (inputMap == null)
            {
                inputMap = new();
                inputMap.UI.SetCallbacks(this);
                inputMap.UI.Enable();
            }
        }
        
        private void OnDisable()
        {
            inputMap?.UI.Disable();
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            NavigateEvent?.Invoke(obj:context.ReadValue<Vector2>());
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                SubmitEvent?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                SubmitEventCancelled?.Invoke();
            }
        }

        public void OnTab(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                TabEvent?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                TabEventCancelled?.Invoke();
            }
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                CancelEvent?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                CancelEventCancelled?.Invoke();
            }
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            PointEvent?.Invoke(obj:context.ReadValue<Vector2>());
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                ClickEvent?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                ClickEventCancelled?.Invoke();
            }
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                RightClickEvent?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                RightClickEventCancelled?.Invoke();
            }
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                MiddleClickEvent?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                MiddleClickEventCancelled?.Invoke();
            }
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            ScrollEvent?.Invoke(obj:context.ReadValue<Vector2>());
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
            PositionChangeEvent?.Invoke(obj:context.ReadValue<Vector3>());
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
            RotationChangeEvent?.Invoke(obj:context.ReadValue<Quaternion>());
        }
    }
}