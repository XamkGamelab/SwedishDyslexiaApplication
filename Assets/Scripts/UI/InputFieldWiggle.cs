using System.Collections;
using TMPro;
using UnityEngine;

namespace SwedishApp.UI
{
    public class InputFieldWiggle : MonoBehaviour
    {
        private RectTransform rect;
        [SerializeField] private float wiggleTimeOneWay = 0.06f;
        private WaitForSeconds waitTime;
        [SerializeField] private Vector3 wiggleMaxScale = new (1.1f, 1.1f, 1.1f);
        [SerializeField] private Vector3 originalScale = Vector3.one;
        private Coroutine wiggleCoroutine;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void OnEnable()
        {
            rect = GetComponent<RectTransform>();
            waitTime = new(wiggleTimeOneWay);

            GetComponent<TMP_InputField>().onValueChanged.AddListener(_ =>
            {
                if (wiggleCoroutine != null)
                {
                    StopCoroutine(wiggleCoroutine);
                    wiggleCoroutine = null;
                }
                wiggleCoroutine = StartCoroutine(DoWiggle());
            });
        }

        private IEnumerator DoWiggle()
        {
            LeanTween.scale(rect, wiggleMaxScale, wiggleTimeOneWay).setEaseInOutQuad();
            yield return waitTime;
            LeanTween.scale(rect, originalScale, wiggleTimeOneWay).setEaseInOutQuad();
            yield return waitTime;
        }
    }
}
