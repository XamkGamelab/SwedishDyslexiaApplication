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
        [SerializeField] private Image goUpImage;
        [SerializeField] private Image goDownImage;
        [SerializeField] private Sprite arrowSpriteDarkmode;
        [SerializeField] private Sprite arrowSpriteLightmode;

        [SerializeField]    //All the references for word category ui images
        private Image verbImage, nounImage, adjectiveImage, timeWordImage, numberWordImage, grammarImage, adverbImage, prepositionImage,
        questionImage, pronounImage, phraseImage;

        [SerializeField]    //All the dark- & lightmode sprites for word category icons
        private Sprite verbSpriteDarkmode, verbSpriteLightmode, nounSpriteDarkmode, nounSpriteLightmode, adjectiveSpriteDarkmode, adjectiveSpriteLightmode,
        timeWordSpriteDarkmode, timeWordSpriteLightmode, numberSpriteDarkmode, numberSpriteLightmode, grammarSpriteDarkmode, grammarSpriteLightmode,
        adverbSpriteDarkmode, adverbSpriteLightmode, prepositionSpriteDarkmode, prepositionSpriteLightmode, questionSpriteDarkmode, questionSpriteLightmode,
        pronounSpriteDarkmode, pronounSpriteLightmode, phraseSpriteDarkmode, phraseSpriteLightmode;

        [SerializeField] private int visibleButtonCount = 4;
        [SerializeField] private float offsetFix = 0.001f;
        private float buttonHeight;
        private float buttonProportion;

        private void Start()
        {
            StartCoroutine(DelayedCalculation());
            UIManager.Instance.LightmodeOnEvent += ToLightmode;
            UIManager.Instance.LightmodeOffEvent += ToDarkmode;
            if (UIManager.Instance.LightmodeOn) ToLightmode();
            else ToDarkmode();
        }

        private void OnEnable()
        {
            scrollbar.value = 1f;
            goUpButton.gameObject.SetActive(false);
            goDownButton.gameObject.SetActive(true);
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

        private void ToLightmode()
        {
            goDownImage.sprite = arrowSpriteLightmode;
            goUpImage.sprite = arrowSpriteLightmode;
            verbImage.sprite = verbSpriteLightmode;
            nounImage.sprite = nounSpriteLightmode;
            adjectiveImage.sprite = adjectiveSpriteLightmode;
            timeWordImage.sprite = timeWordSpriteLightmode;
            numberWordImage.sprite = numberSpriteLightmode;
            grammarImage.sprite = grammarSpriteLightmode;
            adverbImage.sprite = adverbSpriteLightmode;
            prepositionImage.sprite = prepositionSpriteLightmode;
            questionImage.sprite = questionSpriteLightmode;
            pronounImage.sprite = pronounSpriteLightmode;
            phraseImage.sprite = phraseSpriteLightmode;
        }

        private void ToDarkmode()
        {
            goDownImage.sprite = arrowSpriteDarkmode;
            goUpImage.sprite = arrowSpriteDarkmode;
            verbImage.sprite = verbSpriteDarkmode;
            nounImage.sprite = nounSpriteDarkmode;
            adjectiveImage.sprite = adjectiveSpriteDarkmode;
            timeWordImage.sprite = timeWordSpriteDarkmode;
            numberWordImage.sprite = numberSpriteDarkmode;
            grammarImage.sprite = grammarSpriteDarkmode;
            adverbImage.sprite = adverbSpriteDarkmode;
            prepositionImage.sprite = prepositionSpriteDarkmode;
            questionImage.sprite = questionSpriteDarkmode;
            pronounImage.sprite = pronounSpriteDarkmode;
            phraseImage.sprite = phraseSpriteDarkmode;
        }
    }
}