using System;
using System.Collections;
using System.Collections.Generic;
using SwedishApp.UI;
using SwedishApp.Words;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SwedishApp.Minigames
{
    /// <summary>
    /// This script handles all flashcard minigames. The script should be attached to
    /// the menu object that holds all flashcard minigame -related objects!
    /// </summary>
    public class FlashCardMinigame : MonoBehaviour
    {
        //private variables, not shown in editor
        private Word[] activeWordArray;
        public enum GameType
        {
            verb = 0,
            noun,
            adjective,
            number,
            time,
            grammar,
            pronoun,
            phrase
        }
        private Action displayWordAction;

        [Header("Progress visualization")]
        [SerializeField] private TextMeshProUGUI progressText;
        private int _activeWordIndex = 0;
        private int ActiveWordIndex
        {
            get
            {
                return _activeWordIndex;
            }
            set
            {
                _activeWordIndex = value;
                progressText.text = string.Concat(value + 1, "/", gameCardsCount);
                if (value <= 0) prevWordBtn.gameObject.SetActive(false);
                else prevWordBtn.gameObject.SetActive(true);
            }
        }
        private int gameCardsCount = 0;

        [Header("Set delays related to game flow")]
        [SerializeField] private float nextWordDelay = 0.25f;
        private WaitForSeconds nextWordWait;

        [Header("UI holders for flash card bases")]
        [SerializeField] private FlashCardNoun nounObject;
        [SerializeField] private FlashCardVerb verbObject;
        [SerializeField] private FlashCardAdjective adjectiveObject;
        [SerializeField] private FlashCardBase timeObject;
        [SerializeField] private FlashCardNumber numberObject;
        [SerializeField] private FlashCardBase phraseObject;
        [SerializeField] private FlashCardGrammar grammarObject;
        [SerializeField] private FlashCardPronoun pronounObject;
        private FlashCardBase activeFlashcard;

        [Header("Game flow-related buttons")]
        [SerializeField] private Button nextWordBtn;
        [SerializeField] private Button prevWordBtn;
        [SerializeField] private TextMeshProUGUI nextWordTxt;
        [SerializeField] private TextMeshProUGUI prevWordTxt;
        [SerializeField] private Button abortGameButton;

        [Header("Tutorial-related")]
        [SerializeField] private List<TutorialHandler> flashcardTutorials;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            nounObject.gameObject.SetActive(false);
            abortGameButton.onClick.AddListener(EndGame);
            UIManager.Instance.LightmodeOnEvent += ToLightmode;
            UIManager.Instance.LightmodeOffEvent += ToDarkmode;
            nextWordWait = new(nextWordDelay);

            if (UIManager.Instance.LightmodeOn) ToLightmode();
            else ToDarkmode();
        }

        #region settings-related methods

        /// <summary>
        /// This method is subscribed to <see cref="UIManager.LightmodeOnEvent"/>, and handles changing sprites
        /// and colors to fit the light mode
        /// </summary>
        private void ToLightmode()
        {
            abortGameButton.image.sprite = UIManager.Instance.AbortSpriteLightmode;
            nextWordBtn.image.sprite = UIManager.Instance.ButtonSpriteLightmode;
            prevWordBtn.image.sprite = UIManager.Instance.ButtonSpriteLightmode;
            nextWordTxt.color = UIManager.Instance.Darkgrey;
            prevWordTxt.color = UIManager.Instance.Darkgrey;
        }

        /// <summary>
        /// This method is subscribed to <see cref="UIManager.LightmodeOffEvent"/>, and handles changing sprites
        /// and colors to fit the dark mode
        /// </summary>
        private void ToDarkmode()
        {
            abortGameButton.image.sprite = UIManager.Instance.AbortSpriteDarkmode;
            nextWordBtn.image.sprite = UIManager.Instance.ButtonSpriteDarkmode;
            prevWordBtn.image.sprite = UIManager.Instance.ButtonSpriteDarkmode;
            nextWordTxt.color = UIManager.Instance.Lightgrey;
            prevWordTxt.color = UIManager.Instance.Lightgrey;
        }

        private void ShowRelevantTutorial()
        {
            bool foundTutorialToShow = false;
            foreach (TutorialHandler tutorial in flashcardTutorials)
            {
                if (!tutorial.TutorialSeen() && !foundTutorialToShow)
                {
                    tutorial.ShowTutorial();
                    foundTutorialToShow = true;
                }
                else
                {
                    tutorial.gameObject.SetActive(false);
                }
            }
        }

        #endregion

        #region universal methods

        /// <summary>
        /// This method sets up and starts the noun game. Enables relevant game objects, populates the
        /// <seealso cref="activeWordArray"/> array with nouns to be included in the game.
        /// </summary>
        /// <param name="_words">This parameter is used to populate the activeWordArray array</param>
        public void StartGame(Word[] _words, GameType _gameType)
        {
            switch (_gameType)
            {
                case GameType.verb:
                    activeFlashcard = verbObject;
                    UIManager.Instance.LightmodeOnEvent += DisplayCurrentVerb;
                    UIManager.Instance.LightmodeOffEvent += DisplayCurrentVerb;
                    displayWordAction = DisplayCurrentVerb;
                    break;
                case GameType.noun:
                    activeFlashcard = nounObject;
                    UIManager.Instance.LightmodeOnEvent += DisplayCurrentNoun;
                    UIManager.Instance.LightmodeOffEvent += DisplayCurrentNoun;
                    displayWordAction = DisplayCurrentNoun;
                    break;
                case GameType.adjective:
                    activeFlashcard = adjectiveObject;
                    UIManager.Instance.LightmodeOnEvent += DisplayCurrentAdjective;
                    UIManager.Instance.LightmodeOffEvent += DisplayCurrentAdjective;
                    displayWordAction = DisplayCurrentAdjective;
                    break;
                case GameType.time:
                    activeFlashcard = timeObject;
                    UIManager.Instance.LightmodeOnEvent += DisplayCurrentTimeWord;
                    UIManager.Instance.LightmodeOffEvent += DisplayCurrentTimeWord;
                    displayWordAction = DisplayCurrentTimeWord;
                    break;
                case GameType.number:
                    activeFlashcard = numberObject;
                    UIManager.Instance.LightmodeOnEvent += DisplayCurrentNumberWord;
                    UIManager.Instance.LightmodeOffEvent += DisplayCurrentNumberWord;
                    displayWordAction = DisplayCurrentNumberWord;
                    break;
                case GameType.grammar:
                    activeFlashcard = grammarObject;
                    UIManager.Instance.LightmodeOnEvent += DisplayCurrentGrammarWord;
                    UIManager.Instance.LightmodeOffEvent += DisplayCurrentGrammarWord;
                    displayWordAction = DisplayCurrentGrammarWord;
                    break;
                case GameType.pronoun:
                    activeFlashcard = pronounObject;
                    UIManager.Instance.LightmodeOnEvent += DisplayCurrentPronounWord;
                    UIManager.Instance.LightmodeOffEvent += DisplayCurrentPronounWord;
                    displayWordAction = DisplayCurrentPronounWord;
                    break;
                case GameType.phrase:
                    activeFlashcard = phraseObject;
                    UIManager.Instance.LightmodeOnEvent += DisplayCurrentPhraseWord;
                    UIManager.Instance.LightmodeOffEvent += DisplayCurrentPhraseWord;
                    displayWordAction = DisplayCurrentPhraseWord;
                    break;
            }

            activeFlashcard.gameObject.SetActive(true);
            gameObject.SetActive(true);

            //(Re)set variables
            activeWordArray = _words;
            gameCardsCount = activeWordArray.Length;
            ActiveWordIndex = 0;

            //Set initial colors
            if (UIManager.Instance.LightmodeOn)
                activeFlashcard.LightsOn();
            else
                activeFlashcard.LightsOff();

            nextWordBtn.onClick.AddListener(NextWord);
            prevWordBtn.onClick.AddListener(PreviousWord);
            ShowRelevantTutorial();
            StartCoroutine(DisplayWord());
            activeFlashcard.FixTextFields();
            activeFlashcard.ResetToFinnishSide();
        }

        /// <summary>
        /// After a short delay determined by <see cref="nextWordDelay"/>, shows the next flash card
        /// </summary>
        /// <returns></returns>
        private IEnumerator DisplayWord()
        {
            //Disable button, hide the flashcard and populate its text fields
            nextWordBtn.interactable = false;
            prevWordBtn.interactable = false;
            activeFlashcard.gameObject.SetActive(false);
            displayWordAction?.Invoke();

            //After a delay, display the flashcard again
            yield return nextWordWait;
            activeFlashcard.gameObject.SetActive(true);

            //After another delay of the same length, enable button again
            yield return nextWordWait;
            nextWordBtn.interactable = true;
            prevWordBtn.interactable = true;
        }

        /// <summary>
        /// This method is subscribed to the "Next" button on screen, and calls relevant methods
        /// based on the state of the current flashcard and the overall game. If the game can keep
        /// going, reset card, increment the currently active word int and display the next word.
        /// If not, end game without doing anything.
        /// </summary>
        private void NextWord()
        {
            if (activeFlashcard.state == FlashCardBase.State.Flipping) return;
            if (ActiveWordIndex + 1 >= activeWordArray.Length)
            {
                EndGame();
                return;
            }
            activeFlashcard.ResetToFinnishSide();
            ActiveWordIndex++;
            StartCoroutine(DisplayWord());
        }

        private void PreviousWord()
        {
            if (activeFlashcard.state == FlashCardBase.State.Flipping || ActiveWordIndex <= 0) return;
            activeFlashcard.ResetToFinnishSide();
            ActiveWordIndex--;
            StartCoroutine(DisplayWord());
        }

        #endregion

        #region noun-related methods

        /// <summary>
        /// This method updates all of the noun flashcard's text fields to match the current word
        /// </summary>
        private void DisplayCurrentNoun()
        {
            NounWord activeWord = activeWordArray[ActiveWordIndex] as NounWord;
            nounObject.wordFinnishText.text = activeWord.finnishWord;
            nounObject.wordSwedishBaseText.text = activeWord.NounWithGenderStart();
            nounObject.wordSwedishDefinitiveText.text = activeWord.NounWithGenderEnd();
            nounObject.wordSwedishPluralText.text = activeWord.PluralNoun();
            nounObject.wordSwedishDefinitivePluralText.text = activeWord.PluralDefinitiveNoun();
            nounObject.wordDeclensionClassText.text = activeWord.declensionClass.ToString();

            nounObject.SetInitialElements(activeWord.lightModeSprite, activeWord.darkModeSprite);
        }

        private void EndGame()
        {
            nounObject.gameObject.SetActive(false);
            verbObject.gameObject.SetActive(false);
            adjectiveObject.gameObject.SetActive(false);
            timeObject.gameObject.SetActive(false);
            numberObject.gameObject.SetActive(false);
            grammarObject.gameObject.SetActive(false);
            pronounObject.gameObject.SetActive(false);
            phraseObject.gameObject.SetActive(false);
            nextWordBtn.onClick.RemoveAllListeners();
            prevWordBtn.onClick.RemoveAllListeners();
            UIManager.Instance.LightmodeOnEvent -= DisplayCurrentNoun;
            UIManager.Instance.LightmodeOffEvent -= DisplayCurrentNoun;
            UIManager.Instance.LightmodeOnEvent -= DisplayCurrentVerb;
            UIManager.Instance.LightmodeOffEvent -= DisplayCurrentVerb;
            UIManager.Instance.LightmodeOnEvent -= DisplayCurrentAdjective;
            UIManager.Instance.LightmodeOffEvent -= DisplayCurrentAdjective;
            UIManager.Instance.LightmodeOnEvent -= DisplayCurrentTimeWord;
            UIManager.Instance.LightmodeOffEvent -= DisplayCurrentTimeWord;
            UIManager.Instance.LightmodeOnEvent -= DisplayCurrentNumberWord;
            UIManager.Instance.LightmodeOffEvent -= DisplayCurrentNumberWord;
            UIManager.Instance.LightmodeOnEvent -= DisplayCurrentGrammarWord;
            UIManager.Instance.LightmodeOffEvent -= DisplayCurrentGrammarWord;
            UIManager.Instance.LightmodeOnEvent -= DisplayCurrentPronounWord;
            UIManager.Instance.LightmodeOffEvent -= DisplayCurrentPronounWord;
            UIManager.Instance.LightmodeOnEvent -= DisplayCurrentPhraseWord;
            UIManager.Instance.LightmodeOffEvent -= DisplayCurrentPhraseWord;
            UIManager.Instance.TriggerTipChange();
            gameObject.SetActive(false);
        }

        #endregion

        #region verb-related methods

        /// <summary>
        /// This method updates all of the verb flashcard's text fields to match the current word
        /// </summary>
        private void DisplayCurrentVerb()
        {
            VerbWord activeWord = activeWordArray[ActiveWordIndex] as VerbWord;
            verbObject.wordFinnishText.text = activeWord.finnishWord;
            verbObject.wordSwedishBaseText.text = activeWord.BaseformWord();
            verbObject.wordSwedishCurrentText.text = activeWord.CurrentTenseWord();
            verbObject.wordSwedishPastText.text = activeWord.PastTenseWord();
            verbObject.wordSwedishPastPerfectText.text = activeWord.PastPerfectTenseWord();
            verbObject.wordSwedishPastPlusPerfectText.text = activeWord.PastPlusPerfectTenseWord();
            verbObject.wordConjugationClassText.text = activeWord.conjugationClass.ToString();

            verbObject.SetInitialElements(activeWord.lightModeSprite, activeWord.darkModeSprite);
        }

        #endregion

        #region adjective-related methods

        /// <summary>
        /// This method updates all of the adjective flashcard's text fields to match the current word
        /// </summary>
        private void DisplayCurrentAdjective()
        {
            AdjectiveWord activeWord = activeWordArray[ActiveWordIndex] as AdjectiveWord;
            adjectiveObject.wordFinnishText.text = activeWord.finnishWord;
            adjectiveObject.wordSwedishBaseText.text = activeWord.HighlightedSwedishWord();
            adjectiveObject.wordSwedishComparativeText.text = activeWord.AdjectiveComparative();
            adjectiveObject.wordSwedishSuperlativeText.text = activeWord.AdjectiveSuperlative();

            adjectiveObject.SetInitialElements(activeWord.lightModeSprite, activeWord.darkModeSprite);
        }

        #endregion

        #region time-related methods

        /// <summary>
        /// This method updates all of the time flashcard's text fields to match the current word
        /// </summary>
        private void DisplayCurrentTimeWord()
        {
            Word activeWord = activeWordArray[ActiveWordIndex];
            timeObject.wordFinnishText.text = activeWord.finnishWord;
            timeObject.wordSwedishBaseText.text = activeWord.swedishWord;

            timeObject.SetInitialElements(activeWord.lightModeSprite, activeWord.darkModeSprite);
        }

        #endregion

        #region number-related methods

        /// <summary>
        /// This method updates all of the number flashcard's text fields to match the current word
        /// </summary>
        private void DisplayCurrentNumberWord()
        {
            NumberWord activeWord = activeWordArray[ActiveWordIndex] as NumberWord;
            numberObject.wordFinnishText.text = activeWord.finnishWord;
            numberObject.wordFinnishOrdinalText.text = activeWord.ordinalFinnish;
            numberObject.wordSwedishBaseText.text = activeWord.swedishWord;
            numberObject.wordSwedishOrdinalText.text = activeWord.ordinalSwedish;

            numberObject.SetInitialElements(activeWord.lightModeSprite, activeWord.darkModeSprite);
        }

        #endregion

        #region grammar-related methods

        /// <summary>
        /// This method updates all of the grammar flashcard's text fields to match the current word
        /// </summary>
        private void DisplayCurrentGrammarWord()
        {
            GrammarWord activeWord = activeWordArray[ActiveWordIndex] as GrammarWord;
            grammarObject.wordFinnishText.text = activeWord.finnishWord;
            grammarObject.wordSwedishBaseText.text = activeWord.swedishWord;
            grammarObject.wordNoticeText.text = activeWord.notices;
            if (grammarObject.wordNoticeText.text == "") grammarObject.infoHover.gameObject.SetActive(false);
            else grammarObject.infoHover.gameObject.SetActive(true);

            grammarObject.SetInitialElements(activeWord.lightModeSprite, activeWord.darkModeSprite);
        }

        #endregion

        #region pronoun-related methods

        /// <summary>
        /// This method updates all of the pronoun flashcard's text fields to match the current word
        /// </summary>
        private void DisplayCurrentPronounWord()
        {
            PronounWord activeWord = activeWordArray[ActiveWordIndex] as PronounWord;
            pronounObject.wordFinnishText.text = activeWord.finnishWithExplanation;
            pronounObject.wordSwedishBaseText.text = activeWord.swedishWord;
            pronounObject.wordSwedishPossessiveEn.text = activeWord.pronounPossessiveEnSwe;
            pronounObject.wordSwedishPossessiveEtt.text = activeWord.pronounPossessiveEttSwe;
            pronounObject.wordSwedishPossessivePlural.text = activeWord.pronounPossessivePluralSwe;
            pronounObject.wordSwedishObject.text = activeWord.pronounObjectSwe;
            pronounObject.wordFinnishPossessive.text = activeWord.pronounPossessiveFin;
            pronounObject.wordFinnishObject.text = activeWord.pronounObjectFin;

            pronounObject.SetInitialElements(activeWord.lightModeSprite, activeWord.darkModeSprite);
        }

        #endregion

        #region phrase-related methods

        /// <summary>
        /// This method updates all of the phrase flashcard's text fields to match the current word
        /// </summary>
        private void DisplayCurrentPhraseWord()
        {
            Word activeWord = activeWordArray[ActiveWordIndex];
            phraseObject.wordFinnishText.text = activeWord.finnishWord;
            phraseObject.wordSwedishBaseText.text = activeWord.swedishWord;

            phraseObject.SetInitialElements(activeWord.lightModeSprite, activeWord.darkModeSprite);
        }

        #endregion
    }
}
