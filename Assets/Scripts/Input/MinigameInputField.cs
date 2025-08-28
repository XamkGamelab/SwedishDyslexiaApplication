using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SwedishApp.Input
{
    public class MinigameInputField : TMP_InputField
    {
        public override void OnSelect(BaseEventData eventData)
        {
            shouldHideSoftKeyboard = false;
            var activeKeyboard = m_SoftKeyboard;
            m_SoftKeyboard = null;
            base.OnSelect(eventData);
            m_SoftKeyboard = activeKeyboard;
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            var activeKeyboard = m_SoftKeyboard;
            m_SoftKeyboard = null;
            base.OnSubmit(eventData);
            m_SoftKeyboard = activeKeyboard;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            var activeKeyboard = m_SoftKeyboard;
            m_SoftKeyboard = null;
            base.OnDeselect(eventData);
            m_SoftKeyboard = activeKeyboard;
        }
    }
}