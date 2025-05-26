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
        private int activeWordIndex = 0;

        [Header("Set delays related to game flow")]
        [SerializeField] private float nextWordDelay = 0.5f;
        [SerializeField] private float gameEndDelay = 1.0f;

        [Header("UI holders for flash card bases")]
        [SerializeField] private FlashCardNoun nounObject;
        [SerializeField] private FlashCardVerb verbObject;
        [SerializeField] private FlashCardAdjective adjectiveObject;

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

            //Set initial colors
            if (UIManager.instance.LightmodeOn)
                nounObject.LightsOn();
            else
                nounObject.LightsOff();

            //(Re)set variables
            nounWords = _nounWords;
            activeWordIndex = 0;

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

        #region generic methods

        private void EndGame()
        {
            nounWords = null;
            nounObject.gameObject.SetActive(false);
            verbObject.gameObject.SetActive(false);
            adjectiveObject.gameObject.SetActive(false);
            nextWordBtn.onClick.RemoveAllListeners();
            UIManager.instance.LightmodeOnEvent -= DisplayCurrentNoun;
            UIManager.instance.LightmodeOffEvent -= DisplayCurrentNoun;
            StartCoroutine(GameEndDelay());
        }

        private IEnumerator GameEndDelay()
        {
            yield return new WaitForSeconds(gameEndDelay);
            gameObject.SetActive(false);
        }
        
        #endregion
    }
}
