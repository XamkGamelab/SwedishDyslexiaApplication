using System.Collections;
using SwedishApp.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SwedishApp.Minigames
{
    public class InfoHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject infoTextObject;
        private Image infoImg;
        [SerializeField] private Image tipBG;
        [SerializeField] private TextMeshProUGUI tipTxt;
        [SerializeField] private Sprite darkmodeSprite;
        [SerializeField] private Sprite lightmodeSprite;
        [SerializeField] private float hoverDelayTime = 0.25f;
        private WaitForSeconds hoverWait;
        private Coroutine delayCoroutine;

        private void Start()
        {
            hoverWait = new(hoverDelayTime);
            infoImg = GetComponent<Image>();
            infoImg.sprite = UIManager.Instance.LightmodeOn ? lightmodeSprite : darkmodeSprite;
            UIManager.Instance.LightmodeOnEvent += () => infoImg.sprite = lightmodeSprite;
            UIManager.Instance.LightmodeOffEvent += () => infoImg.sprite = darkmodeSprite;
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
