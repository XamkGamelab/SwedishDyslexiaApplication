using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private GameObject wordInputFieldHolderPrefab;
        [SerializeField] private GameObject wordLetterInputPrefab;
        [SerializeField] private TextMeshProUGUI wordToTranslateText;
        private Transform wordInputFieldHolder;
        private InputFieldHandling inputFieldHandler;
        private List<TMP_InputField> wordLetterInputFields;
        private List<TextMeshProUGUI> letterTextRefs;

        [Header("Lightmode Related")]
        [SerializeField] private Sprite abortSpriteDarkmode;
        [SerializeField] private Sprite abortSpriteLightmode;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            checkWordButton.onClick.AddListener(CheckWord);
            abortGameButton.onClick.AddListener(AbortGame);
            inputReader.SubmitEventCancelled += CheckWord;
            inputReader.SubmitEventHeld += DeleteOldWord;
            UIManager.instance.LegibleModeOnEvent += SwapFieldsToLegibleFont;
            UIManager.instance.LegibleModeOffEvent += SwapFieldsToBasicFont;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        /// <summary>
        /// This method sets up relevant variables required to start a game
        /// </summary>
        /// <param name="_wordType">This tells the script what word type will be used for the game</param>
        /// <param name="_words">Give a list of Word-objects as parameter, this is used as the word list for the translation game</param>
        public void StartGame(GameMode _gameMode, List<Word> _words)
        {
            translateMinigameBG.SetActive(true);
            gameMode = _gameMode;
            words = new(_words.ToArray());
            activeGameMaxPoints = words.Count;
            abortGameButton.image.sprite = UIManager.instance.LightmodeOn ? abortSpriteLightmode : abortSpriteDarkmode;
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

            if (gameMode == GameMode.ToSwedish)
            {
                foreach (char letter in currentWord.swedishWord)
                {
                    if (letter != ' ')
                    wordLetterCount++;
                }
                for (int i = 0; i < currentWord.swedishWord.Length; i++)
                    {
                        if (currentWord.swedishWord[i] == ' ') continue;
                        if (wordLetterInputFields[i].text[0] == currentWord.swedishWord[i])
                        {
                            //THIS LETTER WAS CORRECT, DO WE DO A GREEN LITTLE HIGHLIGHT?
                            wordLetterInputFields[i].transform.GetChild(0).gameObject.SetActive(true);
                            correctLettersCount++;
                        }
                        else
                        {
                            //THIS LETTER WAS INCORRECT, SHOW INDICATOR
                            wordLetterInputFields[i].transform.GetChild(1).gameObject.SetActive(true);
                        }
                    }
                if (correctLettersCount == currentWord.swedishWord.Length) wordWasCorrect = true;
            }
            else if (gameMode == GameMode.ToFinnish)
            {
                foreach (char letter in currentWord.finnishWord)
                {
                    if (letter != ' ')
                    wordLetterCount++;
                }
                for (int i = 0; i < currentWord.finnishWord.Length; i++)
                {
                    if (currentWord.finnishWord[i] == ' ') continue;
                    if (wordLetterInputFields[i].text[0] == currentWord.finnishWord[i])
                    {
                        //THIS LETTER WAS CORRECT, DO WE DO A GREEN LITTLE HIGHLIGHT?
                        wordLetterInputFields[i].transform.GetChild(0).gameObject.SetActive(true);
                        correctLettersCount++;
                    }
                    else
                    {
                        //THIS LETTER WAS INCORRECT, SHOW INDICATOR
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

            wordInputFieldHolder = Instantiate(wordInputFieldHolderPrefab, translateMinigameBG.transform).transform;
            inputFieldHandler = wordInputFieldHolder.GetComponent<InputFieldHandling>();

            if (gameMode == GameMode.ToSwedish)
            {
                wordToTranslateText.text = currentWord.finnishWord;
                for (int i = 0; i < currentWord.swedishWord.Length; i++)
                {
                    int indexHolder = i;
                    wordLetterInputFields.Add(Instantiate(wordLetterInputPrefab, wordInputFieldHolder).GetComponent<TMP_InputField>());
                    wordLetterInputFields[i].onValueChanged.AddListener((s) => inputFieldHandler.GoNextField());
                    wordLetterInputFields[i].onSelect.AddListener((s) => inputFieldHandler.GetActiveIndex(indexHolder));
                    if (currentWord.swedishWord[i] == ' ')
                    {
                        wordLetterInputFields[i].image.enabled = false;
                        wordLetterInputFields[i].interactable = false;
                    }
                    letterTextRefs.Add(wordLetterInputFields[i].transform.Find("Text Area").transform.Find("Text").GetComponent<TextMeshProUGUI>());
                    wordLetterInputFields[i].image.color = UIManager.instance.LightmodeOn ? UIManager.instance.Darkgrey : UIManager.instance.Lightgrey;
                    letterTextRefs[i].color = UIManager.instance.LightmodeOn ? UIManager.instance.Lightgrey : UIManager.instance.Darkgrey;
                }
            }
            else if (gameMode == GameMode.ToFinnish)
            {
                wordToTranslateText.text = currentWord.swedishWord;
                for (int i = 0; i < currentWord.finnishWord.Length; i++)
                {
                    int indexHolder = i;
                    wordLetterInputFields.Add(Instantiate(wordLetterInputPrefab, wordInputFieldHolder).GetComponent<TMP_InputField>());
                    wordLetterInputFields[i].onValueChanged.AddListener((s) => inputFieldHandler.GoNextField());
                    wordLetterInputFields[i].onSelect.AddListener((s) => inputFieldHandler.GetActiveIndex(indexHolder));
                    if (currentWord.finnishWord[i] == ' ')
                    {
                        wordLetterInputFields[i].image.enabled = false;
                        wordLetterInputFields[i].interactable = false;
                    }
                    letterTextRefs.Add(wordLetterInputFields[i].transform.Find("Text Area").transform.Find("Text").GetComponent<TextMeshProUGUI>());
                    wordLetterInputFields[i].image.color = UIManager.instance.LightmodeOn ? UIManager.instance.Darkgrey : UIManager.instance.Lightgrey;
                    letterTextRefs[i].color = UIManager.instance.LightmodeOn ? UIManager.instance.Lightgrey : UIManager.instance.Darkgrey;
                }
            }

            UIManager.instance.LightmodeOnEvent += ChangeToLightmode;
            UIManager.instance.LightmodeOffEvent += ChangeToDarkmode;
            wordLetterInputFields[0].ActivateInputField();
        }

        private void DeleteOldWord()
        {
            if (!wordWasChecked) return;
            if (!canDeleteWord) return;
            
            canDeleteWord = false;
            wordLetterInputFields.Clear();
            letterTextRefs.Clear();
            Destroy(wordInputFieldHolder.gameObject);
            UIManager.instance.LegibleModeOnEvent -= SwapFieldsToLegibleFont;
            UIManager.instance.LegibleModeOnEvent -= SwapFieldsToBasicFont;
            UIManager.instance.LightmodeOnEvent -= ChangeToLightmode;
            UIManager.instance.LightmodeOffEvent -= ChangeToDarkmode;
            wordToTranslateText.text = "";

            StartCoroutine(DelayBeforeNewWord());
        }

        private IEnumerator DelayBeforeNewWord()
        {
            //hit a particle effect or some other thing if wordWasCorrect

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

        private void CompleteGame()
        {
            Debug.Log("game completed yippee");
            translateMinigameBG.SetActive(false);
        }

        private void AbortGame()
        {
            Debug.Log("game aborted");

            //unsubscribe events
            UIManager.instance.LegibleModeOnEvent -= SwapFieldsToLegibleFont;
            UIManager.instance.LegibleModeOnEvent -= SwapFieldsToBasicFont;
            UIManager.instance.LightmodeOnEvent -= ChangeToLightmode;
            UIManager.instance.LightmodeOffEvent -= ChangeToDarkmode;

            //clear variables
            words = new();
            currentWord = new();
            wordToTranslateText.text = "";
            score = 0;
            activeGameMaxPoints = 0;
            wordLetterInputFields.Clear();
            letterTextRefs.Clear();

            //destroy word object, disable translate minigame ui
            Destroy(wordInputFieldHolder.gameObject);
            translateMinigameBG.SetActive(false);
        }

        #region lightmode related

        private void ChangeToLightmode()
        {
            for (int i = 0; i < wordLetterInputFields.Count; i++)
            {
                wordLetterInputFields[i].image.color = UIManager.instance.Darkgrey;
                letterTextRefs[i].color = UIManager.instance.Lightgrey;
            }
            abortGameButton.image.sprite = abortSpriteLightmode;
        }

        private void ChangeToDarkmode()
        {
            for (int i = 0; i < wordLetterInputFields.Count; i++)
            {
                wordLetterInputFields[i].image.color = UIManager.instance.Lightgrey;
                letterTextRefs[i].color = UIManager.instance.Darkgrey;
            }
            abortGameButton.image.sprite = abortSpriteDarkmode;
        }

        private void SwapFieldsToLegibleFont()
        {
            foreach (TMP_InputField inputField in wordLetterInputFields)
            {
                inputField.fontAsset = UIManager.instance.legibleFont;
            }
        }

        private void SwapFieldsToBasicFont()
        {
            foreach (TMP_InputField inputField in wordLetterInputFields)
            {
                inputField.fontAsset = UIManager.instance.basicFont;
            }
        }

        #endregion
    }
}