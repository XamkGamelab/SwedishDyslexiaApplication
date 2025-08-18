using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SwedishApp.UI
{
    public class SelectionScreenUI : MonoBehaviour
    {
        [SerializeField] private Scrollbar scrollbar;
        [SerializeField] private RectTransform scrollerRect;
        [SerializeField] private Button goUpButton;
        [SerializeField] private Button goDownButton;
        [SerializeField] private int visibleButtonCount = 4;
        [SerializeField] private float offsetFix = 0.001f;
        private float buttonHeight;
        private float buttonProportion;

        private void Start()
        {
            StartCoroutine(DelayedCalculation());
        }

        private void OnEnable()
        {
            scrollbar.value = 1f;
            goUpButton.gameObject.SetActive(false);
            scrollbar.onValueChanged.AddListener(ScrollbarHandler);
        }

        private void OnDisable()
        {
            scrollbar.onValueChanged.RemoveListener(ScrollbarHandler);
        }

        private void ScrollbarHandler(float _value)
        {
            if (_value < 0.0001f)
            {
                goUpButton.gameObject.SetActive(true);
                goDownButton.gameObject.SetActive(false);
            }
            else if (_value > 0.9999f)
            {
                goUpButton.gameObject.SetActive(false);
                goDownButton.gameObject.SetActive(true);
            }
            else
            {
                goUpButton.gameObject.SetActive(true);
                goDownButton.gameObject.SetActive(true);
            }
        }

        private IEnumerator DelayedCalculation()
        {
            yield return null;

            buttonHeight = scrollerRect.rect.height / ((float)scrollerRect.childCount - visibleButtonCount);
            buttonProportion = buttonHeight / scrollerRect.rect.height;
            buttonProportion -= offsetFix;

            goUpButton.onClick.AddListener(() =>
            {
                scrollbar.value = Mathf.Clamp(scrollbar.value + buttonProportion, 0f, 1f);
            });
            goDownButton.onClick.AddListener(() =>
            {
                scrollbar.value = Mathf.Clamp(scrollbar.value - buttonProportion, 0f, 1f);
            });
        }
    }
}