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
        public static UIManager Instance { get; private set; }

        #region variables

        [Header("Word Lists")]
        public VerbList verbList;
        public NounList nounList;
        public AdjectiveList adjectiveList;
        public TimeList timeList;
        public NumberList numberList;
        public GrammarList grammarList;
        public GrammarList adverbList;
        public GrammarList prepositionList;
        public GrammarList questionList;
        public PronounList pronounList;
        public PhraseList phraseList;


        [Header("Input-Related")]
        [SerializeField] private InputReader inputReader;

        [Header("Minigame-Related")]

        //Dictionary
        [SerializeField] private DictionaryHandler dictionaryHandler;
        [SerializeField] private Button openDictionaryButton;

        //Translate-, conjugation- & declension minigames
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
        [SerializeField] private Button startTranslateAdverbGameBtn;
        [SerializeField] private Button startTranslatePrepositionGameBtn;
        [SerializeField] private Button startTranslateQuestionGameBtn;
        [SerializeField] private ConjugationMinigame conjugationMinigame;
        [SerializeField] private DeclensionMinigame declensionMinigame;

        //Flashcard minigame
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
        [SerializeField] private Button startFlashcardAdverbGameBtn;
        [SerializeField] private Button startFlashcardPrepositionGameBtn;
        [SerializeField] private Button startFlashcardQuestionGameBtn;
        [SerializeField] private FlashCardMinigame flashCardMinigame;

        //Minigame Endscreen
        [SerializeField] private GameObject minigameEndscreen;
        [SerializeField] private GameObject minigameEndscreenImprovementObject;
        [SerializeField] private Button disableMinigameEndscreenBtn;
        [SerializeField] private Transform mistakeWordsHolder;
        [SerializeField] private GameObject mistakeWordPrefab;
        [SerializeField] private TextMeshProUGUI endMessageField;
        [SerializeField] private TextMeshProUGUI scoreField;
        [SerializeField] private Scrollbar mistakeScroller;
        private const string endMessagePerfect = "Täydet pisteet!";
        private const string endMessageGood = "Hyvä suoritus!";
        private const string endMessageMid = "Parannuksen varaa!";

        [Header("Lightmode-Related")]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image flashcardGameTypeBackground;
        [SerializeField] private Image translateGameTypeBackground;
        [SerializeField] private List<TextMeshProUGUI> textObjectList;
        [SerializeField] private List<TextMeshProUGUI> textObjectListReverseLight;
        [SerializeField] private List<TextMeshProUGUI> textObjectListHighlight;
        [SerializeField] private List<Image> lightmodableImages;
        [SerializeField] private List<Image> lightmodableImagesReverse;
        [SerializeField] private List<Image> highlightImages;
        [SerializeField] private List<Image> highlightImagesReverse;
        [SerializeField] private List<Image> buttonImages;
        [SerializeField] private List<Image> flashcardBases;
        [field: SerializeField] public Sprite AbortSpriteDarkmode { get; private set; }
        [field: SerializeField] public Sprite AbortSpriteLightmode { get; private set; }
        [field: SerializeField] public Sprite ButtonSpriteDarkmode { get; private set; }
        [field: SerializeField] public Sprite ButtonSpriteLightmode { get; private set; }
        [field: SerializeField] public Sprite CardSpriteDarkmode { get; private set; }
        [field: SerializeField] public Sprite CardSpriteLightmode { get; private set; }
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
        public readonly Color32 LightmodeHighlightTransparent = new(1, 111, 185, 192);
        public readonly Color32 DarkmodeHighlightTransparent = new(239, 160, 11, 192);

        [Header("Font-Related")]
        private List<TextMeshProUGUI> textFields;
        public bool HyperlegibleOn { get; private set; } = true;
        [SerializeField] private Toggle toggleHyperlegible;
        [field: SerializeField] public TMP_FontAsset LegibleFont { get; private set; }
        [field: SerializeField] public TMP_FontAsset BasicFont { get; private set; }
        public int LegibleSpacing { get; private set; } = 8;
        public int BasicSpacing { get; private set; } = 0;
        // private bool fontSettingsSubscribed = false;
        public event Action LegibleModeOnEvent;
        public event Action LegibleModeOffEvent;
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
            if (Instance == null)
            {
                Instance = this;
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

            openDictionaryButton.onClick.AddListener(() => dictionaryHandler.gameObject.SetActive(true));

            //Add listeners to settings-related buttons
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
            startFlashcardAdverbGameBtn.onClick.AddListener(() =>
        {
            flashCardMinigame.StartGrammarGame(adverbList.grammarList.ToArray());
            flashcardGameTypeMenu.SetActive(false);
        });
            startFlashcardPrepositionGameBtn.onClick.AddListener(() =>
        {
            flashCardMinigame.StartGrammarGame(prepositionList.grammarList.ToArray());
            flashcardGameTypeMenu.SetActive(false);
        });
            startFlashcardQuestionGameBtn.onClick.AddListener(() =>
        {
            flashCardMinigame.StartGrammarGame(questionList.grammarList.ToArray());
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
                startTranslateAdverbGameBtn.onClick.AddListener(() => StartAdverbTranslateGame(_toFinnish: true));
                startTranslatePrepositionGameBtn.onClick.AddListener(() => StartPrepositionTranslateGame(_toFinnish: true));
                startTranslateQuestionGameBtn.onClick.AddListener(() => StartQuestionTranslateGame(_toFinnish: true));
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
                startTranslateAdverbGameBtn.onClick.AddListener(() => StartAdverbTranslateGame(_toFinnish: false));
                startTranslatePrepositionGameBtn.onClick.AddListener(() => StartPrepositionTranslateGame(_toFinnish: false));
                startTranslateQuestionGameBtn.onClick.AddListener(() => StartQuestionTranslateGame(_toFinnish: false));
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
            LightmodeOn = !LightmodeOn;
            
            //Lightmode goes ON here
            if (LightmodeOn)
            {
                AudioManager.Instance.PlayLightModeToggle();
                textObjectList.ForEach((textObject) => textObject.color = Darkgrey);
                textObjectListReverseLight.ForEach((textObject) => textObject.color = Lightgrey);
                textObjectListHighlight.ForEach((textObject) => textObject.color = LightmodeHighlight);
                lightmodableImages.ForEach((imgObject) => imgObject.color = Lightgrey);
                lightmodableImagesReverse.ForEach((imgObject) => imgObject.color = Darkgrey);
                highlightImages.ForEach((imgObject) => imgObject.color = LightmodeHighlight);
                highlightImagesReverse.ForEach((imgObject) => imgObject.color = DarkmodeHighlight);
                buttonImages.ForEach((buttonImg) => buttonImg.sprite = ButtonSpriteLightmode);
                flashcardBases.ForEach((baseImg) => baseImg.sprite = CardSpriteLightmode);

                flashcardGameTypeBackground.color = LightgreyMostAlpha;
                translateGameTypeBackground.color = LightgreyMostAlpha;
                closeFlashcardMenuBtn.image.sprite = AbortSpriteLightmode;
                closeTranslateMenuBtn.image.sprite = AbortSpriteLightmode;
                openCreditsText.color = Darkgrey;
                settingsMenuImage.sprite = ButtonSpriteLightmode;

                LightmodeOnEvent?.Invoke();
            }
            else //Darkmode goes ON here
            {
                LightmodeOn = false;
                AudioManager.Instance.PlayLightModeToggle();
                textObjectList.ForEach((textObject) => textObject.color = Lightgrey);
                textObjectListReverseLight.ForEach((textObject) => textObject.color = Darkgrey);
                textObjectListHighlight.ForEach((textObject) => textObject.color = DarkmodeHighlight);
                lightmodableImages.ForEach((imgObject) => imgObject.color = Darkgrey);
                lightmodableImagesReverse.ForEach((imgObject) => imgObject.color = Lightgrey);
                highlightImages.ForEach((imgObject) => imgObject.color = DarkmodeHighlight);
                highlightImagesReverse.ForEach((imgObject) => imgObject.color = LightmodeHighlight);
                buttonImages.ForEach((buttonImg) => buttonImg.sprite = ButtonSpriteDarkmode);
                flashcardBases.ForEach((baseImg) => baseImg.sprite = CardSpriteDarkmode);

                flashcardGameTypeBackground.color = DarkgreyMostAlpha;
                translateGameTypeBackground.color = DarkgreyMostAlpha;
                closeFlashcardMenuBtn.image.sprite = AbortSpriteDarkmode;
                closeTranslateMenuBtn.image.sprite = AbortSpriteDarkmode;
                openCreditsText.color = Lightgrey;
                settingsMenuImage.sprite = ButtonSpriteDarkmode;

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

        private void StartAdverbTranslateGame(bool _toFinnish)
        {
            TranslateMinigame.GameMode mode = _toFinnish ? TranslateMinigame.GameMode.ToFinnish : TranslateMinigame.GameMode.ToSwedish;
            translateMinigame.StartGame(mode, ScrambleWordList(new List<Word>(adverbList.grammarList)));
            translateGameTypeMenu.SetActive(false);
            UnsubscribeTranslateStartButtons();
        }

        private void StartPrepositionTranslateGame(bool _toFinnish)
        {
            TranslateMinigame.GameMode mode = _toFinnish ? TranslateMinigame.GameMode.ToFinnish : TranslateMinigame.GameMode.ToSwedish;
            translateMinigame.StartGame(mode, ScrambleWordList(new List<Word>(prepositionList.grammarList)));
            translateGameTypeMenu.SetActive(false);
            UnsubscribeTranslateStartButtons();
        }

        private void StartQuestionTranslateGame(bool _toFinnish)
        {
            TranslateMinigame.GameMode mode = _toFinnish ? TranslateMinigame.GameMode.ToFinnish : TranslateMinigame.GameMode.ToSwedish;
            translateMinigame.StartGame(mode, ScrambleWordList(new List<Word>(questionList.grammarList)));
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
            startTranslateAdverbGameBtn.onClick.RemoveAllListeners();
            startTranslatePrepositionGameBtn.onClick.RemoveAllListeners();
            startTranslateQuestionGameBtn.onClick.RemoveAllListeners();
        }

        public void ActivateMinigameEndscreen(int _maxScore, int _realScore, float _goodScoreThreshold, List<Word> _wordsToImprove)
        {
            minigameEndscreen.SetActive(true);
            float scorePercentage = _realScore / (float)_maxScore;
            scoreField.text = string.Concat(_realScore, "/", _maxScore);
            disableMinigameEndscreenBtn.onClick.AddListener(() => minigameEndscreen.SetActive(false));
            mistakeScroller.value = 0f;

            //Handle endscreen message
            if (_realScore == _maxScore)
            {
                //Perfect score means no words to improve, can return here.
                PlayYellowSparkles();
                PlayBlueSparkles();
                endMessageField.text = endMessagePerfect;
                minigameEndscreenImprovementObject.SetActive(false);
                return;
            }
            else if (scorePercentage > _goodScoreThreshold)
            {
                endMessageField.text = endMessageGood;
            }
            else
            {
                endMessageField.text = endMessageMid;
            }

            //If no words to improve, method is done.
            if (_wordsToImprove.Count == 0)
            {
                minigameEndscreenImprovementObject.SetActive(false);
                return;
            }

            minigameEndscreenImprovementObject.SetActive(true);
            List<TextMeshProUGUI> fields = new();
            List<Image> spacers = new();

            foreach (Word wordToImprove in _wordsToImprove)
            {
                MistakeWordHandler mistakeWordHandler = Instantiate(mistakeWordPrefab, mistakeWordsHolder).GetComponent<MistakeWordHandler>();
                mistakeWordHandler.finnishWordField.text = wordToImprove.finnishWord;
                mistakeWordHandler.swedishWordField.text = wordToImprove.swedishWord;
                mistakeWordHandler.spacer.color = LightmodeOn ? Darkgrey : Lightgrey;
                InitTextFieldAppearance(ref mistakeWordHandler.finnishWordField);
                InitTextFieldAppearance(ref mistakeWordHandler.swedishWordField);

                spacers.Add(mistakeWordHandler.spacer);
                fields.Add(mistakeWordHandler.finnishWordField);
                fields.Add(mistakeWordHandler.swedishWordField);
            }

            AddToTextLists(fields);
            lightmodableImagesReverse.AddRange(spacers);
            disableMinigameEndscreenBtn.onClick.AddListener(() =>
            {
                RemoveFromTextLists(fields);
                spacers.ForEach(spacer => lightmodableImagesReverse.Remove(spacer));
                foreach (Transform child in mistakeWordsHolder)
                {
                    Destroy(child.gameObject);
                }
            });
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

        public void AddToTextLists(List<TextMeshProUGUI> _fieldsToAdd)
        {
            textObjectList.AddRange(_fieldsToAdd);
            textFields.AddRange(_fieldsToAdd);
        }

        public void RemoveFromTextLists(List<TextMeshProUGUI> _fieldsToRemove)
        {
            _fieldsToRemove.ForEach(field =>
            {
                textObjectList.Remove(field);
                textFields.Remove(field);
            });
        }

        #endregion

        #region settings-related methods

        private void ToggleHyperlegibleFont(bool _toggledOn)
        {
            if (_toggledOn)
            {
                HyperlegibleOn = true;
                textFields.ForEach((textObject) =>
                {
                    textObject.font = LegibleFont;
                    textObject.characterSpacing = LegibleSpacing;
                });
                LegibleModeOnEvent?.Invoke();
            }
            else
            {
                HyperlegibleOn = false;
                textFields.ForEach((textObject) =>
                {
                    textObject.font = BasicFont;
                    textObject.characterSpacing = BasicSpacing;
                });
                LegibleModeOffEvent?.Invoke();
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

            if (HyperlegibleOn)
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

        public void InitTextFieldAppearance(ref TextMeshProUGUI _field)
        {
            if (LightmodeOn)
            {
                _field.color = Darkgrey;
            }
            else
            {
                _field.color = Lightgrey;
            }

            if (HyperlegibleOn)
            {
                _field.font = LegibleFont;
                _field.characterSpacing = LegibleSpacing;
            }
            else
            {
                _field.font = BasicFont;
                _field.characterSpacing = BasicSpacing;
            }
        }

        #endregion
    }
}