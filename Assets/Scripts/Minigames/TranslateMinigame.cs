using System.Collections.Generic;
using SwedishApp.Input;
using SwedishApp.Words;
using TMPro;
using UnityEngine;

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

        private GameMode? gameMode = null;
        private Stack<Word> words;
        private Word currentWord = null;

        [SerializeField] private GameObject inputFieldHandlerPrefab;
        [SerializeField] private GameObject wordLetterInputPrefab;
        private Transform inputFieldHandlerTransform;
        private TMP_InputField[] wordLetterInputFields;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        /// <summary>
        /// This method sets up relevant variables required to start a game
        /// </summary>
        /// <param name="_wordType">This tells the script what word type will be used for the game</param>
        /// <param name="_verbList">Give a VerbList object as parameter if setting up a verb translation minigame, otherwise set to null</param>
        /// <param name="_nounList">Give a NounList object as parameter if setting up a noun translation minigame, otherwise set to null</param>
        private void StartGame(GameMode _gameMode, List<Word> _words)
        {
            gameMode = _gameMode;
            words = new(_words.ToArray());
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
                        correctLettersCount++;
                    }
                    else
                    {
                        //THIS LETTER WAS INCORRECT, SHOW INDICATOR
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
                        correctLettersCount++;
                    }
                    else
                    {
                        //THIS LETTER WAS INCORRECT, SHOW INDICATOR
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

            inputFieldHandlerTransform = Instantiate(inputFieldHandlerPrefab.transform);

            if (gameMode == GameMode.ToSwedish)
            {
                for (int i = 0; i < currentWord.swedishWord.Length; i++)
                {
                    wordLetterInputFields[i] = Instantiate(wordLetterInputPrefab, inputFieldHandlerTransform).GetComponent<TMP_InputField>();
                    wordLetterInputFields[i].GetComponent<InputFieldHandling>().index = i;
                }
            }
            else if (gameMode == GameMode.ToFinnish)
            {
                for (int i = 0; i < currentWord.finnishWord.Length; i++)
                {
                    wordLetterInputFields[i] = Instantiate(wordLetterInputPrefab, inputFieldHandlerTransform).GetComponent<TMP_InputField>();
                    wordLetterInputFields[i].GetComponent<InputFieldHandling>().index = i;
                }
            }

            wordLetterInputFields[0].Select();
        }
    }
}
