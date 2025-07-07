using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SwedishApp
{
    public class InfoHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject infoTextObject;
        [SerializeField] private float hoverDelayTime = 0.25f;
        private WaitForSeconds hoverWait;
        private Coroutine delayCoroutine;

        private void Start()
        {
            hoverWait = new(hoverDelayTime);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (delayCoroutine != null) StopCoroutine(delayCoroutine);
            delayCoroutine = StartCoroutine(HoverDelay(true));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (delayCoroutine != null) StopCoroutine(delayCoroutine);
            delayCoroutine = StartCoroutine(HoverDelay(false));
        }

        private IEnumerator HoverDelay(bool _on)
        {
            yield return hoverWait;
            infoTextObject.SetActive(_on);
            delayCoroutine = null;
        }
    }
}
