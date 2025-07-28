using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SwedishApp.Core;
using SwedishApp.Input;
using SwedishApp.UI;
using SwedishApp.Words;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SwedishApp.Minigames
{
    /// <summary>
    /// This class will house methods and variables related to running BOTH of the translate minigames.
    /// </summary>
    public class TranslateMinigame : MonoBehaviour
    {
        public enum GameMode
        {
            ToSwedish = 0,
            ToFinnish = 1
        }

        [Header("Input related")]
        [SerializeField] private InputReader inputReader;

        private GameMode? gameMode = null;
        private Queue<Word> words;
        private Word currentWord = null;
        private bool wordWasChecked = false;
        private bool canDeleteWord = false;

        [Header("Game variables")]
        [SerializeField] private float nextWordDelayTime = 1.5f;
        [SerializeField] private int allowedMissedInputsCount = 2;
        private int score = 0;
        private int activeGameMaxPoints = 0;
        private string activeWordNoHighlight;

        [Header("UI related")]
        [SerializeField] private Button abortGameButton;
        [SerializeField] private GameObject translateMinigameBG;
        [SerializeField] private Button checkWordButton;
        [SerializeField] private Button nextWordButton;
        [SerializeField] private TextMeshProUGUI checkWordTxt;
        [SerializeField] private TextMeshProUGUI nextWordTxt;
        [SerializeField] private GameObject wordInputFieldHolderPrefab;
        [SerializeField] private GameObject wordLetterInputPrefab;
        [SerializeField] private TextMeshProUGUI wordToTranslateText;
        private Transform wordInputFieldHolder;
        private InputFieldHandling inputFieldHandler;
        private List<TMP_InputField> wordLetterInputFields;
        private List<TextMeshProUGUI> letterTextRefs;

        //Events
        public event Action WordCorrectEvent;
        public event Action WordIncorrectEvent;

        #region unity default methods

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //Subscribe to various events
            checkWordButton.onClick.AddListener(CheckWord);
            nextWordButton.onClick.AddListener(DeleteOldWord);
            abortGameButton.onClick.AddListener(AbortGame);
        }

        #endregion

        #region translate game handling

        /// <summary>
        /// This method sets up relevant variables required to start a game and calls the method to set up the first word
        /// </summary>
        /// <param name="_wordType">This tells the script what word type will be used for the game</param>
        /// <param name="_words">Give a list of Word-objects as parameter, this is used as the word list for the translation game</param>
        public void StartGame(GameMode _gameMode, List<Word> _words)
        {
            translateMinigameBG.SetActive(true);
            gameMode = _gameMode;
            words = new(_words.ToArray());
            activeGameMaxPoints = words.Count;

            //Buttons' sprites are set according to if light mode is on
            abortGameButton.image.sprite = UIManager.instance.LightmodeOn ? UIManager.instance.abortSpriteLightmode : UIManager.instance.abortSpriteDarkmode;
            checkWordButton.image.sprite = UIManager.instance.LightmodeOn ? UIManager.instance.buttonSpriteLightmode : UIManager.instance.buttonSpriteDarkmode;
            nextWordButton.image.sprite = UIManager.instance.LightmodeOn ? UIManager.instance.buttonSpriteLightmode : UIManager.instance.buttonSpriteDarkmode;
            //Text colors as well
            checkWordTxt.color = UIManager.instance.LightmodeOn ? UIManager.instance.Darkgrey : UIManager.instance.Lightgrey;
            nextWordTxt.color = UIManager.instance.LightmodeOn ? UIManager.instance.Darkgrey : UIManager.instance.Lightgrey;

            //And make abort button react to light mode changes!
            UIManager.instance.LightmodeOnEvent += ToLightmode;
            UIManager.instance.LightmodeOffEvent += ToDarkmode;
            inputReader.SubmitEventCancelled += CheckWord;
            inputReader.SubmitEventHeld += DeleteOldWord;
            SetupNewWord();
        }

        /// <summary>
        /// This method is used to check whether the player has placed correct letters in each input field of the word
        /// </summary>
        private void CheckWord()
        {
            bool wordWasCorrect = false;
            canDeleteWord = true;
            int correctLettersCount = 0;
            int missedInputsCount = 0;

            //Translate given answer to lowercase to ease checking
            List<char> chars = new();
            for (int i = 0; i < wordLetterInputFields.Count; i++)
            {
                if (wordLetterInputFields[i].text != "") chars.Add(wordLetterInputFields[i].text[0]);
                else chars.Add(' ');
            }
            string givenString = new(chars.ToArray());
            givenString.ToLower();

            int j = 0;

            //For every letter in the word, set a highlight depending if the letter was correct or not
            for (int i = 0; i < activeWordNoHighlight.Length; i++)
            {
                if (activeWordNoHighlight[i] == ' ')
                {
                    correctLettersCount++;
                    j++;
                    continue;
                }
                else if (wordLetterInputFields[j].text == "")
                {
                    missedInputsCount++;
                    continue;
                }
                else if (givenString[j] == activeWordNoHighlight[i])
                {
                    //This is the 'correct' indicator
                    wordLetterInputFields[j].transform.GetChild(0).gameObject.SetActive(true);
                    wordLetterInputFields[j].transform.GetChild(1).gameObject.SetActive(false);
                    correctLettersCount++;
                }
                else
                {
                    //This is the 'incorrect' indicator
                    wordLetterInputFields[j].transform.GetChild(0).gameObject.SetActive(false);
                    wordLetterInputFields[j].transform.GetChild(1).gameObject.SetActive(true);
                }

                j++;
            }
            if (correctLettersCount == activeWordNoHighlight.Length) wordWasCorrect = true;

            if (wordWasCorrect)
            {
                //DO A LITTLE THING IF WORD WAS CORRECT!!!
                WordCorrectEvent?.Invoke();
                AudioManager.Instance.PlayCorrect();
                score++;
            }
            else
            {
                WordIncorrectEvent?.Invoke();
                AudioManager.Instance.PlayIncorrect();
            }

            wordLetterInputFields[0].Select();

            if (missedInputsCount <= allowedMissedInputsCount)
            {
                wordWasChecked = true;
                nextWordButton.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// This is ran when starting a new game or after the player completes a word and the relevant list of words has entries remaining.
        /// </summary>
        private void SetupNewWord()
        {
            currentWord = words.Dequeue();
            wordLetterInputFields = new();
            letterTextRefs = new();
            wordWasChecked = false;

            //Setup a new holder for all the individual input fields
            wordInputFieldHolder = Instantiate(wordInputFieldHolderPrefab, translateMinigameBG.transform).transform;
            inputFieldHandler = wordInputFieldHolder.GetComponent<InputFieldHandling>();
            string wordToTranslate = gameMode == GameMode.ToSwedish ? currentWord.finnishWord : currentWord.swedishWord;
            activeWordNoHighlight = gameMode == GameMode.ToSwedish ? CleanWord(currentWord.swedishWord) : CleanWord(currentWord.finnishWord);

            wordToTranslateText.text = wordToTranslate;

            //Create an input field for every letter of the word
            for (int i = 0; i < activeWordNoHighlight.Length; i++)
            {
                int indexHolder = i;
                wordLetterInputFields.Add(Instantiate(wordLetterInputPrefab, wordInputFieldHolder).GetComponent<TMP_InputField>());

                //Add listeners to input fields. These are used for navigating between each input field of the word
                wordLetterInputFields[i].onValueChanged.AddListener((s) => inputFieldHandler.GoNextField());
                wordLetterInputFields[i].onSelect.AddListener((s) => inputFieldHandler.GetActiveIndex(indexHolder));

                //If the letter in the word is a space, disable visuals and make input field unable to be interacted with
                if (activeWordNoHighlight[i] == ' ')
                {
                    wordLetterInputFields[i].image.enabled = false;
                    wordLetterInputFields[i].interactable = false;
                }

                //Save references to the text slot of each input field, used when changing font settings!
                letterTextRefs.Add(wordLetterInputFields[i].transform.Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>());

                //Set initial input field background and font colors based on if light mode is enabled or not
                FieldToRightColors(i);
            }
            UIManager.instance.LegibleModeOnEvent += SwapFieldsToLegibleFont;
            UIManager.instance.LegibleModeOffEvent += SwapFieldsToBasicFont;
            wordLetterInputFields[0].Select();
        }

        /// <summary>
        /// Ran when going to next word. Clears word-related variables and unsubscribes events related to fonts
        /// </summary>
        private void DeleteOldWord()
        {
            if (!wordWasChecked) return;
            if (!canDeleteWord) return;

            canDeleteWord = false;
            wordLetterInputFields.Clear();
            letterTextRefs.Clear();
            Destroy(wordInputFieldHolder.gameObject);
            nextWordButton.gameObject.SetActive(false);
            UIManager.instance.LegibleModeOnEvent -= SwapFieldsToLegibleFont;
            UIManager.instance.LegibleModeOffEvent -= SwapFieldsToBasicFont;
            wordToTranslateText.text = "";

            StartCoroutine(DelayBeforeNewWord());
        }

        /// <summary>
        /// This delays the appearance of the new word by a set amount, should help with juiciness later
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelayBeforeNewWord()
        {
            //hit a particle effect or some other thing if wordWasCorrect
            //AudioManager.Instance.PlayMenuSelect3();    // Probably just a temporary thing to signal that the game isn't lagging
            yield return new WaitForSeconds(nextWordDelayTime);

            if (words.Count > 0)
            {
                SetupNewWord();
            }
            else
            {
                CompleteGame();
            }
        }

        /// <summary>
        /// This handles whatever we decide to do when a game is completed
        /// </summary>
        private void CompleteGame()
        {
            Debug.Log("game completed yippee");
            translateMinigameBG.SetActive(false);

            //Unsubscribe events
            UIManager.instance.LegibleModeOnEvent -= SwapFieldsToLegibleFont;
            UIManager.instance.LegibleModeOffEvent -= SwapFieldsToBasicFont;
            UIManager.instance.LightmodeOnEvent -= ToLightmode;
            UIManager.instance.LightmodeOffEvent -= ToDarkmode;
            inputReader.SubmitEventCancelled -= CheckWord;
            inputReader.SubmitEventHeld -= DeleteOldWord;
        }

        /// <summary>
        /// This method interrupts the game without finishing it. Events are unsubscribed, variables cleared,
        /// word holder is destroyed, and the minigame screen is set inactive.
        /// </summary>
        private void AbortGame()
        {
            Debug.Log("game aborted");

            //Unsubscribe events
            UIManager.instance.LegibleModeOnEvent -= SwapFieldsToLegibleFont;
            UIManager.instance.LegibleModeOffEvent -= SwapFieldsToBasicFont;
            UIManager.instance.LightmodeOnEvent -= ToLightmode;
            UIManager.instance.LightmodeOffEvent -= ToDarkmode;
            inputReader.SubmitEventCancelled -= CheckWord;
            inputReader.SubmitEventHeld -= DeleteOldWord;

            //Clear variables
            words = new();
            currentWord = new();
            wordToTranslateText.text = "";
            score = 0;
            activeGameMaxPoints = 0;
            wordLetterInputFields.Clear();
            letterTextRefs.Clear();

            //Destroy word object, disable translate minigame ui
            Destroy(wordInputFieldHolder.gameObject);
            translateMinigameBG.SetActive(false);
        }

        private string CleanWord(string _wordToClean)
        {
            List<char> chars = new();
            bool ignoreLetters = false;
            string cleanedWord = _wordToClean.Replace(@"\u00AD", null);

            foreach (char c in cleanedWord)
            {
                if (c == '<')
                {
                    ignoreLetters = true;
                    continue;
                }
                else if (c == '>')
                {
                    ignoreLetters = false;
                    continue;
                }
                if (ignoreLetters) continue;

                chars.Add(c);
            }

            return new(chars.ToArray());
        }

        #endregion

        #region font related

        /// <summary>
        /// This method handles changing the word's input fields to the activated font
        /// </summary>
        private void SwapFieldsToLegibleFont()
        {
            foreach (TMP_InputField inputField in wordLetterInputFields)
            {
                inputField.fontAsset = UIManager.instance.legibleFont;
            }
        }

        /// <summary>
        /// This method handles changing the word's input fields to the activated font
        /// </summary>
        private void SwapFieldsToBasicFont()
        {
            foreach (TMP_InputField inputField in wordLetterInputFields)
            {
                inputField.fontAsset = UIManager.instance.basicFont;
            }
        }

        #endregion

        #region lightmode related

        /// <summary>
        /// This method handles changing all elements related to this minigame to lightmode
        /// </summary>
        private void ToLightmode()
        {
            abortGameButton.image.sprite = UIManager.instance.abortSpriteLightmode;
            checkWordButton.image.sprite = UIManager.instance.buttonSpriteLightmode;
            nextWordButton.image.sprite = UIManager.instance.buttonSpriteLightmode;
            checkWordTxt.color = UIManager.instance.Darkgrey;
            nextWordTxt.color = UIManager.instance.Darkgrey;

            //Input fields
            for (int i = 0; i < wordLetterInputFields.Count; i++)
            {
                var colorBlock = wordLetterInputFields[i].colors;
                colorBlock.normalColor = UIManager.instance.Darkgrey;
                colorBlock.selectedColor = UIManager.instance.LightmodeHighlight;
                colorBlock.highlightedColor = UIManager.instance.DarkgreyLighter;
                colorBlock.pressedColor = UIManager.instance.DarkgreyLighter;
                colorBlock.disabledColor = UIManager.instance.DarkgreyHalfAlpha;
                wordLetterInputFields[i].colors = colorBlock;
                letterTextRefs[i].color = UIManager.instance.Lightgrey;
            }
        }

        /// <summary>
        /// This method handles changing all elements related to this minigame to darkmode
        /// </summary>
        private void ToDarkmode()
        {
            abortGameButton.image.sprite = UIManager.instance.abortSpriteDarkmode;
            checkWordButton.image.sprite = UIManager.instance.buttonSpriteDarkmode;
            nextWordButton.image.sprite = UIManager.instance.buttonSpriteDarkmode;
            checkWordTxt.color = UIManager.instance.Lightgrey;
            nextWordTxt.color = UIManager.instance.Lightgrey;

            //Input fields
            for (int i = 0; i < wordLetterInputFields.Count; i++)
            {
                var colorBlock = wordLetterInputFields[i].colors;
                colorBlock.normalColor = UIManager.instance.Lightgrey;
                colorBlock.selectedColor = UIManager.instance.DarkmodeHighlight;
                colorBlock.highlightedColor = UIManager.instance.LightgreyDarker;
                colorBlock.pressedColor = UIManager.instance.LightgreyDarker;
                colorBlock.disabledColor = UIManager.instance.LightgreyHalfAlpha;
                wordLetterInputFields[i].colors = colorBlock;
                letterTextRefs[i].color = UIManager.instance.Darkgrey;
            }
        }

        private void FieldToRightColors(int _i)
        {

            if (UIManager.instance.LightmodeOn)
            {
                var colorBlock = wordLetterInputFields[_i].colors;
                colorBlock.normalColor = UIManager.instance.Darkgrey;
                colorBlock.selectedColor = UIManager.instance.LightmodeHighlight;
                colorBlock.highlightedColor = UIManager.instance.DarkgreyLighter;
                colorBlock.pressedColor = UIManager.instance.DarkgreyLighter;
                wordLetterInputFields[_i].colors = colorBlock;
                letterTextRefs[_i].color = UIManager.instance.Lightgrey;
            }
            else
            {
                var colorBlock = wordLetterInputFields[_i].colors;
                colorBlock.normalColor = UIManager.instance.Lightgrey;
                colorBlock.selectedColor = UIManager.instance.DarkmodeHighlight;
                colorBlock.highlightedColor = UIManager.instance.LightgreyDarker;
                colorBlock.pressedColor = UIManager.instance.LightgreyDarker;
                wordLetterInputFields[_i].colors = colorBlock;
                letterTextRefs[_i].color = UIManager.instance.Darkgrey;
            }
        }

        #endregion
    }
}