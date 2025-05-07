using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SwedishApp.Input
{
    /// <summary>
    /// This class handles reading the program's inputs.
    /// </summary>
    [CreateAssetMenu (menuName = "InputReader")]
    public class InputReader : ScriptableObject, InputMap.IAppControlsActions
    {
        private InputMap inputMap;
        public bool InputsEnabled => inputMap.AppControls.enabled;

        public event Action TabEvent;
        public event Action TabEventCancelled;
        public event Action EnterEvent;
        public event Action EnterEventCancelled;
        public event Action LeftEvent;
        public event Action LeftEventCancelled;
        public event Action RightEvent;
        public event Action RightEventCancelled;

        /// <summary>
        /// This method initializes an InputMap if one isn't already initialized,
        /// and sets input callbacks to use it. The app's controls are also enabled.
        /// </summary>
        private void OnEnable()
        {
            if (inputMap == null)
            {
                inputMap = new();
                inputMap.AppControls.SetCallbacks(this);
                inputMap.AppControls.Enable();
            }
        }
        
        private void OnDestroy()
        {
            inputMap?.AppControls.Disable();
        }

        /// <summary>
        /// This method handles invoking events tied to the state of the enter-key
        /// </summary>
        /// <param name="context">This parameter indicates the state of the key</param>
        public void OnEnterAction(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                EnterEvent?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                EnterEventCancelled?.Invoke();
            }
        }

        /// <summary>
        /// This method handles invoking events tied to the state of the tab-key
        /// </summary>
        /// <param name="context">This parameter indicates the state of the key</param>
        public void OnTabAction(InputAction.CallbackContext context)
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

        /// <summary>
        /// This method handles invoking events tied to the state of the left-input
        /// </summary>
        /// <param name="context">This parameter indicates the state of the input</param>
        public void OnLeftAction(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                LeftEvent?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                LeftEventCancelled?.Invoke();
            }
        }

        /// <summary>
        /// This method handles invoking events tied to the state of the right-input
        /// </summary>
        /// <param name="context">This parameter indicates the state of the input</param>
        public void OnRightAction(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                RightEvent?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                RightEventCancelled?.Invoke();
            }
        }
    }
}
