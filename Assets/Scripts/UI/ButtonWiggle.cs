using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SwedishApp.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonWiggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] float wiggleDuration = 0.1f;
        private WaitForSeconds wiggleWait;
        [SerializeField] float wiggleIntensity = 1.05f;
        [SerializeField] float clickIntensityIncrease = 0.08f;
        private Vector3 originalScale;
        private RectTransform rectTransform;
        private Coroutine wiggleCoroutine;
        private Button button;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            button = GetComponent<Button>();
            rectTransform = GetComponent<RectTransform>();
            originalScale = rectTransform.localScale;
            wiggleWait = new(wiggleDuration);
        }

        void OnEnable()
        {
            if (button == null) button = GetComponent<Button>();
            if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
            if (originalScale == null || originalScale == Vector3.zero) originalScale = rectTransform.localScale;
            button.onClick.AddListener(ClickEffect);
        }

        void OnDisable()
        {
            if (wiggleCoroutine != null) StopCoroutine(wiggleCoroutine);
            button.onClick.RemoveListener(ClickEffect);
            rectTransform.localScale = originalScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            wiggleCoroutine = StartCoroutine(ButtonWiggler(wiggleIntensity));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopCoroutine(wiggleCoroutine);
            wiggleCoroutine = StartCoroutine(ButtonReturn());
        }

        private void ClickEffect()
        {
            StopCoroutine(wiggleCoroutine);
            if (gameObject.activeInHierarchy)
            wiggleCoroutine = StartCoroutine(ButtonWiggler(wiggleIntensity + clickIntensityIncrease, wiggleDuration));
        }

        private IEnumerator ButtonWiggler(float _multiplier, float? _returnDelay = null)
        {
            LeanTween.scale(rectTransform, originalScale * _multiplier, wiggleDuration).setEaseInOutQuad();
            yield return wiggleWait;
            if (_returnDelay == null) yield break;
            yield return new WaitForSeconds((float)_returnDelay);
            wiggleCoroutine = StartCoroutine(ButtonReturn());
        }

        private IEnumerator ButtonReturn()
        {
            LeanTween.scale(rectTransform, originalScale, wiggleDuration).setEaseInOutQuad();
            yield return wiggleWait;
        }
    }
}
