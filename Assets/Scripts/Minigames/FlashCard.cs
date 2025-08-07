using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private List<TextMeshProUGUI> textsInChildren;
        [SerializeField] private GameObject cardFinnishSide;
        [SerializeField] private GameObject cardSwedishSide;
        [SerializeField] private Image hintImage;
        public Sprite darkmodeSprite { get; set; }
        public Sprite lightmodeSprite { get; set; }

        [SerializeField] private float flipTime = 0.3f;

        public TextMeshProUGUI wordFinnishText;
        public TextMeshProUGUI wordSwedishBaseText;

        private void Awake()
        {
            textsInChildren = transform.GetComponentsInChildren<TextMeshProUGUI>(true).ToList();
            //Add listener to every text field, called when a layout is changed. This then fixes character spacing for soft hyphens.
            textsInChildren.ForEach(field => field.RegisterDirtyLayoutCallback(() => UIManager.Instance.FixTextSpacing(field)));
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //get references
            thisButton = GetComponent<Button>();

            //set initial state
            state = State.Finnish;

            //like and subscribe
            thisButton.onClick.AddListener(CallFlip);
            UIManager.Instance.LightmodeOnEvent += LightsOn;
            UIManager.Instance.LightmodeOffEvent += LightsOff;
        }

        public void SetInitialElements(Sprite _lightmodeSprite = null, Sprite _darkmodeSprite = null)
        {
            lightmodeSprite = _lightmodeSprite;
            darkmodeSprite = _darkmodeSprite;
            if (lightmodeSprite != null && darkmodeSprite != null)
            {
                hintImage.enabled = true;
                hintImage.sprite = UIManager.Instance.LightmodeOn ? lightmodeSprite : darkmodeSprite;
            }
            else
                hintImage.enabled = false;
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
                text.color = UIManager.Instance.Darkgrey;
            }
            if (lightmodeSprite != null)
            {
                hintImage.sprite = lightmodeSprite;
            }
        }

        /// <summary>
        /// This method handles changing all the card's relevant elements to darkmode
        /// </summary>
        public void LightsOff()
        {
            foreach (TextMeshProUGUI text in textsInChildren)
            {
                text.color = UIManager.Instance.Lightgrey;
            }
            if (darkmodeSprite != null)
            {
                hintImage.sprite = darkmodeSprite;
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
