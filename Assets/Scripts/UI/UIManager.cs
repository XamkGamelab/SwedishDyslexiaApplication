using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SwedishApp.Core;
using SwedishApp.Input;
using SwedishApp.Minigames;
using SwedishApp.Words;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SwedishApp.UI
{
    /// <summary>
    /// This class handles the majority of UI functionality in this project.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        //Singleton
        public static UIManager instance { get; private set; }

        [Header("Word Lists")]
        public VerbList verbList;
        public NounList nounList;
        public AdjectiveList adjectiveList;
       

        [Header("Input-Related")]
        [SerializeField] private InputReader inputReader;
        private Vector2 mousePos;

        [Header("Minigame-Related")]
        [SerializeField] private Button startTranslationGameToFinnishBtn;
        [SerializeField] private Button startTranslationGameToSwedishBtn;
        [SerializeField] private Button startConjugationGameBtn;
        [SerializeField] private TranslateMinigame translateMinigame;
        [SerializeField] private ConjugationMinigame conjugationMinigame;
        [SerializeField] private GameObject flashcardGameTypeMenu;
        [SerializeField] private Button openFlashcardMenuBtn;
        [SerializeField] private Button closeFlashcardMenuBtn;
        [SerializeField] private Button startFlashcardNounGameBtn;
        [SerializeField] private Button startFlashcardVerbGameBtn;
        [SerializeField] private Button startFlashcardAdjectiveGameBtn;
        [SerializeField] private FlashCardMinigame flashCardMinigame;

        [Header("Lightmode-Related")]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image cardtypeBackground;
        [SerializeField] private List<TextMeshProUGUI> textObjectList;
        [SerializeField] private List<Image> lightmodableImages;
        [SerializeField] private List<TextMeshProUGUI> textObjectListReverseLight;
        [SerializeField] private List<Image> lightmodableImagesReverse;
        [SerializeField] private List<Image> highlightImages;
        [SerializeField] private List<Image> highlightImagesReverse;
        [SerializeField] private List<Image> buttonImages;
        [SerializeField] private List<Image> flashcardBases;
        [field: SerializeField] public Sprite abortSpriteDarkmode { get; private set; }
        [field: SerializeField] public Sprite abortSpriteLightmode { get; private set; }
        [field: SerializeField] public Sprite buttonSpriteDarkmode { get; private set; }
        [field: SerializeField] public Sprite buttonSpriteLightmode { get; private set; }
        [field: SerializeField] public Sprite cardSpriteDarkmode { get; private set; }
        [field: SerializeField] public Sprite cardSpriteLightmode { get; private set; }
        private bool lightmodeHelper = false;
        public bool LightmodeOn { get; private set; } = false;
        public event Action LightmodeOnEvent;
        public event Action LightmodeOffEvent;
        public readonly Color32 Lightgrey = new(235, 235, 235, 255);
        public readonly Color32 LightgreyDarker = new(200, 200, 200, 255);
        public readonly Color32 LightgreyHalfAlpha = new(235, 235, 235, 127);
        public readonly Color32 LightgreyMostAlpha = new(235, 235, 235, 210);
        public readonly Color32 Darkgrey = new(23, 26, 33, 255);
        public readonly Color32 DarkgreyLighter = new(46, 52, 66, 255);
        public readonly Color32 DarkgreyHalfAlpha = new(23, 26, 33, 127);
        public readonly Color32 DarkgreyMostAlpha = new(23, 26, 33, 248);
        public readonly Color32 LightmodeHighlight = new(1, 111, 185, 255);
        public readonly Color32 DarkmodeHighlight = new(239, 160, 11, 255);

        [Header("Font-Related")]
        private List<TextMeshProUGUI> textFields;
        public bool hyperlegibleOn { get; private set; } = true;
        [SerializeField] private Toggle toggleHyperlegible;
        [SerializeField] private Toggle fontSmallToggle;
        [SerializeField] private Toggle fontMediumToggle;
        [SerializeField] private Toggle fontLargeToggle;
        [field: SerializeField] public TMP_FontAsset legibleFont { get; private set; }
        [field: SerializeField] public TMP_FontAsset basicFont { get; private set; }
        public int legibleSpacing { get; private set; } = 8;
        public int basicSpacing { get; private set; } = 0;
        // private bool fontSettingsSubscribed = false;
        public event Action LegibleModeOnEvent;
        public event Action LegibleModeOffEvent;
        public event Action FontSmallEvent;
        public event Action FontMediumEvent;
        public event Action FontLargeEvent;

        [Header("Credits-Related")]
        [SerializeField] private GameObject creditsScreen;
        [SerializeField] private Button openCreditsButton;
        [SerializeField] private TextMeshProUGUI openCreditsText;
        [SerializeField] private Button closeCreditsButton;

        [Header("TEMPORARY")]
        public TextMeshProUGUI TEST_VERB;

        enum FontSize
        {
            small = 1,
            medium,
            large
        }

        [Header("Settings-Related")]
        private bool settingsOpen = false;
        [SerializeField] private Button toggleSettingsBtn;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private Image settingsMenuImage;
        private RectTransform settingsRect;
        [SerializeField] private Button toggleLightmodeBtn;
        [SerializeField] private Slider toggledSlider;
        [SerializeField] private float lerpDuration = 0.06f;

        #region unity default methods

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);  //Destroy is called at end of frame don't worry
                Debug.LogError($"Found more than one UIManager, destroying duplicate. Fix this!");
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            textFields = textObjectList;
            textFields.AddRange(textObjectListReverseLight);

            settingsRect = settingsMenu.GetComponent<RectTransform>();

            //Add input events
            inputReader.ClickEvent += ClickOffCloseSettings;
            inputReader.PointEvent += GetMousePosition;

            //Add listeners to settings-related buttons
            fontSmallToggle.onValueChanged.AddListener((toggleOn) => PickFontSize(FontSize.small, toggleOn));
            fontMediumToggle.onValueChanged.AddListener((toggleOn) => PickFontSize(FontSize.medium, toggleOn));
            fontLargeToggle.onValueChanged.AddListener((toggleOn) => PickFontSize(FontSize.large, toggleOn));
            toggleHyperlegible.onValueChanged.AddListener((toggleOn) => ToggleHyperlegibleFont(toggleOn));
            toggleLightmodeBtn.onClick.AddListener(ToggleLightmode);
            toggleSettingsBtn.onClick.AddListener(ToggleSettingsMenu);

            //Add listeners to credits buttons
            openCreditsButton.onClick.AddListener(() => creditsScreen.SetActive(true));
            closeCreditsButton.onClick.AddListener(() => creditsScreen.SetActive(false));

            //Add listeners to translate minigame buttons
            startTranslationGameToFinnishBtn.onClick.AddListener(() =>
                translateMinigame.StartGame(TranslateMinigame.GameMode.ToFinnish, new List<Word>(nounList.nounList)));
            startTranslationGameToSwedishBtn.onClick.AddListener(() =>
                translateMinigame.StartGame(TranslateMinigame.GameMode.ToSwedish, new List<Word>(nounList.nounList)));
            startConjugationGameBtn.onClick.AddListener(() => conjugationMinigame.InitializeGame(verbList.verbList));

            //Add listeners to flashcard minigame buttons
            openFlashcardMenuBtn.onClick.AddListener(() => flashcardGameTypeMenu.SetActive(true));
            closeFlashcardMenuBtn.onClick.AddListener(() => flashcardGameTypeMenu.SetActive(false));
            startFlashcardNounGameBtn.onClick.AddListener(() =>
            {
                flashCardMinigame.StartNounGame(nounList.nounList.ToArray());
                flashcardGameTypeMenu.SetActive(false);
            });
            startFlashcardVerbGameBtn.onClick.AddListener(() =>
            {
                flashCardMinigame.StartVerbGame(verbList.verbList.ToArray());
                flashcardGameTypeMenu.SetActive(false);
            });
            startFlashcardAdjectiveGameBtn.onClick.AddListener(() =>
            {
                flashCardMinigame.StartAdjectiveGame(adjectiveList.adjectiveList.ToArray());
                flashcardGameTypeMenu.SetActive(false);
            });

            inputReader.EnableInputs();
        }

        /// <summary>
        /// Disable inputs on application quit or Unity yells at me
        /// </summary>
        private void OnApplicationQuit()
        {
            inputReader.DisableInputs();
        }

        #endregion

        #region minigame-related methods



        #endregion

        #region lightmode-related methods

        /// <summary>
        /// Toggles the light mode on or off and updates the toggle UI accordingly. This also invokes an
        /// event that can be used elsewhere to handle changes related to light mode changes.
        /// </summary>
        private void ToggleLightmode()
        {
            //Lightmode goes ON here
            if (!LightmodeOn)
            {
                LightmodeOn = true;
                AudioManager.Instance.PlayLightModeToggle();
                textObjectList.ForEach((textObject) => textObject.color = Darkgrey);
                lightmodableImages.ForEach((imgObject) => imgObject.color = Lightgrey);
                textObjectListReverseLight.ForEach((textObject) => textObject.color = Lightgrey);
                lightmodableImagesReverse.ForEach((imgObject) => imgObject.color = Darkgrey);
                highlightImages.ForEach((imgObject) => imgObject.color = LightmodeHighlight);
                highlightImagesReverse.ForEach((imgObject) => imgObject.color = DarkmodeHighlight);
                buttonImages.ForEach((buttonImg) => buttonImg.sprite = buttonSpriteLightmode);
                flashcardBases.ForEach((baseImg) => baseImg.sprite = cardSpriteLightmode);

                cardtypeBackground.color = LightgreyMostAlpha;
                closeFlashcardMenuBtn.image.sprite = abortSpriteLightmode;
                openCreditsText.color = Darkgrey;
                settingsMenuImage.sprite = buttonSpriteLightmode;

                LightmodeOnEvent?.Invoke();
            }
            else //Darkmode goes ON here
            {
                LightmodeOn = false;
                AudioManager.Instance.PlayLightModeToggle();
                textObjectList.ForEach((textObject) => textObject.color = Lightgrey);
                lightmodableImages.ForEach((imgObject) => imgObject.color = Darkgrey);
                textObjectListReverseLight.ForEach((textObject) => textObject.color = Darkgrey);
                lightmodableImagesReverse.ForEach((imgObject) => imgObject.color = Lightgrey);
                highlightImages.ForEach((imgObject) => imgObject.color = DarkmodeHighlight);
                highlightImagesReverse.ForEach((imgObject) => imgObject.color = LightmodeHighlight);
                buttonImages.ForEach((buttonImg) => buttonImg.sprite = buttonSpriteDarkmode);
                flashcardBases.ForEach((baseImg) => baseImg.sprite = cardSpriteDarkmode);

                cardtypeBackground.color = DarkgreyMostAlpha;
                closeFlashcardMenuBtn.image.sprite = abortSpriteDarkmode;
                openCreditsText.color = Lightgrey;
                settingsMenuImage.sprite = buttonSpriteDarkmode;

                LightmodeOffEvent?.Invoke();
            }
            StartCoroutine(SliderLerp());
        }

        #endregion

        #region word list-related methods

        /// <summary>
        /// This method can be used to randomize the contents of the word lists
        /// </summary>
        private void ScrambleWordLists()
        {
            System.Random rng = new();
            verbList.verbList = verbList.verbList.OrderBy(a => rng.Next()).ToList();
            nounList.nounList = nounList.nounList.OrderBy(a => rng.Next()).ToList();
            adjectiveList.adjectiveList = adjectiveList.adjectiveList.OrderBy(a => rng.Next()).ToList();
        }

        #endregion

        #region settings-related methods

        private void ToggleHyperlegibleFont(bool _toggledOn)
        {
            if (_toggledOn)
            {
                hyperlegibleOn = true;
                textFields.ForEach((textObject) =>
                {
                    textObject.font = legibleFont;
                    textObject.characterSpacing = legibleSpacing;
                });
                LegibleModeOnEvent?.Invoke();
            }
            else
            {
                hyperlegibleOn = false;
                textFields.ForEach((textObject) =>
                {
                    textObject.font = basicFont;
                    textObject.characterSpacing = basicSpacing;
                });
                LegibleModeOffEvent?.Invoke();
            }
        }

        /// <summary>
        /// Changes the font size
        /// </summary>
        /// <param name="_size"> Changes font size (small, medium or large) </param>
        /// <param name="_toggledOn"> If _toggledOn = true, check the font size </param>
        private void PickFontSize(FontSize _size, bool _toggledOn)   //font size is an enum, 1=small, 2=medium, 3=large
        {
            if (!_toggledOn) return; // Doesn't go through the case options

            switch (_size)           // _toggledOn = true
            {
                case FontSize.small:
                    fontSmallToggle.interactable = false;
                    fontMediumToggle.interactable = true;
                    fontLargeToggle.interactable = true;
                    fontMediumToggle.isOn = false;
                    fontLargeToggle.isOn = false;
                    FontSmallEvent?.Invoke();
                    break;
                case FontSize.medium:
                    fontSmallToggle.interactable = true;
                    fontMediumToggle.interactable = false;
                    fontLargeToggle.interactable = true;
                    fontSmallToggle.isOn = false;
                    fontLargeToggle.isOn = false;
                    FontMediumEvent?.Invoke();
                    break;
                case FontSize.large:
                    fontSmallToggle.interactable = true;
                    fontMediumToggle.interactable = true;
                    fontLargeToggle.interactable = false;
                    fontSmallToggle.isOn = false;
                    fontMediumToggle.isOn = false;
                    FontLargeEvent?.Invoke();
                    break;
            }
        }

        /// <summary>
        /// Toggles settings menu on or off
        /// </summary>
        private void ToggleSettingsMenu()
        {
            if (settingsOpen)
            {
                settingsMenu.SetActive(false);
                settingsOpen = false;
            }
            else
            {
                settingsMenu.SetActive(true);
                settingsOpen = true;
            }
        }

        /// <summary>
        /// This method fetches the mouse position based on the input reader
        /// </summary>
        /// <param name="_pos">Vector2 indicating the position of the mouse</param>
        private void GetMousePosition(Vector2 _pos)
        {
            mousePos = _pos;
        }

        /// <summary>
        /// When the player clicks outside of the settings window while it's active, it gets deactivated.
        /// </summary>
        private void ClickOffCloseSettings()
        {
            if (!settingsOpen) return;
            if (!RectTransformUtility.RectangleContainsScreenPoint(settingsRect, mousePos))
            {
                settingsMenu.SetActive(false);
                settingsOpen = false;
            }
        }

        /// <summary>
        /// Makes the dark mode toggle slide to the opposite side
        /// </summary>
        /// <returns> Yields null to advance to next frame </returns>
        private IEnumerator SliderLerp()
        {
            toggleLightmodeBtn.interactable = false;
            float timer = 0f;

            if (lightmodeHelper)
            {
                while (timer < lerpDuration)
                {
                    timer += Time.deltaTime;
                    toggledSlider.value = Mathf.Lerp(1f, 0f, timer / lerpDuration);
                    lightmodeHelper = false;
                    yield return null;
                }
            }
            else
            {
                while (timer < lerpDuration)
                {
                    timer += Time.deltaTime;
                    toggledSlider.value = Mathf.Lerp(0f, 1f, timer / lerpDuration);
                    lightmodeHelper = true;
                    yield return null;
                }
            }

            toggleLightmodeBtn.interactable = true;
        }

        #endregion
    }
}