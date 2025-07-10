using SwedishApp.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SwedishApp.Input
{
    /// <summary>
    /// This class should be attached to a holder housing an input field for each character in a word.
    /// If an input field is active, navigation is enabled.
    /// </summary>
    public class InputFieldHandling : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        private TMP_InputField inputField;
        private RectTransform holder;
        public int index;
        private bool controlHeld = false;
        private bool disableGoNext = false;

        private void Awake()
        {
            holder = GetComponent<RectTransform>();
            inputReader.NavigateEvent += Navigate;
            inputReader.ControlEvent += ControlDownHandler;
            inputReader.ControlEventCancelled += ControlUpHandler;
            inputReader.BackspaceEvent += BackspaceDownHandler;
        }

        private void OnDestroy()
        {
            inputField.DeactivateInputField();
            inputReader.NavigateEvent -= Navigate;
            inputReader.ControlEvent -= ControlDownHandler;
            inputReader.ControlEventCancelled -= ControlUpHandler;
            inputReader.BackspaceEvent -= BackspaceDownHandler;
        }

        private void ControlDownHandler()
        {
            controlHeld = true;
        }

        private void ControlUpHandler()
        {
            controlHeld = false;
        }

        private void BackspaceDownHandler()
        {
            if (controlHeld)
            {
                TMP_InputField[] inputFields = GetComponentsInChildren<TMP_InputField>();
                disableGoNext = true;

                for (int i = index; i >= 0; i--)
                {
                    inputFields[i].text = "";
                }

                index = 0;
                inputField = holder.GetChild(index).GetComponent<TMP_InputField>();
                inputField.ActivateInputField();
                disableGoNext = false;
            }
        }

        /// <summary>
        /// This should be called when an input field is selected. This sets the index to the active
        /// input field's index, and sets the inputField reference to the active input field.
        /// </summary>
        /// <param name="_index">This parameter is defined when subscribing this method to an input
        /// field's OnSelect() event. It should be set equal to the child index of the input field.</param>
        public void GetActiveIndex(int _index)
        {
            index = _index;
            inputField = holder.GetChild(_index).GetComponent<TMP_InputField>();
            inputField.caretPosition = 0;
        }

        /// <summary>
        /// This method navigates between input fields based on the input events, taking into account
        /// the index limits. If the next field is space, it's not interactable and is skipped.
        /// </summary>
        /// <param name="_input">This parameter is received from the input event which invokes
        /// this method</param>
        private void Navigate(Vector2 _input)
        {
            if (!inputField.IsActive()) return;

            if (_input.y > 0)
            {

                index = 0;
                holder.GetChild(index).GetComponent<TMP_InputField>().ActivateInputField();
                return;
            }
            else if (_input.y < 0)
            {

                index = holder.childCount - 1;
                holder.GetChild(index).GetComponent<TMP_InputField>().ActivateInputField();
                return;
            }

            //If moving left
            if (_input.x < 0)
            {
                if (controlHeld)
                {
                    index = 0;
                    holder.GetChild(index).GetComponent<TMP_InputField>().ActivateInputField();
                    return;
                }
                //And there is space to move left
                if (index > 0)
                {
                    TMP_InputField nextField = holder.GetChild(index - 1).GetComponent<TMP_InputField>();
                    //And the next field is interactable, set it active
                    if (nextField.interactable)
                    {
                        nextField.ActivateInputField();
                    }
                    //Or if the next field is not interactable and there is space, skip it
                    else if (index > 1)
                    {
                        holder.GetChild(index - 2).GetComponent<TMP_InputField>().ActivateInputField();
                    }
                }
                else
                {
                    inputField.DeactivateInputField();
                    inputField.ActivateInputField();
                }
            }
            //If moving right
            else if (_input.x > 0)
            {
                if (controlHeld)
                {
                    index = holder.childCount - 1;
                    holder.GetChild(index).GetComponent<TMP_InputField>().ActivateInputField();
                    return;
                }
                //And there is space to move right
                if (index + 1 < holder.childCount)
                {
                    TMP_InputField nextField = holder.GetChild(index + 1).GetComponent<TMP_InputField>();
                    //And the next field is interactable, set it active
                    if (nextField.interactable)
                    {
                        nextField.ActivateInputField();
                    }
                    //Or if the next field is not interactable and there is space, skip it
                    else if (index + 2 < holder.childCount)
                    {
                        holder.GetChild(index + 2).GetComponent<TMP_InputField>().ActivateInputField();
                    }
                }
                else
                {
                    inputField.DeactivateInputField();
                    inputField.ActivateInputField();
                }
            }
        }

        /// <summary>
        /// This method is called when an input field's value is changed, and is used to go to either
        /// the next input field or the previous one, depending if the field was cleared or filled.
        /// If the next field is space, it's not interactable and is skipped.
        /// </summary>
        public void GoNextField()
        {
            if (!inputField.IsActive() || disableGoNext) return;
            if (inputField.text == " ")
            {
                inputField.text = "";
                inputField.ActivateInputField();
                return;
            }

            //Go to previous valid input field if the current field was cleared
            if (inputField.text.Length == 0 && index > 0)
            {
                TMP_InputField nextField = holder.GetChild(index - 1).GetComponent<TMP_InputField>();
                //If next field is interactable, set it active
                if (nextField.interactable)
                {
                    nextField.ActivateInputField();
                }
                //Else, if there is enough space and the input field is not interactable, skip it
                else if (index > 1)
                {
                    holder.GetChild(index - 2).GetComponent<TMP_InputField>().ActivateInputField();
                }
            }
            //Go to next valid input field if the current field was filled
            else if (inputField.text.Length == 1 && index + 1 < holder.childCount)
            {
                TMP_InputField nextField = holder.GetChild(index + 1).GetComponent<TMP_InputField>();
                //If next field is interactable, set it active
                if (nextField.interactable)
                {
                    nextField.ActivateInputField();
                }
                //If next field was not interactable and there is space, skip and activate next field
                else if (index + 2 < holder.childCount)
                {
                    holder.GetChild(index + 2).GetComponent<TMP_InputField>().ActivateInputField();
                }
            }
            else
            {
                inputField.DeactivateInputField();
                inputField.ActivateInputField();
            }

            AudioManager.Instance.PlayInputSound();
        }
    }
}