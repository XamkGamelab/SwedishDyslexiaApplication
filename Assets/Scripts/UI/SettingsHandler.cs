using UnityEngine;
using UnityEngine.EventSystems;

namespace SwedishApp.UI
{
    public class SettingsHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            UIManager.Instance.MouseOverSettings = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIManager.Instance.MouseOverSettings = false;
        }
    }
}
