using System.Collections;
using SwedishApp.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SwedishApp.Minigames
{
    [RequireComponent(typeof(Button))]
    public class FlashCardBase : MonoBehaviour
    {
        public enum State
        {
            Finnish = 0,
            Flipping,
            Swedish
        }
        private Button thisButton;
        public State state { get; private set; }
        private TextMeshProUGUI[] textsInChildren;
        [SerializeField] public GameObject cardFinnishSide;
        [SerializeField] private GameObject cardSwedishSide;
        [SerializeField] private Image hintImage;
        [SerializeField] private Sprite darkmodeImage;
        [SerializeField] private Sprite lightmodeImage;

        [SerializeField] private float flipTime = 0.3f;

        public TextMeshProUGUI wordFinnishText;
        public TextMeshProUGUI wordSwedishBaseText;

        private void Awake()
        {
            textsInChildren = transform.GetComponentsInChildren<TextMeshProUGUI>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            thisButton = GetComponent<Button>();
            thisButton.onClick.AddListener(CallFlip);
            state = State.Finnish;

            //like and subscribe
            UIManager.instance.LightmodeOnEvent += LightsOn;
            UIManager.instance.LightmodeOffEvent += LightsOff;
        }

        private void CallFlip()
        {
            if (state == State.Flipping) return;
            StartCoroutine(HandleFlip());
        }

        public void LightsOn()
        {
            foreach (TextMeshProUGUI text in textsInChildren)
            {
                text.color = UIManager.instance.Lightgrey;
            }
            if (lightmodeImage != null)
            {
                hintImage.sprite = lightmodeImage;
            }
        }

        public void LightsOff()
        {
            foreach (TextMeshProUGUI text in textsInChildren)
            {
                text.color = UIManager.instance.Darkgrey;
            }
            if (darkmodeImage != null)
            {
                hintImage.sprite = darkmodeImage;
            }
        }

        public void ResetToFinnishSide()
        {
            state = State.Finnish;
            cardFinnishSide.SetActive(true);
            cardSwedishSide.SetActive(false);
        }

        private IEnumerator HandleFlip()
        {
            if (state == State.Finnish)
            {
                state = State.Flipping;
                LeanTween.scaleX(gameObject, 0f, flipTime).setEaseInOutCubic();
                yield return new WaitForSeconds(flipTime);
                cardFinnishSide.SetActive(false);
                cardSwedishSide.SetActive(true);
                LeanTween.scaleX(gameObject, 1f, flipTime).setEaseInOutCubic();
                state = State.Swedish;
            }
            else if (state == State.Swedish)
            {
                state = State.Flipping;
                LeanTween.scaleX(gameObject, 0f, flipTime).setEaseInOutCubic();
                yield return new WaitForSeconds(flipTime);
                cardFinnishSide.SetActive(true);
                cardSwedishSide.SetActive(false);
                LeanTween.scaleX(gameObject, 1f, flipTime).setEaseInOutCubic();
                state = State.Finnish;
            }
        }
    }
}
