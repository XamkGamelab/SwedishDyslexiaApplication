using System.Collections;
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
        private NounWord[] nounWords;
        private VerbWord[] verbWords;
        private AdjectiveWord[] adjectiveWords;
        private TimeWord[] timeWords;
        private NumberWord[] numberWords;
        private GrammarWord[] grammarWords;
        private PronounWord[] pronounWords;
        private Word[] phraseWords;
        private int activeWordIndex = 0;

        [Header("Set delays related to game flow")]
        [SerializeField] private float nextWordDelay = 0.25f;
        [SerializeField] private float gameEndDelay = 0.5f;
        private WaitForSeconds nextWordWait;
        private WaitForSeconds gameEndWait;

        [Header("UI holders for flash card bases")]
        [SerializeField] private FlashCardNoun nounObject;
        [SerializeField] private FlashCardVerb verbObject;
        [SerializeField] private FlashCardAdjective adjectiveObject;
        [SerializeField] private FlashCardBase timeObject;
        [SerializeField] private FlashCardNumber numberObject;
        [SerializeField] private FlashCardGrammar grammarObject;
        [SerializeField] private FlashCardPronoun pronounObject;
        [SerializeField] private FlashCardBase phraseObject;

        [Header("Game flow related buttons")]
        [SerializeField] private Button nextWordBtn;
        [SerializeField] private TextMeshProUGUI nextWordTxt;
        [SerializeField] private Button abortGameButton;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            nounObject.gameObject.SetActive(false);
            abortGameButton.onClick.AddListener(EndGame);
            UIManager.instance.LightmodeOnEvent += FlashcardGameToLightmode;
            UIManager.instance.LightmodeOffEvent += FlashcardGameToDarkmode;
            nextWordWait = new(nextWordDelay);
            gameEndWait = new(gameEndDelay);

            //Buttons' sprites are set according to whether or not light mode is on
            abortGameButton.image.sprite = UIManager.instance.LightmodeOn ? UIManager.instance.abortSpriteLightmode : UIManager.instance.abortSpriteDarkmode;
            nextWordBtn.image.sprite = UIManager.instance.LightmodeOn ? UIManager.instance.buttonSpriteLightmode : UIManager.instance.buttonSpriteDarkmode;
            nextWordTxt.color = UIManager.instance.LightmodeOn ? UIManager.instance.Darkgrey : UIManager.instance.Lightgrey;
        }

        #region lightmode related methods

        /// <summary>
        /// This method is subscribed to <see cref="UIManager.LightmodeOnEvent"/>, and handles changing sprites
        /// and colors to fit the light mode
        /// </summary>
        private void FlashcardGameToLightmode()
        {
            abortGameButton.image.sprite = UIManager.instance.abortSpriteLightmode;
            nextWordBtn.image.sprite = UIManager.instance.buttonSpriteLightmode;
            nextWordTxt.color = UIManager.instance.Darkgrey;
        }

        /// <summary>
        /// This method is subscribed to <see cref="UIManager.LightmodeOffEvent"/>, and handles changing sprites
        /// and colors to fit the dark mode
        /// </summary>
        private void FlashcardGameToDarkmode()
        {
            abortGameButton.image.sprite = UIManager.instance.abortSpriteDarkmode;
            nextWordBtn.image.sprite = UIManager.instance.buttonSpriteDarkmode;
            nextWordTxt.color = UIManager.instance.Lightgrey;
        }

        #endregion

        #region noun related methods

        /// <summary>
        /// This method updates all of the noun flashcard's text fields to match the current word
        /// </summary>
        private void DisplayCurrentNoun()
        {
            Debug.Log(activeWordIndex);
            nounObject.wordFinnishText.text = nounWords[activeWordIndex].finnishWord;
            nounObject.wordSwedishBaseText.text = nounWords[activeWordIndex].NounWithGenderStart();
            nounObject.wordSwedishDefinitiveText.text = nounWords[activeWordIndex].NounWithGenderEnd();
            nounObject.wordSwedishPluralText.text = nounWords[activeWordIndex].PluralNoun();
            nounObject.wordSwedishDefinitivePluralText.text = nounWords[activeWordIndex].PluralDefinitiveNoun();
            nounObject.wordDeclensionClassText.text = nounWords[activeWordIndex].declensionClass.ToString();

            nounObject.SetInitialElements(nounWords[activeWordIndex].lightModeSprite, nounWords[activeWordIndex].darkModeSprite);
        }

        /// <summary>
        /// This method sets up and starts the noun game. Enables relevant game objects, populates the
        /// <seealso cref="nounWords"/> array with nouns to be included in the game.
        /// </summary>
        /// <param name="_nounWords">This parameter is used to populate the nounWords array</param>
        public void StartNounGame(NounWord[] _nounWords)
        {
            //Enable relevant objects
            gameObject.SetActive(true);
            nounObject.gameObject.SetActive(true);

            //(Re)set variables
            nounWords = _nounWords;
            activeWordIndex = 0;

            //Set initial colors
            if (UIManager.instance.LightmodeOn)
                nounObject.LightsOn();
            else
                nounObject.LightsOff();


            //Add relevant listeners to game and UI events
            nextWordBtn.onClick.AddListener(NextNoun);
            UIManager.instance.LightmodeOnEvent += DisplayCurrentNoun;
            UIManager.instance.LightmodeOffEvent += DisplayCurrentNoun;

            //Start displaying words and reset the flash card to the finnish side if it was flipped
            StartCoroutine(DisplayNoun());
            nounObject.ResetToFinnishSide();
        }

        /// <summary>
        /// After a short delay determined by <see cref="nextWordDelay"/>, shows the next flash card
        /// </summary>
        /// <returns></returns>
        private IEnumerator DisplayNoun()
        {
            //Disable button, hide the flashcard and populate its text fields
            nextWordBtn.interactable = false;
            nounObject.gameObject.SetActive(false);
            DisplayCurrentNoun();

            //After a delay, display the flashcard again
            yield return new WaitForSeconds(nextWordDelay);
            nounObject.gameObject.SetActive(true);

            //After another delay of the same length, enable button again
            yield return new WaitForSeconds(nextWordDelay);
            nextWordBtn.interactable = true;
        }

        /// <summary>
        /// This method is subscribed to the "Next" button on screen, and calls relevant methods
        /// based on the state of the current flashcard and the overall game. If the game can keep
        /// going, reset card, increment the currently active word int and display the next word.
        /// If not, end game without doing anything.
        /// </summary>
        private void NextNoun()
        {
            if (nounObject.state == FlashCardBase.State.Flipping) return;
            if (activeWordIndex + 1 >= nounWords.Length)
            {
                EndGame();
                return;
            }
            nounObject.ResetToFinnishSide();
            activeWordIndex++;
            StartCoroutine(DisplayNoun());
        }

        #endregion

        #region verb related methods

        /// <summary>
        /// This method updates all of the verb flashcard's text fields to match the current word
        /// </summary>
        private void DisplayCurrentVerb()
        {
            Debug.Log(activeWordIndex);
            verbObject.wordFinnishText.text = verbWords[activeWordIndex].finnishWord;
            verbObject.wordSwedishBaseText.text = verbWords[activeWordIndex].BaseformWord();
            verbObject.wordSwedishCurrentText.text = verbWords[activeWordIndex].CurrentTenseWord();
            verbObject.wordSwedishPastText.text = verbWords[activeWordIndex].PastTenseWord();
            verbObject.wordSwedishPastPerfectText.text = verbWords[activeWordIndex].PastPerfectTenseWord();
            verbObject.wordSwedishPastPlusPerfectText.text = verbWords[activeWordIndex].PastPlusPerfectTenseWord();
            verbObject.wordConjugationClassText.text = verbWords[activeWordIndex].conjugationClass.ToString();

            verbObject.SetInitialElements(verbWords[activeWordIndex].lightModeSprite, verbWords[activeWordIndex].darkModeSprite);
        }

        /// <summary>
        /// This method sets up and starts the verb game. Enables relevant game objects, populates the
        /// <seealso cref="verbWords"/> array with verbs to be included in the game.
        /// </summary>
        /// <param name="_verbWords">This parameter is used to populate the verbWords array</param>
        public void StartVerbGame(VerbWord[] _verbWords)
        {
            //Enable relevant objects
            gameObject.SetActive(true);
            verbObject.gameObject.SetActive(true);

            //Set initial colors
            if (UIManager.instance.LightmodeOn)
                verbObject.LightsOn();
            else
                verbObject.LightsOff();

            //(Re)set variables
            verbWords = _verbWords;
            activeWordIndex = 0;

            //Add relevant listeners to game and UI events
            nextWordBtn.onClick.AddListener(NextVerb);
            UIManager.instance.LightmodeOnEvent += DisplayCurrentVerb;
            UIManager.instance.LightmodeOffEvent += DisplayCurrentVerb;

            //Start displaying words and reset the flash card to the finnish side if it was flipped
            StartCoroutine(DisplayVerb());
            verbObject.ResetToFinnishSide();
        }

        /// <summary>
        /// After a short delay determined by <see cref="nextWordDelay"/>, shows the next flash card
        /// </summary>
        /// <returns></returns>
        private IEnumerator DisplayVerb()
        {
            //Disable button, hide the flashcard and populate its text fields
            nextWordBtn.interactable = false;
            verbObject.gameObject.SetActive(false);
            DisplayCurrentVerb();

            //After a delay, display the flashcard again
            yield return new WaitForSeconds(nextWordDelay);
            verbObject.gameObject.SetActive(true);

            //After another delay of the same length, enable button again
            yield return new WaitForSeconds(nextWordDelay);
            nextWordBtn.interactable = true;
        }

        /// <summary>
        /// This method is subscribed to the "Next" button on screen, and calls relevant methods
        /// based on the state of the current flashcard and the overall game. If the game can keep
        /// going, reset card, increment the currently active word int and display the next word.
        /// If not, end game without doing anything.
        /// </summary>
        private void NextVerb()
        {
            if (verbObject.state == FlashCardBase.State.Flipping) return;
            if (activeWordIndex + 1 >= verbWords.Length)
            {
                EndGame();
                return;
            }
            verbObject.ResetToFinnishSide();
            activeWordIndex++;
            StartCoroutine(DisplayVerb());
        }

        #endregion

        #region adjective related methods

        /// <summary>
        /// This method updates all of the adjective flashcard's text fields to match the current word
        /// </summary>
        private void DisplayCurrentAdjective()
        {
            Debug.Log(activeWordIndex);
            adjectiveObject.wordFinnishText.text = adjectiveWords[activeWordIndex].finnishWord;
            adjectiveObject.wordSwedishBaseText.text = adjectiveWords[activeWordIndex].swedishWord;
            adjectiveObject.wordSwedishComparativeText.text = adjectiveWords[activeWordIndex].AdjectiveComparative();
            adjectiveObject.wordSwedishSuperlativeText.text = adjectiveWords[activeWordIndex].AdjectiveSuperlative();

            adjectiveObject.SetInitialElements(adjectiveWords[activeWordIndex].lightModeSprite, adjectiveWords[activeWordIndex].darkModeSprite);
        }

        /// <summary>
        /// This method sets up and starts the adjective game. Enables relevant game objects, populates the
        /// <seealso cref="adjectiveWords"/> array with adjectives to be included in the game.
        /// </summary>
        /// <param name="_adjectiveWords">This parameter is used to populate the adjectiveWords array</param>
        public void StartAdjectiveGame(AdjectiveWord[] _adjectiveWords)
        {
            //Enable relevant objects
            gameObject.SetActive(true);
            adjectiveObject.gameObject.SetActive(true);

            //Set initial colors
            if (UIManager.instance.LightmodeOn)
                adjectiveObject.LightsOn();
            else
                adjectiveObject.LightsOff();

            //(Re)set variables
            adjectiveWords = _adjectiveWords;
            activeWordIndex = 0;

            //Add relevant listeners to game and UI events
            nextWordBtn.onClick.AddListener(NextAdjective);
            UIManager.instance.LightmodeOnEvent += DisplayCurrentAdjective;
            UIManager.instance.LightmodeOffEvent += DisplayCurrentAdjective;

            //Start displaying words and reset the flash card to the finnish side if it was flipped
            StartCoroutine(DisplayAdjective());
            adjectiveObject.ResetToFinnishSide();
        }

        /// <summary>
        /// After a short delay determined by <see cref="nextWordDelay"/>, shows the next flash card
        /// </summary>
        /// <returns></returns>
        private IEnumerator DisplayAdjective()
        {
            //Disable button, hide the flashcard and populate its text fields
            nextWordBtn.interactable = false;
            adjectiveObject.gameObject.SetActive(false);
            DisplayCurrentAdjective();

            //After a delay, display the flashcard again
            yield return new WaitForSeconds(nextWordDelay);
            adjectiveObject.gameObject.SetActive(true);

            //After another delay of the same length, enable button again
            yield return new WaitForSeconds(nextWordDelay);
            nextWordBtn.interactable = true;
        }

        /// <summary>
        /// This method is subscribed to the "Next" button on screen, and calls relevant methods
        /// based on the state of the current flashcard and the overall game. If the game can keep
        /// going, reset card, increment the currently active word int and display the next word.
        /// If not, end game without doing anything.
        /// </summary>
        private void NextAdjective()
        {
            if (adjectiveObject.state == FlashCardBase.State.Flipping) return;
            if (activeWordIndex + 1 >= adjectiveWords.Length)
            {
                EndGame();
                return;
            }
            adjectiveObject.ResetToFinnishSide();
            activeWordIndex++;
            StartCoroutine(DisplayAdjective());
        }

        #endregion

        #region time related methods

        /// <summary>
        /// This method updates all of the time flashcard's text fields to match the current word
        /// </summary>
        private void DisplayCurrentTimeWord()
        {
            Debug.Log(activeWordIndex);
            timeObject.wordFinnishText.text = timeWords[activeWordIndex].finnishWord;
            timeObject.wordSwedishBaseText.text = timeWords[activeWordIndex].swedishWord;

            timeObject.SetInitialElements(timeWords[activeWordIndex].lightModeSprite, timeWords[activeWordIndex].darkModeSprite);
        }

        /// <summary>
        /// This method sets up and starts the time word game. Enables relevant game objects, populates the
        /// <seealso cref="timeWords"/> array with time words to be included in the game.
        /// </summary>
        /// <param name="_timeWords">This parameter is used to populate the timeWords array</param>
        public void StartTimeWordGame(TimeWord[] _timeWords)
        {
            //Enable relevant objects
            gameObject.SetActive(true);
            timeObject.gameObject.SetActive(true);

            //Set initial colors
            if (UIManager.instance.LightmodeOn)
                timeObject.LightsOn();
            else
                timeObject.LightsOff();

            //(Re)set variables
            timeWords = _timeWords;
            activeWordIndex = 0;

            //Add relevant listeners to game and UI events
            nextWordBtn.onClick.AddListener(NextTimeWord);
            UIManager.instance.LightmodeOnEvent += DisplayCurrentTimeWord;
            UIManager.instance.LightmodeOffEvent += DisplayCurrentTimeWord;

            //Start displaying words and reset the flash card to the finnish side if it was flipped
            StartCoroutine(DisplayTimeWord());
            timeObject.ResetToFinnishSide();
        }

        /// <summary>
        /// After a short delay determined by <see cref="nextWordDelay"/>, shows the next flash card
        /// </summary>
        /// <returns></returns>
        private IEnumerator DisplayTimeWord()
        {
            //Disable button, hide the flashcard and populate its text fields
            nextWordBtn.interactable = false;
            timeObject.gameObject.SetActive(false);
            DisplayCurrentTimeWord();

            //After a delay, display the flashcard again
            yield return nextWordWait;
            timeObject.gameObject.SetActive(true);

            //After another delay of the same length, enable button again
            yield return nextWordWait;
            nextWordBtn.interactable = true;
        }

        /// <summary>
        /// This method is subscribed to the "Next" button on screen, and calls relevant methods
        /// based on the state of the current flashcard and the overall game. If the game can keep
        /// going, reset card, increment the currently active word int and display the next word.
        /// If not, end game without doing anything.
        /// </summary>
        private void NextTimeWord()
        {
            if (timeObject.state == FlashCardBase.State.Flipping) return;
            if (activeWordIndex + 1 >= timeWords.Length)
            {
                EndGame();
                return;
            }
            timeObject.ResetToFinnishSide();
            activeWordIndex++;
            StartCoroutine(DisplayTimeWord());
        }

        #endregion

        #region number related methods

        /// <summary>
        /// This method updates all of the number flashcard's text fields to match the current word
        /// </summary>
        private void DisplayCurrentNumberWord()
        {
            Debug.Log(activeWordIndex);
            numberObject.wordFinnishText.text = numberWords[activeWordIndex].finnishWord;
            numberObject.wordSwedishBaseText.text = numberWords[activeWordIndex].swedishWord;

            numberObject.SetInitialElements(numberWords[activeWordIndex].lightModeSprite, numberWords[activeWordIndex].darkModeSprite);
        }

        /// <summary>
        /// This method sets up and starts the number word game. Enables relevant game objects, populates the
        /// <seealso cref="numberWords"/> array with number words to be included in the game.
        /// </summary>
        /// <param name="_numberWords">This parameter is used to populate the numberWords array</param>
        public void StartNumberGame(NumberWord[] _numberWords)
        {
            //Enable relevant objects
            gameObject.SetActive(true);
            numberObject.gameObject.SetActive(true);

            //Set initial colors
            if (UIManager.instance.LightmodeOn)
                numberObject.LightsOn();
            else
                numberObject.LightsOff();

            //(Re)set variables
            numberWords = _numberWords;
            activeWordIndex = 0;

            //Add relevant listeners to game and UI events
            nextWordBtn.onClick.AddListener(NextNumberWord);
            UIManager.instance.LightmodeOnEvent += DisplayCurrentNumberWord;
            UIManager.instance.LightmodeOffEvent += DisplayCurrentNumberWord;

            //Start displaying words and reset the flash card to the finnish side if it was flipped
            StartCoroutine(DisplayNumberWord());
            numberObject.ResetToFinnishSide();
        }

        /// <summary>
        /// After a short delay determined by <see cref="nextWordDelay"/>, shows the next flash card
        /// </summary>
        /// <returns></returns>
        private IEnumerator DisplayNumberWord()
        {
            //Disable button, hide the flashcard and populate its text fields
            nextWordBtn.interactable = false;
            numberObject.gameObject.SetActive(false);
            DisplayCurrentNumberWord();

            //After a delay, display the flashcard again
            yield return nextWordWait;
            numberObject.gameObject.SetActive(true);

            //After another delay of the same length, enable button again
            yield return nextWordWait;
            nextWordBtn.interactable = true;
        }

        /// <summary>
        /// This method is subscribed to the "Next" button on screen, and calls relevant methods
        /// based on the state of the current flashcard and the overall game. If the game can keep
        /// going, reset card, increment the currently active word int and display the next word.
        /// If not, end game without doing anything.
        /// </summary>
        private void NextNumberWord()
        {
            if (numberObject.state == FlashCardBase.State.Flipping) return;
            if (activeWordIndex + 1 >= numberWords.Length)
            {
                EndGame();
                return;
            }
            numberObject.ResetToFinnishSide();
            activeWordIndex++;
            StartCoroutine(DisplayNumberWord());
        }

        #endregion

        #region grammar related methods

        /// <summary>
        /// This method updates all of the grammar flashcard's text fields to match the current word
        /// </summary>
        private void DisplayCurrentGrammarWord()
        {
            Debug.Log(activeWordIndex);
            grammarObject.wordFinnishText.text = grammarWords[activeWordIndex].finnishWord;
            grammarObject.wordSwedishBaseText.text = grammarWords[activeWordIndex].swedishWord;
            grammarObject.wordNoticeText.text = grammarWords[activeWordIndex].notices;
            if (grammarObject.wordNoticeText.text == "") grammarObject.infoHover.gameObject.SetActive(false);
            else grammarObject.infoHover.gameObject.SetActive(true);

            grammarObject.SetInitialElements(grammarWords[activeWordIndex].lightModeSprite, grammarWords[activeWordIndex].darkModeSprite);
        }

        /// <summary>
        /// This method sets up and starts the grammar word game. Enables relevant game objects, populates the
        /// <seealso cref="grammarWords"/> array with grammar words to be included in the game.
        /// </summary>
        /// <param name="_grammarWords">This parameter is used to populate the grammarWords array</param>
        public void StartGrammarGame(GrammarWord[] _grammarWords)
        {
            //Enable relevant objects
            gameObject.SetActive(true);
            grammarObject.gameObject.SetActive(true);

            //Set initial colors
            if (UIManager.instance.LightmodeOn)
                grammarObject.LightsOn();
            else
                grammarObject.LightsOff();

            //(Re)set variables
            grammarWords = _grammarWords;
            activeWordIndex = 0;

            //Add relevant listeners to game and UI events
            nextWordBtn.onClick.AddListener(NextGrammarWord);
            UIManager.instance.LightmodeOnEvent += DisplayCurrentGrammarWord;
            UIManager.instance.LightmodeOffEvent += DisplayCurrentGrammarWord;

            //Start displaying words and reset the flash card to the finnish side if it was flipped
            StartCoroutine(DisplayGrammarWord());
            grammarObject.ResetToFinnishSide();
        }

        /// <summary>
        /// After a short delay determined by <see cref="nextWordDelay"/>, shows the next flash card
        /// </summary>
        /// <returns></returns>
        private IEnumerator DisplayGrammarWord()
        {
            //Disable button, hide the flashcard and populate its text fields
            nextWordBtn.interactable = false;
            grammarObject.gameObject.SetActive(false);
            grammarObject.HideInfo();
            DisplayCurrentGrammarWord();

            //After a delay, display the flashcard again
            yield return nextWordWait;
            grammarObject.gameObject.SetActive(true);

            //After another delay of the same length, enable button again
            yield return nextWordWait;
            nextWordBtn.interactable = true;
        }

        /// <summary>
        /// This method is subscribed to the "Next" button on screen, and calls relevant methods
        /// based on the state of the current flashcard and the overall game. If the game can keep
        /// going, reset card, increment the currently active word int and display the next word.
        /// If not, end game without doing anything.
        /// </summary>
        private void NextGrammarWord()
        {
            if (grammarObject.state == FlashCardBase.State.Flipping) return;
            if (activeWordIndex + 1 >= grammarWords.Length)
            {
                EndGame();
                return;
            }
            grammarObject.ResetToFinnishSide();
            activeWordIndex++;
            StartCoroutine(DisplayGrammarWord());
        }

        #endregion

        #region pronoun related methods

        /// <summary>
        /// This method updates all of the pronoun flashcard's text fields to match the current word
        /// </summary>
        private void DisplayCurrentPronounWord()
        {
            Debug.Log(activeWordIndex);
            pronounObject.wordFinnishText.text = pronounWords[activeWordIndex].finnishWord;
            pronounObject.wordSwedishBaseText.text = pronounWords[activeWordIndex].swedishWord;
            pronounObject.wordSwedishPossessiveEn.text = pronounWords[activeWordIndex].pronounPossessiveEnSwe;
            pronounObject.wordSwedishPossessiveEtt.text = pronounWords[activeWordIndex].pronounPossessiveEttSwe;
            pronounObject.wordSwedishPossessivePlural.text = pronounWords[activeWordIndex].pronounPossessivePluralSwe;
            pronounObject.wordSwedishObject.text = pronounWords[activeWordIndex].pronounObjectSwe;
            pronounObject.wordFinnishPossessive.text = pronounWords[activeWordIndex].pronounPossessiveFin;
            pronounObject.wordFinnishObject.text = pronounWords[activeWordIndex].pronounObjectFin;

            pronounObject.SetInitialElements(pronounWords[activeWordIndex].lightModeSprite, pronounWords[activeWordIndex].darkModeSprite);
        }

        /// <summary>
        /// This method sets up and starts the pronoun word game. Enables relevant game objects, populates the
        /// <seealso cref="pronounWords"/> array with pronoun words to be included in the game.
        /// </summary>
        /// <param name="_pronounWords">This parameter is used to populate the pronounWords array</param>
        public void StartPronounGame(PronounWord[] _pronounWords)
        {
            //Enable relevant objects
            gameObject.SetActive(true);
            pronounObject.gameObject.SetActive(true);

            //Set initial colors
            if (UIManager.instance.LightmodeOn)
                pronounObject.LightsOn();
            else
                pronounObject.LightsOff();

            //(Re)set variables
            pronounWords = _pronounWords;
            activeWordIndex = 0;

            //Add relevant listeners to game and UI events
            nextWordBtn.onClick.AddListener(NextPronounWord);
            UIManager.instance.LightmodeOnEvent += DisplayCurrentPronounWord;
            UIManager.instance.LightmodeOffEvent += DisplayCurrentPronounWord;

            //Start displaying words and reset the flash card to the finnish side if it was flipped
            StartCoroutine(DisplayPronounWord());
            pronounObject.ResetToFinnishSide();
        }

        /// <summary>
        /// After a short delay determined by <see cref="nextWordDelay"/>, shows the next flash card
        /// </summary>
        /// <returns></returns>
        private IEnumerator DisplayPronounWord()
        {
            //Disable button, hide the flashcard and populate its text fields
            nextWordBtn.interactable = false;
            pronounObject.gameObject.SetActive(false);
            DisplayCurrentPronounWord();

            //After a delay, display the flashcard again
            yield return nextWordWait;
            pronounObject.gameObject.SetActive(true);

            //After another delay of the same length, enable button again
            yield return nextWordWait;
            nextWordBtn.interactable = true;
        }

        /// <summary>
        /// This method is subscribed to the "Next" button on screen, and calls relevant methods
        /// based on the state of the current flashcard and the overall game. If the game can keep
        /// going, reset card, increment the currently active word int and display the next word.
        /// If not, end game without doing anything.
        /// </summary>
        private void NextPronounWord()
        {
            if (pronounObject.state == FlashCardBase.State.Flipping) return;
            if (activeWordIndex + 1 >= pronounWords.Length)
            {
                EndGame();
                return;
            }
            pronounObject.ResetToFinnishSide();
            activeWordIndex++;
            StartCoroutine(DisplayPronounWord());
        }

        #endregion

        #region phrase related methods

        /// <summary>
        /// This method updates all of the phrase flashcard's text fields to match the current word
        /// </summary>
        private void DisplayCurrentPhraseWord()
        {
            Debug.Log(activeWordIndex);
            phraseObject.wordFinnishText.text = phraseWords[activeWordIndex].finnishWord;
            phraseObject.wordSwedishBaseText.text = phraseWords[activeWordIndex].swedishWord;

            phraseObject.SetInitialElements(phraseWords[activeWordIndex].lightModeSprite, phraseWords[activeWordIndex].darkModeSprite);
        }

        /// <summary>
        /// This method sets up and starts the phrase word game. Enables relevant game objects, populates the
        /// <seealso cref="phraseWords"/> array with phrase words to be included in the game.
        /// </summary>
        /// <param name="_phraseWords">This parameter is used to populate the phraseWords array</param>
        public void StartPhraseGame(Word[] _phraseWords)
        {
            //Enable relevant objects
            gameObject.SetActive(true);
            phraseObject.gameObject.SetActive(true);

            //Set initial colors
            if (UIManager.instance.LightmodeOn)
                phraseObject.LightsOn();
            else
                phraseObject.LightsOff();

            //(Re)set variables
            phraseWords = _phraseWords;
            activeWordIndex = 0;

            //Add relevant listeners to game and UI events
            nextWordBtn.onClick.AddListener(NextPhraseWord);
            UIManager.instance.LightmodeOnEvent += DisplayCurrentPhraseWord;
            UIManager.instance.LightmodeOffEvent += DisplayCurrentPhraseWord;

            //Start displaying words and reset the flash card to the finnish side if it was flipped
            StartCoroutine(DisplayPhraseWord());
            phraseObject.ResetToFinnishSide();
        }

        /// <summary>
        /// After a short delay determined by <see cref="nextWordDelay"/>, shows the next flash card
        /// </summary>
        /// <returns></returns>
        private IEnumerator DisplayPhraseWord()
        {
            //Disable button, hide the flashcard and populate its text fields
            nextWordBtn.interactable = false;
            phraseObject.gameObject.SetActive(false);
            DisplayCurrentPhraseWord();

            //After a delay, display the flashcard again
            yield return nextWordWait;
            phraseObject.gameObject.SetActive(true);

            //After another delay of the same length, enable button again
            yield return nextWordWait;
            nextWordBtn.interactable = true;
        }

        /// <summary>
        /// This method is subscribed to the "Next" button on screen, and calls relevant methods
        /// based on the state of the current flashcard and the overall game. If the game can keep
        /// going, reset card, increment the currently active word int and display the next word.
        /// If not, end game without doing anything.
        /// </summary>
        private void NextPhraseWord()
        {
            if (phraseObject.state == FlashCardBase.State.Flipping) return;
            if (activeWordIndex + 1 >= phraseWords.Length)
            {
                EndGame();
                return;
            }
            phraseObject.ResetToFinnishSide();
            activeWordIndex++;
            StartCoroutine(DisplayPhraseWord());
        }

        #endregion

        #region generic methods

        private void EndGame()
        {
            nounWords = null;
            nounObject.gameObject.SetActive(false);
            verbObject.gameObject.SetActive(false);
            adjectiveObject.gameObject.SetActive(false);
            timeObject.gameObject.SetActive(false);
            numberObject.gameObject.SetActive(false);
            grammarObject.gameObject.SetActive(false);
            pronounObject.gameObject.SetActive(false);
            pronounObject.gameObject.SetActive(false);
            nextWordBtn.onClick.RemoveAllListeners();
            UIManager.instance.LightmodeOnEvent -= DisplayCurrentNoun;
            UIManager.instance.LightmodeOffEvent -= DisplayCurrentNoun;
            StartCoroutine(GameEndDelay());
        }

        private IEnumerator GameEndDelay()
        {
            yield return gameEndWait;
            gameObject.SetActive(false);
        }
        
        #endregion
    }
}
