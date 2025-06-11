using UnityEngine;
using UnityEngine.EventSystems;

namespace SwedishApp.UI
{
    public class SettingsHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            UIManager.instance.mouseOverSettings = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIManager.instance.mouseOverSettings = false;
        }
    }
}
