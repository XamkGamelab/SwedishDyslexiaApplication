using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SwedishApp.UI
{
    public class ButtonHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] float colorChangeTime = 0.2f;
        private WaitForSeconds colorChangeWait;
        private Image image;
        int activeTween = -1;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            image = GetComponent<Image>();
            colorChangeWait = new(colorChangeTime);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (activeTween >= 0) LeanTween.cancel(gameObject);
            Color newColor = UIManager.Instance.LightmodeOn ? UIManager.Instance.LightmodeHighlight : UIManager.Instance.DarkmodeHighlight;
            activeTween = LeanTween.value(gameObject, (color) => image.color = color, image.color, newColor, colorChangeTime).id;
            StartCoroutine(TweenValueClear());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (activeTween >= 0) LeanTween.cancel(gameObject);
            Color newColor = UIManager.Instance.LightmodeOn ? UIManager.Instance.Darkgrey : UIManager.Instance.Lightgrey;
            activeTween = LeanTween.value(gameObject, (color) => image.color = color, image.color, newColor, colorChangeTime).id;
            StartCoroutine(TweenValueClear());
        }

        private IEnumerator TweenValueClear()
        {
            yield return colorChangeWait;
            activeTween = -1;
        }
    }
}
