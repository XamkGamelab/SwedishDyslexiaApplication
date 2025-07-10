using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace SwedishApp.UI
{
    /// <summary>
    /// This class fixes the issue where clicking an already active input field deselects the text, 
    /// which disables the ability to write anything in the input field until another input field is selected
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class InputFieldReselecter : MonoBehaviour, IPointerClickHandler
    {
        private TMP_InputField field;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            field = GetComponent<TMP_InputField>();
        }

        /// <summary>
        /// If the selected object is already active, reactivate it on click
        /// </summary>
        /// <param name="eventData"> Holds data related to the click event </param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.selectedObject != gameObject || !field.IsActive()) return;
            field.DeactivateInputField();
            field.ActivateInputField();
        }
    }
}
