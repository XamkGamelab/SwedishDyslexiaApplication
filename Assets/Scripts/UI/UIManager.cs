using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SwedishApp.Input;
using SwedishApp.Minigames;
using SwedishApp.Words;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace SwedishApp.UI
{
    /// <summary>
    /// This class handles the majority of UI functionality in this project.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("TEMP")]
        public TextMeshProUGUI TEST_VERB;
        public VerbList verbList;
        public NounList nounList;

        [Header("Input Related")]
        [SerializeField] private InputReader inputReader;
        //Singleton
        public static UIManager instance { get; private set; }

        [Header("Lightmode Related")]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private List<TextMeshProUGUI> textObjectList;
        [SerializeField] private List<Image> lightmodableImages;
        private bool lightmodeHelper = false;
        public bool LightmodeOn { get; private set; } = false;
        public event Action LightmodeOnEvent;
        public event Action LightmodeOffEvent;
        public readonly Color32 Lightgrey = new(235, 235, 235, 255);
        public readonly Color32 Darkgrey = new(23, 26, 33, 255);
        public readonly Color32 LightmodeHighlight = new(1, 111, 185, 255);
        public readonly Color32 DarkmodeHighlight = new(239, 160, 11, 255);

        [Header("Font Related")]
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
        public event Action FontLegibleEvent;
        public event Action FontBasicEvent;

        enum FontSize
        {
            small = 1,
            medium,
            large
        }

        [Header("Settings Related")]
        private bool settingsOpen = false;
        [SerializeField] private Button toggleSettingsBtn;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private Button toggleLightmodeBtn;
        [SerializeField] private Slider toggledSlider;
        [SerializeField] private float lerpDuration = 0.06f;

        [Header("Minigame Related")]
        [SerializeField] private Button startTranslateMinigameToFinnish;
        // [SerializeField] private Button startTranslateMinigameToSwedish;
        [SerializeField] private TranslateMinigame translateMinigame;

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
            fontSmallToggle.onValueChanged.AddListener((toggleOn) => PickFontSize(FontSize.small, toggleOn));
            fontMediumToggle.onValueChanged.AddListener((toggleOn) => PickFontSize(FontSize.medium, toggleOn));
            fontLargeToggle.onValueChanged.AddListener((toggleOn) => PickFontSize(FontSize.large, toggleOn));
            toggleHyperlegible.onValueChanged.AddListener((toggleOn) => ToggleHyperlegibleFont(toggleOn));

            toggleLightmodeBtn.onClick.AddListener(ToggleLightmode);
            toggleSettingsBtn.onClick.AddListener(ToggleSettingsMenu);
            LightmodeOnEvent += TestVerbOutput;
            LightmodeOffEvent += TestVerbOutput;

            startTranslateMinigameToFinnish.onClick.AddListener(() =>
                translateMinigame.StartGame(TranslateMinigame.GameMode.ToFinnish, new List<Word>(nounList.nounList)));
            // startTranslateMinigameToSwedish.onClick.AddListener(() =>
            //     translateMinigame.StartGame(TranslateMinigame.GameMode.ToSwedish, new List<Word>(nounList.nounList)));

            TestVerbOutput();

            inputReader.EnableInputs();
        }

        private void OnApplicationQuit()
        {
            inputReader.DisableInputs();
        }

        #endregion

        #region lightmode related methods

        /// <summary>
        /// Toggles the light mode off or on and updates the toggle UI accordingly
        /// </summary>
        private void ToggleLightmode()
        {
            if (LightmodeOn)
            {
                LightmodeOn = false;
                textObjectList.ForEach((textObject) => textObject.color = Lightgrey);
                lightmodableImages.ForEach((textObject) => textObject.color = Darkgrey);
                LightmodeOnEvent?.Invoke();
            }
            else
            {
                LightmodeOn = true;
                textObjectList.ForEach((textObject) => textObject.color = Darkgrey);
                lightmodableImages.ForEach((textObject) => textObject.color = Lightgrey);
                LightmodeOffEvent?.Invoke();
            }
            StartCoroutine(SliderLerp());
        }

        #endregion

        #region settings related methods

        private void ToggleHyperlegibleFont(bool _toggledOn)
        {
            if (_toggledOn)
            {
                textObjectList.ForEach((textObject) => 
                {
                    textObject.font = legibleFont;
                    textObject.characterSpacing = legibleSpacing;
                });
                LegibleModeOnEvent?.Invoke();
            }
            else
            {
                textObjectList.ForEach((textObject) => 
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

        private void TestVerbOutput()
        {
            TEST_VERB.text = nounList.nounList[1].PluralKnownNoun();
        }
    }
}
