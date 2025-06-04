using System.Collections;
using System.Collections.Generic;
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
        private Stack<Word> words;
        private Word currentWord = null;
        private bool wordWasChecked = false;
        private bool canDeleteWord = false;

        [Header("Game variables")]
        [SerializeField] private float nextWordDelayTime = 1.5f;
        private int score = 0;
        private int activeGameMaxPoints = 0;

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

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //Subscribe to various events
            checkWordButton.onClick.AddListener(CheckWord);
            nextWordButton.onClick.AddListener(DeleteOldWord);
            abortGameButton.onClick.AddListener(AbortGame);
            inputReader.SubmitEventCancelled += CheckWord;
            inputReader.SubmitEventHeld += DeleteOldWord;
        }

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
            SetupNewWord();
        }

        /// <summary>
        /// This method is used to check whether the player has placed correct letters in each input field of the word
        /// </summary>
        private void CheckWord()
        {
            bool wordWasCorrect = false;
            wordWasChecked = true;
            canDeleteWord = true;
            int correctLettersCount = 0;
            int wordLetterCount = 0;

            //If the gamemode is set to translate into swedish
            if (gameMode == GameMode.ToSwedish)
            {
                //Grab the amount of valid letters in the swedish word
                foreach (char letter in currentWord.swedishWord)
                {
                    if (letter != ' ')
                        wordLetterCount++;
                }

                //Translate given answer to lowercase to ease checking
                List<char> chars = new();
                for (int i = 0; i < currentWord.swedishWord.Length; i++)
                {
                    chars.Add(wordLetterInputFields[i].text[0]);
                }
                string givenString = new(chars.ToArray());
                givenString.ToLower();

                //For every letter in the word, set a highlight depending if the letter was correct or not
                for (int i = 0; i < currentWord.swedishWord.Length; i++)
                {
                    if (currentWord.swedishWord[i] == ' ' || wordLetterInputFields[i].text == "") continue;
                    if (givenString[i] == currentWord.swedishWord[i])
                    {
                        //This is the 'correct' indicator
                        wordLetterInputFields[i].transform.GetChild(0).gameObject.SetActive(true);
                        correctLettersCount++;
                    }
                    else
                    {
                        //This is the 'incorrect' indicator
                        wordLetterInputFields[i].transform.GetChild(1).gameObject.SetActive(true);
                    }
                }
                if (correctLettersCount == currentWord.swedishWord.Length) wordWasCorrect = true;
            }
            //If the gamemode is set to translate into finnish
            else if (gameMode == GameMode.ToFinnish)
            {
                //Grab the amount of valid letters in the finnish word
                foreach (char letter in currentWord.finnishWord)
                {
                    if (letter != ' ')
                        wordLetterCount++;
                }

                //Translate given answer to lowercase to ease checking
                List<char> chars = new();
                for (int i = 0; i < currentWord.finnishWord.Length; i++)
                {
                    chars.Add(wordLetterInputFields[i].text[0]);
                }
                string givenString = new(chars.ToArray());
                givenString.ToLower();

                //For every letter in the word, set a highlight depending on if the letter was correct or not
                for (int i = 0; i < currentWord.finnishWord.Length; i++)
                {
                    if (currentWord.finnishWord[i] == ' ' || wordLetterInputFields[i].text == "") continue;
                    if (givenString[i] == currentWord.finnishWord[i])
                    {
                        //This is the 'correct' indicator
                        wordLetterInputFields[i].transform.GetChild(0).gameObject.SetActive(true);
                        wordLetterInputFields[i].transform.GetChild(1).gameObject.SetActive(false);
                        correctLettersCount++;
                    }
                    else
                    {
                        //This is the 'incorrect' indicator
                        wordLetterInputFields[i].transform.GetChild(0).gameObject.SetActive(false);
                        wordLetterInputFields[i].transform.GetChild(1).gameObject.SetActive(true);
                    }
                }
                if (correctLettersCount == currentWord.finnishWord.Length) wordWasCorrect = true;
            }

            if (wordWasCorrect)
            {
                //DO A LITTLE THING IF WORD WAS CORRECT!!!
                score++;
            }

            wordLetterInputFields[0].Select();
            nextWordButton.gameObject.SetActive(true);
        }

        /// <summary>
        /// This is ran when starting a new game or after the player completes a word and the relevant list of words has entries remaining.
        /// </summary>
        private void SetupNewWord()
        {
            currentWord = words.Pop();
            wordLetterInputFields = new();
            letterTextRefs = new();
            wordWasChecked = false;

            //Setup a new holder for all the individual input fields
            wordInputFieldHolder = Instantiate(wordInputFieldHolderPrefab, translateMinigameBG.transform).transform;
            inputFieldHandler = wordInputFieldHolder.GetComponent<InputFieldHandling>();

            //If the gamemode is set to translate into swedish
            if (gameMode == GameMode.ToSwedish)
            {
                //Show the word to be translated
                wordToTranslateText.text = currentWord.finnishWord;

                //Create an input field for every letter of the word
                for (int i = 0; i < currentWord.swedishWord.Length; i++)
                {
                    int indexHolder = i;
                    wordLetterInputFields.Add(Instantiate(wordLetterInputPrefab, wordInputFieldHolder).GetComponent<TMP_InputField>());

                    //Add listeners to input fields. These are used for navigating between each input field of the word
                    wordLetterInputFields[i].onValueChanged.AddListener((s) => inputFieldHandler.GoNextField());
                    wordLetterInputFields[i].onSelect.AddListener((s) => inputFieldHandler.GetActiveIndex(indexHolder));

                    //If the letter in the word is a space, disable visuals and make input field unable to be interacted with
                    if (currentWord.swedishWord[i] == ' ')
                    {
                        wordLetterInputFields[i].image.enabled = false;
                        wordLetterInputFields[i].interactable = false;
                    }

                    //Save references to the text slot of each input field, used when changing font settings!
                    letterTextRefs.Add(wordLetterInputFields[i].transform.Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>());

                    //Set initial input field background and font colors based on if light mode is enabled or not
                    wordLetterInputFields[i].image.color = UIManager.instance.LightmodeOn ? UIManager.instance.Darkgrey : UIManager.instance.Lightgrey;
                    letterTextRefs[i].color = UIManager.instance.LightmodeOn ? UIManager.instance.Lightgrey : UIManager.instance.Darkgrey;
                    letterTextRefs[i].font = UIManager.instance.hyperlegibleOn ? UIManager.instance.legibleFont : UIManager.instance.basicFont;
                }
            }
            //If the gamemode is set to translate into finnish
            else if (gameMode == GameMode.ToFinnish)
            {
                //Show the word to be translated
                wordToTranslateText.text = currentWord.swedishWord;

                //Create an input field for every letter of the word
                for (int i = 0; i < currentWord.finnishWord.Length; i++)
                {
                    int indexHolder = i;
                    wordLetterInputFields.Add(Instantiate(wordLetterInputPrefab, wordInputFieldHolder).GetComponent<TMP_InputField>());

                    //Add listeners to input fields. These are used for navigating between each input field of the word
                    wordLetterInputFields[i].onValueChanged.AddListener((s) => inputFieldHandler.GoNextField());
                    wordLetterInputFields[i].onSelect.AddListener((s) => inputFieldHandler.GetActiveIndex(indexHolder));

                    //If the letter in the word is a space, disable visuals and make input field unable to be interacted with
                    if (currentWord.finnishWord[i] == ' ')
                    {
                        wordLetterInputFields[i].image.enabled = false;
                        wordLetterInputFields[i].interactable = false;
                    }

                    //Save references to the text slot of each input field, used when changing font settings!
                    letterTextRefs.Add(wordLetterInputFields[i].transform.Find("Text Area").transform.Find("Text").GetComponent<TextMeshProUGUI>());

                    //Set initial input field background and font colors based on if light mode is enabled or not
                    if (UIManager.instance.LightmodeOn)
                    {
                        var colorBlock = wordLetterInputFields[i].colors;
                        colorBlock.normalColor = UIManager.instance.Darkgrey;
                        colorBlock.selectedColor = UIManager.instance.LightmodeHighlight;
                        colorBlock.highlightedColor = UIManager.instance.DarkgreyLighter;
                        colorBlock.pressedColor = UIManager.instance.DarkgreyLighter;
                        wordLetterInputFields[i].colors = colorBlock;
                        letterTextRefs[i].color = UIManager.instance.Lightgrey;
                    }
                    else
                    {
                        var colorBlock = wordLetterInputFields[i].colors;
                        colorBlock.normalColor = UIManager.instance.Lightgrey;
                        colorBlock.selectedColor = UIManager.instance.DarkmodeHighlight;
                        colorBlock.highlightedColor = UIManager.instance.LightgreyDarker;
                        colorBlock.pressedColor = UIManager.instance.LightgreyDarker;
                        wordLetterInputFields[i].colors = colorBlock;
                        letterTextRefs[i].color = UIManager.instance.Darkgrey;
                    }
                }
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
            AudioManager.Instance.PlayMenuSelect3();    // Probably just a temporary thing to signal that the game isn't lagging
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

        #endregion
    }
}