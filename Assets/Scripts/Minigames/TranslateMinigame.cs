using System;
using System.Collections;
using System.Collections.Generic;
using SwedishApp.Core;
using SwedishApp.Input;
using SwedishApp.Meta;
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
        private List<Word> wordsToImprove;
        private Word currentWord = null;

        [Header("Game variables")]
        [Tooltip("Value between 0 and 1; percentage")]
        [SerializeField] private float goodScoreThreshold = 0.5f;
        [SerializeField] private float nextWordDelayTime = 1.5f;
        [SerializeField] private float allowedMissedLettersPercentage = 0.9f;
        private bool wordWasChecked = false;
        private bool wordWasCorrect = false;
        private bool canDeleteWord = false;
        private bool gotScoreForWord = false;
        private int score = 0;
        private int playedWordsCount = -1;
        private int activeGameWordCount = 0;
        private string activeWordNoHighlight;
        [SerializeField] private AudioClip correctClip;
        [SerializeField] private AudioClip incorrectClip;

        [Header("UI related")]
        [SerializeField] private Button abortGameButton;
        [SerializeField] private GameObject translateMinigameBG;
        [SerializeField] private Transform wordAnchor;
        [SerializeField] private Button checkWordButton;
        [SerializeField] private Button nextWordButton;
        [SerializeField] private TextMeshProUGUI checkWordTxt;
        [SerializeField] private TextMeshProUGUI nextWordTxt;
        [SerializeField] private GameObject wordInputFieldHolderPrefab;
        [SerializeField] private GameObject wordLetterInputPrefab;
        [SerializeField] private TextMeshProUGUI wordToTranslateText;
        [SerializeField] private TextMeshProUGUI translatedCounter;
        [SerializeField] private TextMeshProUGUI correctCounter;
        private Transform wordInputFieldHolder;
        private InputFieldHandling inputFieldHandler;
        private List<TMP_InputField> wordLetterInputFields;
        private List<TextMeshProUGUI> letterTextRefs;

        //Events
        public event Action WordCorrectEvent;
        public event Action WordIncorrectEvent;

        [Header("Tutorials")]
        [SerializeField] private TutorialHandler initialTutorial;
        [SerializeField] private TutorialHandler nextWordTutorial;
        [SerializeField] private TutorialHandler incorrectTutorial;

        #region unity default methods

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //Subscribe to various events
            checkWordButton.onClick.AddListener(CheckWord);
            nextWordButton.onClick.AddListener(DeleteOldWord);
            abortGameButton.onClick.AddListener(AbortGame);
            wordToTranslateText.RegisterDirtyLayoutCallback(() => UIManager.Instance.FixTextSpacing(wordToTranslateText));
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
            gameObject.SetActive(true);
            gameMode = _gameMode;
            words = new(_words.ToArray());
            wordsToImprove = new();
            activeGameWordCount = words.Count;
            correctCounter.text = "0";
            score = 0;
            correctCounter.text = score.ToString();
            playedWordsCount = -1;
            initialTutorial.gameObject.SetActive(false);
            nextWordTutorial.gameObject.SetActive(false);
            incorrectTutorial.gameObject.SetActive(false);

            //Buttons' sprites are set according to if light mode is on
            abortGameButton.image.sprite = UIManager.Instance.LightmodeOn ? UIManager.Instance.AbortSpriteLightmode : UIManager.Instance.AbortSpriteDarkmode;
            checkWordButton.image.sprite = UIManager.Instance.LightmodeOn ? UIManager.Instance.ButtonSpriteLightmode : UIManager.Instance.ButtonSpriteDarkmode;
            nextWordButton.image.sprite = UIManager.Instance.LightmodeOn ? UIManager.Instance.ButtonSpriteLightmode : UIManager.Instance.ButtonSpriteDarkmode;
            //Text colors as well
            checkWordTxt.color = UIManager.Instance.LightmodeOn ? UIManager.Instance.Darkgrey : UIManager.Instance.Lightgrey;
            nextWordTxt.color = UIManager.Instance.LightmodeOn ? UIManager.Instance.Darkgrey : UIManager.Instance.Lightgrey;

            //And make abort button react to light mode changes!
            UIManager.Instance.LightmodeOnEvent += ToLightmode;
            UIManager.Instance.LightmodeOffEvent += ToDarkmode;
            inputReader.SubmitEventCancelled += CheckWord;
            inputReader.SubmitEventHeld += DeleteOldWord;
            SetupNewWord();
        }

        /// <summary>
        /// This method is used to check whether the player has placed correct letters in each input field of the word
        /// </summary>
        private void CheckWord()
        {
            if (!checkWordButton.interactable) return;
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
            givenString = givenString.ToLower();

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
                AudioManager.Instance.PlayClip(correctClip);
                if (!gotScoreForWord)
                {
                    score++;
                    gotScoreForWord = true;
                }
            }
            else
            {
                if (!incorrectTutorial.TutorialSeen()) incorrectTutorial.ShowTutorial();
                WordIncorrectEvent?.Invoke();
                AudioManager.Instance.PlayClip(incorrectClip);
            }

            wordLetterInputFields[0].Select();
            correctCounter.text = score.ToString();

            int allowedMissedInputsCount = Mathf.RoundToInt((float)activeWordNoHighlight.Length * (1f - allowedMissedLettersPercentage));

            if (wordWasCorrect || missedInputsCount <= allowedMissedInputsCount)
            {
                wordWasChecked = true;
                nextWordButton.gameObject.SetActive(true);
            }

            if (!nextWordTutorial.TutorialSeen()) nextWordTutorial.ShowTutorial();
        }

        /// <summary>
        /// This is ran when starting a new game or after the player completes a word and the relevant list of words has entries remaining.
        /// </summary>
        private void SetupNewWord()
        {
            currentWord = words.Dequeue();
            wordLetterInputFields = new();
            letterTextRefs = new();
            wordWasCorrect = false;
            wordWasChecked = false;
            gotScoreForWord = false;
            checkWordButton.interactable = true;
            playedWordsCount++;
            translatedCounter.text = string.Concat(playedWordsCount, "/", activeGameWordCount);

            if (!initialTutorial.TutorialSeen()) initialTutorial.ShowTutorial();

            //Setup a new holder for all the individual input fields
            wordInputFieldHolder = Instantiate(wordInputFieldHolderPrefab, wordAnchor).transform;
            inputFieldHandler = wordInputFieldHolder.GetComponent<InputFieldHandling>();
            string wordToTranslate = gameMode == GameMode.ToSwedish ? currentWord.finnishWord : currentWord.swedishWord;
            activeWordNoHighlight = gameMode == GameMode.ToSwedish ? Helpers.CleanWord(currentWord.swedishWord) : Helpers.CleanWord(currentWord.finnishWord);

            wordToTranslateText.text = wordToTranslate;
            UIManager.Instance.FixTextSpacing(wordToTranslateText);

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
            UIManager.Instance.LegibleModeOnEvent += SwapFieldsToLegibleFont;
            UIManager.Instance.LegibleModeOffEvent += SwapFieldsToBasicFont;
            wordLetterInputFields[0].Select();
        }

        /// <summary>
        /// Ran when going to next word. Clears word-related variables and unsubscribes events related to fonts
        /// </summary>
        private void DeleteOldWord()
        {
            if (!wordWasChecked)
            {
                CheckWord();
                return;
            }
            if (!canDeleteWord) return;

            canDeleteWord = false;
            checkWordButton.interactable = false;
            wordLetterInputFields.Clear();
            letterTextRefs.Clear();
            if (!wordWasCorrect) wordsToImprove.Add(currentWord);
            Destroy(wordInputFieldHolder.gameObject);
            nextWordButton.gameObject.SetActive(false);
            UIManager.Instance.LegibleModeOnEvent -= SwapFieldsToLegibleFont;
            UIManager.Instance.LegibleModeOffEvent -= SwapFieldsToBasicFont;
            wordToTranslateText.text = "";

            StartCoroutine(DelayBeforeNewWord());
        }

        /// <summary>
        /// This delays the appearance of the new word by a set amount, should help with juiciness later
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelayBeforeNewWord()
        {
            if (words.Count > 0)
            {
                yield return new WaitForSeconds(nextWordDelayTime);
                SetupNewWord();
            }
            else
            {
                CompleteGame();
                yield break;
            }
        }

        /// <summary>
        /// This handles whatever we decide to do when a game is completed
        /// </summary>
        private void CompleteGame()
        {
            UnsubscribeEvents();

            gameObject.SetActive(false);
            UIManager.Instance.TriggerTipChange();
            UIManager.Instance.ActivateMinigameEndscreen(_maxScore: activeGameWordCount, _realScore: score,
                _goodScoreThreshold: goodScoreThreshold, _wordsToImprove: wordsToImprove);
        }

        /// <summary>
        /// This method interrupts the game without finishing it. Events are unsubscribed, variables cleared,
        /// word holder is destroyed, and the minigame screen is set inactive.
        /// </summary>
        private void AbortGame()
        {
            UnsubscribeEvents();

            gameObject.SetActive(false);
            Destroy(wordInputFieldHolder.gameObject);
            UIManager.Instance.TriggerTipChange();
        }

        private void UnsubscribeEvents()
        {
            UIManager.Instance.LegibleModeOnEvent -= SwapFieldsToLegibleFont;
            UIManager.Instance.LegibleModeOffEvent -= SwapFieldsToBasicFont;
            UIManager.Instance.LightmodeOnEvent -= ToLightmode;
            UIManager.Instance.LightmodeOffEvent -= ToDarkmode;
            inputReader.SubmitEventCancelled -= CheckWord;
            inputReader.SubmitEventHeld -= DeleteOldWord;
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
                inputField.fontAsset = UIManager.Instance.LegibleFont;
            }
        }

        /// <summary>
        /// This method handles changing the word's input fields to the activated font
        /// </summary>
        private void SwapFieldsToBasicFont()
        {
            foreach (TMP_InputField inputField in wordLetterInputFields)
            {
                inputField.fontAsset = UIManager.Instance.BasicFont;
            }
        }

        #endregion

        #region lightmode related

        /// <summary>
        /// This method handles changing all elements related to this minigame to lightmode
        /// </summary>
        private void ToLightmode()
        {
            abortGameButton.image.sprite = UIManager.Instance.AbortSpriteLightmode;
            checkWordButton.image.sprite = UIManager.Instance.ButtonSpriteLightmode;
            nextWordButton.image.sprite = UIManager.Instance.ButtonSpriteLightmode;
            checkWordTxt.color = UIManager.Instance.Darkgrey;
            nextWordTxt.color = UIManager.Instance.Darkgrey;

            //Input fields
            for (int i = 0; i < wordLetterInputFields.Count; i++)
            {
                var colorBlock = wordLetterInputFields[i].colors;
                colorBlock.normalColor = UIManager.Instance.Darkgrey;
                colorBlock.selectedColor = UIManager.Instance.LightmodeHighlight;
                colorBlock.highlightedColor = UIManager.Instance.DarkgreyLighter;
                colorBlock.pressedColor = UIManager.Instance.DarkgreyLighter;
                colorBlock.disabledColor = UIManager.Instance.DarkgreyHalfAlpha;
                wordLetterInputFields[i].colors = colorBlock;
                letterTextRefs[i].color = UIManager.Instance.Lightgrey;
            }
        }

        /// <summary>
        /// This method handles changing all elements related to this minigame to darkmode
        /// </summary>
        private void ToDarkmode()
        {
            abortGameButton.image.sprite = UIManager.Instance.AbortSpriteDarkmode;
            checkWordButton.image.sprite = UIManager.Instance.ButtonSpriteDarkmode;
            nextWordButton.image.sprite = UIManager.Instance.ButtonSpriteDarkmode;
            checkWordTxt.color = UIManager.Instance.Lightgrey;
            nextWordTxt.color = UIManager.Instance.Lightgrey;

            //Input fields
            for (int i = 0; i < wordLetterInputFields.Count; i++)
            {
                var colorBlock = wordLetterInputFields[i].colors;
                colorBlock.normalColor = UIManager.Instance.Lightgrey;
                colorBlock.selectedColor = UIManager.Instance.DarkmodeHighlight;
                colorBlock.highlightedColor = UIManager.Instance.LightgreyDarker;
                colorBlock.pressedColor = UIManager.Instance.LightgreyDarker;
                colorBlock.disabledColor = UIManager.Instance.LightgreyHalfAlpha;
                wordLetterInputFields[i].colors = colorBlock;
                letterTextRefs[i].color = UIManager.Instance.Darkgrey;
            }
        }

        private void FieldToRightColors(int _i)
        {
            if (UIManager.Instance.LightmodeOn)
            {
                var colorBlock = wordLetterInputFields[_i].colors;
                colorBlock.normalColor = UIManager.Instance.Darkgrey;
                colorBlock.selectedColor = UIManager.Instance.LightmodeHighlight;
                colorBlock.highlightedColor = UIManager.Instance.DarkgreyLighter;
                colorBlock.pressedColor = UIManager.Instance.DarkgreyLighter;
                wordLetterInputFields[_i].colors = colorBlock;
                letterTextRefs[_i].color = UIManager.Instance.Lightgrey;
            }
            else
            {
                var colorBlock = wordLetterInputFields[_i].colors;
                colorBlock.normalColor = UIManager.Instance.Lightgrey;
                colorBlock.selectedColor = UIManager.Instance.DarkmodeHighlight;
                colorBlock.highlightedColor = UIManager.Instance.LightgreyDarker;
                colorBlock.pressedColor = UIManager.Instance.LightgreyDarker;
                wordLetterInputFields[_i].colors = colorBlock;
                letterTextRefs[_i].color = UIManager.Instance.Darkgrey;
            }
        }

        #endregion
    }
}