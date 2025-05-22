using System.Collections;
using SwedishApp.UI;
using SwedishApp.Words;
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
        private NounWord[] nounWords;
        private int activeWordIndex = 0;

        [SerializeField] private float nextWordDelay = 0.5f;
        [SerializeField] private float gameEndDelay = 1.0f;

        [SerializeField] private FlashCardNoun nounObject;
        [SerializeField] private Button nextWordBtn;
        private bool gameEnding = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            nounObject.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void DisplayCurrentNoun()
        {
            Debug.Log(activeWordIndex);
            nounObject.wordFinnishText.text = nounWords[activeWordIndex].finnishWord;
            nounObject.wordSwedishBaseText.text = nounWords[activeWordIndex].NounWithGenderStart();
            nounObject.wordSwedishDefinitiveText.text = nounWords[activeWordIndex].NounWithGenderEnd();
            nounObject.wordSwedishPluralText.text = nounWords[activeWordIndex].PluralNoun();
            nounObject.wordSwedishDefinitivePluralText.text = nounWords[activeWordIndex].PluralKnownNoun();
        }

        public void StartNounGame(NounWord[] _nounWords)
        {
            gameObject.SetActive(true);
            nounObject.gameObject.SetActive(true);
            nounWords = _nounWords;
            activeWordIndex = 0;
            StartCoroutine(DisplayNoun());
            nextWordBtn.onClick.AddListener(NextNoun);
            nounObject.ResetToFinnishSide();
            UIManager.instance.LightmodeOnEvent += DisplayCurrentNoun;
            UIManager.instance.LightmodeOffEvent += DisplayCurrentNoun;

            //Set initial colors
            if (UIManager.instance.LightmodeOn)
                nounObject.LightsOn();
            else
                nounObject.LightsOff();
        }

        private IEnumerator DisplayNoun()
        {
            nextWordBtn.interactable = false;
            nounObject.gameObject.SetActive(false);
            DisplayCurrentNoun();
            yield return new WaitForSeconds(nextWordDelay);
            nounObject.gameObject.SetActive(true);
            yield return new WaitForSeconds(nextWordDelay);
            nextWordBtn.interactable = true;
        }

        private void NextNoun()
        {
            if (nounObject.state == FlashCardBase.State.Flipping || gameEnding) return;
            if (activeWordIndex + 1 >= nounWords.Length)
            {
                EndGame();
                return;
            }
            nounObject.ResetToFinnishSide();
            activeWordIndex++;
            StartCoroutine(DisplayNoun());
        }

        private void EndGame()
        {
            gameEnding = true;
            nounWords = null;
            nounObject.gameObject.SetActive(false);
            nextWordBtn.onClick.RemoveAllListeners();
            UIManager.instance.LightmodeOnEvent -= DisplayCurrentNoun;
            UIManager.instance.LightmodeOffEvent -= DisplayCurrentNoun;
            StartCoroutine(GameEndDelay());
        }

        private IEnumerator GameEndDelay()
        {
            yield return new WaitForSeconds(gameEndDelay);
            gameObject.SetActive(false);
            gameEnding = false;
        }
    }
}
