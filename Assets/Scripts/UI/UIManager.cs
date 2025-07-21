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

        #region variables

        [Header("Word Lists")]
        public VerbList verbList;
        public NounList nounList;
        public AdjectiveList adjectiveList;
        public TimeList timeList;
        public NumberList numberList;
        public GrammarList grammarList;
        public PronounList pronounList;
        public PhraseList phraseList;
       
        [Header("Input-Related")]
        [SerializeField] private InputReader inputReader;
        private Vector2 mousePos;

        [Header("Minigame-Related")]
        [SerializeField] private DictionaryHandler dictionaryHandler;
        [SerializeField] private Button openDictionaryButton;
        [SerializeField] private Button startTranslationGameToFinnishBtn;
        [SerializeField] private Button startTranslationGameToSwedishBtn;
        [SerializeField] private Button startConjugationGameBtn;
        [SerializeField] private Button startDeclensionGameBtn;
        [SerializeField] private TranslateMinigame translateMinigame;
        [SerializeField] private GameObject translateGameTypeMenu;
        [SerializeField] private Button closeTranslateMenuBtn;
        [SerializeField] private Button startTranslateNounGameBtn;
        [SerializeField] private Button startTranslateVerbGameBtn;
        [SerializeField] private Button startTranslateAdjectiveGameBtn;
        [SerializeField] private Button startTranslateTimeGameBtn;
        [SerializeField] private Button startTranslateNumberGameBtn;
        [SerializeField] private Button startTranslateGrammarGameBtn;
        [SerializeField] private Button startTranslatePronounGameBtn;
        [SerializeField] private Button startTranslatePhraseGameBtn;
        [SerializeField] private ConjugationMinigame conjugationMinigame;
        [SerializeField] private DeclensionMinigame declensionMinigame;
        [SerializeField] private GameObject flashcardGameTypeMenu;
        [SerializeField] private Button openFlashcardMenuBtn;
        [SerializeField] private Button closeFlashcardMenuBtn;
        [SerializeField] private Button startFlashcardNounGameBtn;
        [SerializeField] private Button startFlashcardVerbGameBtn;
        [SerializeField] private Button startFlashcardAdjectiveGameBtn;
        [SerializeField] private Button startFlashcardTimeGameBtn;
        [SerializeField] private Button startFlashcardNumberGameBtn;
        [SerializeField] private Button startFlashcardGrammarGameBtn;
        [SerializeField] private Button startFlashcardPronounGameBtn;
        [SerializeField] private Button startFlashcardPhraseGameBtn;
        [SerializeField] private FlashCardMinigame flashCardMinigame;

        [Header("Lightmode-Related")]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image flashcardGameTypeBackground;
        [SerializeField] private Image translateGameTypeBackground;
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
        private const string spaceTagStart = "<cspace=-0.08em>";
        private const string spaceTagEnd = "</cspace>";
        private const string spaceTagsWithSoftHyphen = "<cspace=-0.08em>\u00AD</cspace>";
        private const string colorEndTag = "</color>";
        private const string spaceEndWithColorEnd = "</cspace></color>";

        [Header("Credits-Related")]
        [SerializeField] private GameObject creditsScreen;
        [SerializeField] private Button openCreditsButton;
        [SerializeField] private TextMeshProUGUI openCreditsText;
        [SerializeField] private Button closeCreditsButton;

        [Header("Effects-related")]
        [SerializeField] private ParticleSystem yellowLeftSparkleFX;
        [SerializeField] private ParticleSystem yellowRightSparkleFX;
        [SerializeField] private ParticleSystem blueLeftSparkleFX;
        [SerializeField] private ParticleSystem blueRightSparkleFX;
        [SerializeField] private ParticleSystem yellowLameLeftSparkleFX;
        [SerializeField] private ParticleSystem yellowLameRightSparkleFX;
        [SerializeField] private ParticleSystem blueLameLeftSparkleFX;
        [SerializeField] private ParticleSystem blueLameRightSparkleFX;

        enum FontSize
        {
            small = 1,
            medium,
            large
        }

        [Header("Settings-Related")]
        private bool settingsOpen = false;
        public bool mouseOverSettings { private get; set; } = false;
        [SerializeField] private Button toggleSettingsBtn;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private Image settingsMenuImage;
        [SerializeField] private Button toggleLightmodeBtn;
        [SerializeField] private Slider toggledSlider;
        [SerializeField] private float lerpDuration = 0.06f;

        #endregion

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

            dictionaryHandler.InitializeDictionary();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            textObjectList.AddRange(dictionaryHandler.GetDictionaryTextFields());
            textFields = textObjectList;
            textFields.AddRange(textObjectListReverseLight);
            lightmodableImagesReverse.AddRange(dictionaryHandler.GetSpacerImages());

            //Add listener to every text field, called when a layout is changed. This then fixes character spacing for soft hyphens.
            textFields.ForEach(field => field.RegisterDirtyLayoutCallback(() => FixTextSpacing(field)));

            //Add input events
            inputReader.ClickEvent += ClickOffCloseSettings;
            inputReader.PointEvent += GetMousePosition;

            openDictionaryButton.onClick.AddListener(() => dictionaryHandler.gameObject.SetActive(true));

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

            //Add listeners to conjugation / declension minigame buttons
            startConjugationGameBtn.onClick.AddListener(() => conjugationMinigame.InitializeGame(verbList.verbList));
            startDeclensionGameBtn.onClick.AddListener(() => declensionMinigame.InitializeGame(nounList.nounList));

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
            startFlashcardTimeGameBtn.onClick.AddListener(() =>
            {
                flashCardMinigame.StartTimeWordGame(timeList.timeList.ToArray());
                flashcardGameTypeMenu.SetActive(false);
            });
            startFlashcardNumberGameBtn.onClick.AddListener(() =>
            {
                flashCardMinigame.StartNumberGame(numberList.numberList.ToArray());
                flashcardGameTypeMenu.SetActive(false);
            });
            startFlashcardGrammarGameBtn.onClick.AddListener(() =>
            {
                flashCardMinigame.StartGrammarGame(grammarList.grammarList.ToArray());
                flashcardGameTypeMenu.SetActive(false);
            });
            startFlashcardPronounGameBtn.onClick.AddListener(() =>
            {
                flashCardMinigame.StartPronounGame(pronounList.pronounList.ToArray());
                flashcardGameTypeMenu.SetActive(false);
            });
            startFlashcardPhraseGameBtn.onClick.AddListener(() =>
            {
                flashCardMinigame.StartPhraseGame(phraseList.phraseList.ToArray());
                flashcardGameTypeMenu.SetActive(false);
            });

            //Add listeners to translate minigame buttons
            startTranslationGameToFinnishBtn.onClick.AddListener(() =>
            {
                translateGameTypeMenu.SetActive(true);
                startTranslateNounGameBtn.onClick.AddListener(() => StartNounTranslateGame(_toFinnish: true));
                startTranslateVerbGameBtn.onClick.AddListener(() => StartVerbTranslateGame(_toFinnish: true));
                startTranslateAdjectiveGameBtn.onClick.AddListener(() => StartAdjectiveTranslateGame(_toFinnish: true));
                startTranslateTimeGameBtn.onClick.AddListener(() => StartTimeTranslateGame(_toFinnish: true));
                startTranslateNumberGameBtn.onClick.AddListener(() => StartNumberTranslateGame(_toFinnish: true));
                startTranslateGrammarGameBtn.onClick.AddListener(() => StartGrammarTranslateGame(_toFinnish: true));
                startTranslatePronounGameBtn.onClick.AddListener(() => StartPronounTranslateGame(_toFinnish: true));
                startTranslatePhraseGameBtn.onClick.AddListener(() => StartPhraseTranslateGame(_toFinnish: true));
            });
            startTranslationGameToSwedishBtn.onClick.AddListener(() =>
            {
                translateGameTypeMenu.SetActive(true);
                startTranslateNounGameBtn.onClick.AddListener(() => StartNounTranslateGame(_toFinnish: false));
                startTranslateVerbGameBtn.onClick.AddListener(() => StartVerbTranslateGame(_toFinnish: false));
                startTranslateAdjectiveGameBtn.onClick.AddListener(() => StartAdjectiveTranslateGame(_toFinnish: false));
                startTranslateTimeGameBtn.onClick.AddListener(() => StartTimeTranslateGame(_toFinnish: false));
                startTranslateNumberGameBtn.onClick.AddListener(() => StartNumberTranslateGame(_toFinnish: false));
                startTranslateGrammarGameBtn.onClick.AddListener(() => StartGrammarTranslateGame(_toFinnish: false));
                startTranslatePronounGameBtn.onClick.AddListener(() => StartPronounTranslateGame(_toFinnish: false));
                startTranslatePhraseGameBtn.onClick.AddListener(() => StartPhraseTranslateGame(_toFinnish: false));
            });
            closeTranslateMenuBtn.onClick.AddListener(() =>
            {
                translateGameTypeMenu.SetActive(false);
                UnsubscribeTranslateStartButtons();
            });
            
            //Subscribe to word correct events
            conjugationMinigame.WordCorrectEvent += PlayYellowSparkles;
            conjugationMinigame.WordCorrectEvent += PlayBlueSparkles;
            declensionMinigame.WordCorrectEvent += PlayYellowSparkles;
            declensionMinigame.WordCorrectEvent += PlayBlueSparkles;
            translateMinigame.WordCorrectEvent += PlayYellowSparkles;
            translateMinigame.WordCorrectEvent += PlayBlueSparkles;
            conjugationMinigame.WordIncorrectEvent += PlayLameYellowSparkles;
            conjugationMinigame.WordIncorrectEvent += PlayLameBlueSparkles;
            declensionMinigame.WordIncorrectEvent += PlayLameYellowSparkles;
            declensionMinigame.WordIncorrectEvent += PlayLameBlueSparkles;
            translateMinigame.WordIncorrectEvent += PlayLameYellowSparkles;
            translateMinigame.WordIncorrectEvent += PlayLameBlueSparkles;

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

        #region effects-related methods

        private void PlayYellowSparkles()
        {
            if (LightmodeOn) return;
            yellowLeftSparkleFX.Play();
            yellowRightSparkleFX.Play();
        }

        private void PlayBlueSparkles()
        {
            if (!LightmodeOn) return;
            blueLeftSparkleFX.Play();
            blueRightSparkleFX.Play();
        }

        private void PlayLameYellowSparkles()
        {
            if (LightmodeOn) return;
            yellowLameLeftSparkleFX.Play();
            yellowLameRightSparkleFX.Play();
        }

        private void PlayLameBlueSparkles()
        {
            if (!LightmodeOn) return;
            blueLameLeftSparkleFX.Play();
            blueLameRightSparkleFX.Play();
        }

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

                flashcardGameTypeBackground.color = LightgreyMostAlpha;
                translateGameTypeBackground.color = LightgreyMostAlpha;
                closeFlashcardMenuBtn.image.sprite = abortSpriteLightmode;
                closeTranslateMenuBtn.image.sprite = abortSpriteLightmode;
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

                flashcardGameTypeBackground.color = DarkgreyMostAlpha;
                translateGameTypeBackground.color = DarkgreyMostAlpha;
                closeFlashcardMenuBtn.image.sprite = abortSpriteDarkmode;
                closeTranslateMenuBtn.image.sprite = abortSpriteDarkmode;
                openCreditsText.color = Lightgrey;
                settingsMenuImage.sprite = buttonSpriteDarkmode;

                LightmodeOffEvent?.Invoke();
            }
            StartCoroutine(SliderLerp());
        }

        #endregion

        #region minigame-related methods

        private void StartNounTranslateGame(bool _toFinnish)
        {
            TranslateMinigame.GameMode mode = _toFinnish ? TranslateMinigame.GameMode.ToFinnish : TranslateMinigame.GameMode.ToSwedish;
            translateMinigame.StartGame(mode, ScrambleWordList(new List<Word>(nounList.nounList)));
            translateGameTypeMenu.SetActive(false);
            UnsubscribeTranslateStartButtons();
        }

        private void StartVerbTranslateGame(bool _toFinnish)
        {
            TranslateMinigame.GameMode mode = _toFinnish ? TranslateMinigame.GameMode.ToFinnish : TranslateMinigame.GameMode.ToSwedish;
            translateMinigame.StartGame(mode, ScrambleWordList(new List<Word>(verbList.verbList)));
            translateGameTypeMenu.SetActive(false);
            UnsubscribeTranslateStartButtons();
        }

        private void StartAdjectiveTranslateGame(bool _toFinnish)
        {
            TranslateMinigame.GameMode mode = _toFinnish ? TranslateMinigame.GameMode.ToFinnish : TranslateMinigame.GameMode.ToSwedish;
            translateMinigame.StartGame(mode, ScrambleWordList(new List<Word>(adjectiveList.adjectiveList)));
            translateGameTypeMenu.SetActive(false);
            UnsubscribeTranslateStartButtons();
        }

        private void StartTimeTranslateGame(bool _toFinnish)
        {
            TranslateMinigame.GameMode mode = _toFinnish ? TranslateMinigame.GameMode.ToFinnish : TranslateMinigame.GameMode.ToSwedish;
            translateMinigame.StartGame(mode, ScrambleWordList(new List<Word>(timeList.timeList)));
            translateGameTypeMenu.SetActive(false);
            UnsubscribeTranslateStartButtons();
        }

        private void StartNumberTranslateGame(bool _toFinnish)
        {
            TranslateMinigame.GameMode mode = _toFinnish ? TranslateMinigame.GameMode.ToFinnish : TranslateMinigame.GameMode.ToSwedish;
            translateMinigame.StartGame(mode, ScrambleWordList(new List<Word>(numberList.numberList)));
            translateGameTypeMenu.SetActive(false);
            UnsubscribeTranslateStartButtons();
        }

        private void StartGrammarTranslateGame(bool _toFinnish)
        {
            TranslateMinigame.GameMode mode = _toFinnish ? TranslateMinigame.GameMode.ToFinnish : TranslateMinigame.GameMode.ToSwedish;
            translateMinigame.StartGame(mode, ScrambleWordList(new List<Word>(grammarList.grammarList)));
            translateGameTypeMenu.SetActive(false);
            UnsubscribeTranslateStartButtons();
        }

        private void StartPronounTranslateGame(bool _toFinnish)
        {
            TranslateMinigame.GameMode mode = _toFinnish ? TranslateMinigame.GameMode.ToFinnish : TranslateMinigame.GameMode.ToSwedish;
            translateMinigame.StartGame(mode, ScrambleWordList(new List<Word>(pronounList.pronounList)));
            translateGameTypeMenu.SetActive(false);
            UnsubscribeTranslateStartButtons();
        }

        private void StartPhraseTranslateGame(bool _toFinnish)
        {
            TranslateMinigame.GameMode mode = _toFinnish ? TranslateMinigame.GameMode.ToFinnish : TranslateMinigame.GameMode.ToSwedish;
            translateMinigame.StartGame(mode, ScrambleWordList(new List<Word>(phraseList.phraseList)));
            translateGameTypeMenu.SetActive(false);
            UnsubscribeTranslateStartButtons();
        }

        private void UnsubscribeTranslateStartButtons()
        {
            startTranslateNounGameBtn.onClick.RemoveAllListeners();
            startTranslateVerbGameBtn.onClick.RemoveAllListeners();
            startTranslateAdjectiveGameBtn.onClick.RemoveAllListeners();
            startTranslateTimeGameBtn.onClick.RemoveAllListeners();
            startTranslateNumberGameBtn.onClick.RemoveAllListeners();
            startTranslateGrammarGameBtn.onClick.RemoveAllListeners();
            startTranslatePronounGameBtn.onClick.RemoveAllListeners();
            startTranslatePhraseGameBtn.onClick.RemoveAllListeners();
        }

        #endregion

        #region word list-related methods

        /// <summary>
        /// This method can be used to randomize the contents of the word lists
        /// </summary>
        private List<Word> ScrambleWordList(List<Word> _startList)
        {
            System.Random rng = new();
            return _startList.OrderBy(a => rng.Next()).ToList();
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
        /// Assign this to a TextMeshProUGUI's RegisterDirtyLayoutCallback event to fix soft hyphen spacing
        /// </summary>
        /// <param name="_textField">TextMeshProUGUI to fix spacing for</param>
        public void FixTextSpacing(TextMeshProUGUI _textField)
        {
            string oldString = _textField.text;
            string newString;

            if (hyperlegibleOn)
            {
                if (oldString.Contains(spaceTagStart)) return;

                newString = oldString.Replace(@"\u00AD", spaceTagsWithSoftHyphen);

                while (newString.Contains(spaceTagsWithSoftHyphen))
                {
                    int colorEndIndex = newString.IndexOf(colorEndTag);
                    int index = newString.IndexOf(spaceTagsWithSoftHyphen);
                    int offset = 0;
                    if (newString.Contains(spaceEndWithColorEnd) && index + spaceTagsWithSoftHyphen.Length + colorEndTag.Length <= newString.Length
                        && newString.Substring(index + spaceTagsWithSoftHyphen.Length, colorEndTag.Length) == colorEndTag)
                    {
                        offset = colorEndTag.Length;
                    }
                    int movedCharIndex = index + offset + spaceTagsWithSoftHyphen.Length;
                    if (movedCharIndex == newString.Length) break;

                    char charToMove = newString[movedCharIndex];
                    newString = newString.Remove(movedCharIndex, 1);
                    newString = newString.Insert(index + spaceTagStart.Length + 1, charToMove.ToString());
                    
                    //moved character wasn't included by color tags before but is included now
                    if (colorEndIndex < movedCharIndex && colorEndIndex > index + spaceTagStart.Length)
                    {
                        newString = newString.Replace(colorEndTag, null);
                        newString = newString.Insert(index + spaceTagStart.Length + 1, colorEndTag);
                    }
                }
            }
            else
            {
                if (!oldString.Contains(spaceTagStart)) return;

                newString = oldString.Replace(spaceTagStart, null);
                newString = newString.Replace(spaceTagEnd, null);
            }

            newString = newString.Replace("\u00AD", @"\u00AD");

            if (newString != oldString) _textField.text = newString;
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
            if (!mouseOverSettings)
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