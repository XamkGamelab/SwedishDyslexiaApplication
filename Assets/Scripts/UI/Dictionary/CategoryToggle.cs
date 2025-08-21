using UnityEngine;
using UnityEngine.UI;

namespace SwedishApp.UI
{
    [RequireComponent(typeof(Button))]
    public class CategoryToggle : MonoBehaviour
    {
        [SerializeField] private GameObject categoryHolder;
        [SerializeField] private Button button;
        [SerializeField] private RectTransform imageRect;
        [SerializeField] private Image image;
        [SerializeField] private Sprite arrowSpriteDarkmode;
        [SerializeField] private Sprite arrowSpriteLightmode;
        [SerializeField] private float tweenTime = 0.1f;
        private int tweenId = -1;
        private bool categoryVisible = true;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            button.onClick.AddListener(ToggleCategory);
            UIManager.Instance.LightmodeOnEvent += ToLightmode;
            UIManager.Instance.LightmodeOffEvent += ToDarkmode;
            if (UIManager.Instance.LightmodeOn) ToLightmode();
            else ToDarkmode();
        }

        private void ToggleCategory()
        {
            categoryVisible = !categoryVisible;

            if (categoryVisible)
            {
                float openRotation = -180f;
                categoryHolder.SetActive(true);
                if (tweenId >= 0) LeanTween.cancel(imageRect.gameObject);

                tweenId = LeanTween.rotateZ(imageRect.gameObject, openRotation, tweenTime).setEaseInOutQuad().id;

                // tweenId = LeanTween.value(
                //     imageRect.gameObject,
                //     (newValue) => imageRect.rotation = Quaternion.Euler(0, 0, newValue),
                //     imageRect.rotation.z,
                //     openRotation,
                //     tweenTime).
                //     setEaseInOutQuad().id;
                Invoke(nameof(ResetTweenId), tweenTime);
            }
            else
            {
                float closedRotation = -90f;
                categoryHolder.SetActive(false);
                if (tweenId >= 0) LeanTween.cancel(imageRect.gameObject);

                tweenId = LeanTween.rotateZ(imageRect.gameObject, closedRotation, tweenTime).setEaseInOutQuad().id;

                // tweenId = LeanTween.value(
                //     imageRect.gameObject,
                //     (newValue) => imageRect.rotation = Quaternion.Euler(0, 0, newValue),
                //     imageRect.rotation.z,
                //     closedRotation,
                //     tweenTime).
                //     setEaseInOutQuad().id;
                Invoke(nameof(ResetTweenId), tweenTime);
            }
        }

        private void ResetTweenId()
        {
            tweenId = -1;
        }

        private void ToLightmode()
        {
            image.sprite = arrowSpriteLightmode;
        }

        private void ToDarkmode()
        {
            image.sprite = arrowSpriteDarkmode;
        }
    }
}
