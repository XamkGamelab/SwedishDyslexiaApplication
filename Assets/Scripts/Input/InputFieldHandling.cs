using TMPro;
using UnityEngine;

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

        private void Awake()
        {
            holder = GetComponent<RectTransform>();
            inputReader.NavigateEvent += Navigate;
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
            inputField = holder.GetChild(index).GetComponent<TMP_InputField>();
        }

        /// <summary>
        /// This method navigates between input fields based on the input events, taking into account
        /// the index limits.
        /// </summary>
        /// <param name="_input">This parameter is received from the input event which invokes
        /// this method</param>
        private void Navigate(Vector2 _input)
        {
            if (!inputField.IsActive()) return;

            if (_input.x < 0)
            {
                if (index > 0)
                    holder.GetChild(index - 1).GetComponent<TMP_InputField>().ActivateInputField();
            }
            else if (_input.x > 0)
            {
                if (index + 1 < holder.childCount)
                    holder.GetChild(index + 1).GetComponent<TMP_InputField>().ActivateInputField();
            }
        }

        /// <summary>
        /// This method is called when an input field's value is changed, and is used to go to either
        /// the next input field or the previous one, depending if the field was cleared or filled.
        /// </summary>
        public void GoNextField()
        {
            if (!inputField.IsActive()) return;

            if (inputField.text.Length == 0 && index > 0)
            {
                holder.GetChild(index - 1).GetComponent<TMP_InputField>().ActivateInputField();
                return;
            }

            if (inputField.text.Length == 1 && index + 1 < holder.childCount)
            {
                holder.GetChild(index + 1).GetComponent<TMP_InputField>().ActivateInputField();
            }
        }
    }
}