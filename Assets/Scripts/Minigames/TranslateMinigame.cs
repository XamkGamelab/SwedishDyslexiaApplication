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

        [SerializeField] private InputReader inputReader;

        private GameMode? gameMode = null;
        private Stack<Word> words;
        private Word currentWord = null;

        [SerializeField] private GameObject translateMinigameBG;
        [SerializeField] private Button checkWordButton;
        [SerializeField] private GameObject wordInputFieldHolderPrefab;
        [SerializeField] private GameObject wordLetterInputPrefab;
        private Transform wordInputFieldHolder;
        private InputFieldHandling inputFieldHandler;
        private List<TMP_InputField> wordLetterInputFields;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            checkWordButton.onClick.AddListener(CheckWord);
            inputReader.SubmitEvent += CheckWord;
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

            wordInputFieldHolder = Instantiate(wordInputFieldHolderPrefab, translateMinigameBG.transform).transform;
            inputFieldHandler = wordInputFieldHolder.GetComponent<InputFieldHandling>();

            if (gameMode == GameMode.ToSwedish)
            {
                for (int i = 0; i < currentWord.swedishWord.Length; i++)
                {
                    int indexHolder = 0;
                    wordLetterInputFields.Add(Instantiate(wordLetterInputPrefab, wordInputFieldHolder).GetComponent<TMP_InputField>());
                    wordLetterInputFields[i].onValueChanged.AddListener((s) => inputFieldHandler.GoNextField());
                    indexHolder = i;
                    wordLetterInputFields[i].onSelect.AddListener((s) => inputFieldHandler.GetActiveIndex(indexHolder));
                }
            }
            else if (gameMode == GameMode.ToFinnish)
            {
                for (int i = 0; i < currentWord.finnishWord.Length; i++)
                {
                    int indexHolder = 0;
                    wordLetterInputFields.Add(Instantiate(wordLetterInputPrefab, wordInputFieldHolder).GetComponent<TMP_InputField>());
                    wordLetterInputFields[i].onValueChanged.AddListener((s) => inputFieldHandler.GoNextField());
                    indexHolder = i;
                    wordLetterInputFields[i].onSelect.AddListener((s) => inputFieldHandler.GetActiveIndex(indexHolder));
                }
            }

            wordLetterInputFields[0].ActivateInputField();
        }

        private void DeleteOldWord()
        {
            wordLetterInputFields.Clear();
            Destroy(wordInputFieldHolder.gameObject);
        }
    }
}
