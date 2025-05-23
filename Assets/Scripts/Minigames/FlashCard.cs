using System.Collections;
using SwedishApp.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SwedishApp.Minigames
{
    /// <summary>
    /// This class is the base for all visible flash cards. Used to handle the
    /// text fields and other "reactive" UI elements of the flash card.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class FlashCardBase : MonoBehaviour
    {
        //Simple state tracker for the card, used in "animations"
        public enum State
        {
            Finnish = 0,
            Flipping,
            Swedish
        }
        public State state { get; private set; }
        private Button thisButton;
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

        /// <summary>
        /// This method is attached to the card's button component. Used to call the flip "animation" coroutine.
        /// </summary>
        private void CallFlip()
        {
            if (state == State.Flipping) return;
            StartCoroutine(HandleFlip());
        }

        /// <summary>
        /// This method handles changing all the card's relevant elements to lightmode
        /// </summary>
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

        /// <summary>
        /// This method handles changing all the card's relevant elements to darkmode
        /// </summary>
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

        /// <summary>
        /// This method makes sure the finnish side of the card is visible when next card is
        /// placed on the screen.
        /// </summary>
        public void ResetToFinnishSide()
        {
            state = State.Finnish;
            cardFinnishSide.SetActive(true);
            cardSwedishSide.SetActive(false);
        }

        /// <summary>
        /// This method handles the flip "animation" of the card. In actuality the card's x-scale
        /// is lerped to 0, the contents are changed, and it's lerped back to 1.
        /// </summary>
        /// <returns></returns>
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
