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

        [Header("UI related")]
        [SerializeField] private GameObject translateMinigameBG;
        [SerializeField] private Button checkWordButton;
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
            checkWordButton.onClick.AddListener(CheckWord);
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

            if (gameMode == GameMode.ToSwedish)
            {
                for (int i = 0; i < currentWord.swedishWord.Length; i++)
                {
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
                for (int i = 0; i < currentWord.finnishWord.Length; i++)
                {
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
                    int indexHolder = 0;
                    wordLetterInputFields.Add(Instantiate(wordLetterInputPrefab, wordInputFieldHolder).GetComponent<TMP_InputField>());
                    wordLetterInputFields[i].onValueChanged.AddListener((s) => inputFieldHandler.GoNextField());
                    indexHolder = i;
                    wordLetterInputFields[i].onSelect.AddListener((s) => inputFieldHandler.GetActiveIndex(indexHolder));
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
                    int indexHolder = 0;
                    wordLetterInputFields.Add(Instantiate(wordLetterInputPrefab, wordInputFieldHolder).GetComponent<TMP_InputField>());
                    wordLetterInputFields[i].onValueChanged.AddListener((s) => inputFieldHandler.GoNextField());
                    indexHolder = i;
                    wordLetterInputFields[i].onSelect.AddListener((s) => inputFieldHandler.GetActiveIndex(indexHolder));
                    letterTextRefs.Add(wordLetterInputFields[i].transform.Find("Text Area").transform.Find("Text").GetComponent<TextMeshProUGUI>());
                    wordLetterInputFields[i].image.color = UIManager.instance.LightmodeOn ? UIManager.instance.Darkgrey : UIManager.instance.Lightgrey;
                    letterTextRefs[i].color = UIManager.instance.LightmodeOn ? UIManager.instance.Lightgrey : UIManager.instance.Darkgrey;
                }
            }

            UIManager.instance.LightmodeOnEvent += SetInputBoxesToLightmode;
            UIManager.instance.LightmodeOffEvent += SetInputBoxesToDarkmode;
            wordLetterInputFields[0].ActivateInputField();
        }

        private void DeleteOldWord()
        {
            if (!wordWasChecked) return;
            if (!canDeleteWord) return;
            
            canDeleteWord = false;
            Debug.Log("delete word called");
            wordLetterInputFields.Clear();
            letterTextRefs.Clear();
            Destroy(wordInputFieldHolder.gameObject);
            UIManager.instance.LegibleModeOnEvent -= SwapFieldsToLegibleFont;
            UIManager.instance.LegibleModeOnEvent -= SwapFieldsToBasicFont;
            UIManager.instance.LightmodeOnEvent -= SetInputBoxesToLightmode;
            UIManager.instance.LightmodeOffEvent -= SetInputBoxesToDarkmode;
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
                EndGame();
            }
        }

        private void EndGame()
        {
            Debug.Log("game completed yippee");
        }

        #region lightmode related

        private void SetInputBoxesToLightmode()
        {
            for (int i = 0; i < wordLetterInputFields.Count; i++)
            {
                wordLetterInputFields[i].image.color = UIManager.instance.Lightgrey;
                letterTextRefs[i].color = UIManager.instance.Darkgrey;
            }
        }

        private void SetInputBoxesToDarkmode()
        {
            for (int i = 0; i < wordLetterInputFields.Count; i++)
            {
                wordLetterInputFields[i].image.color = UIManager.instance.Darkgrey;
                letterTextRefs[i].color = UIManager.instance.Lightgrey;
            }
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
