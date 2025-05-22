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
        private VerbWord[] verbWords;
        private AdjectiveWord[] adjectiveWords;
        private int activeWordIndex = 0;

        [SerializeField] private float nextWordDelay = 0.5f;
        [SerializeField] private float gameEndDelay = 1.0f;

        [SerializeField] private FlashCardNoun nounObject;
        [SerializeField] private FlashCardVerb verbObject;
        [SerializeField] private FlashCardAdjective adjectiveObject;
        [SerializeField] private Button nextWordBtn;
        [SerializeField] private Button abortGameButton;
        private bool gameEnding = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            nounObject.gameObject.SetActive(false);
            abortGameButton.onClick.AddListener(EndGame);
            
            //Abort game button's sprite is set according to if light mode is on
            abortGameButton.image.sprite = UIManager.instance.LightmodeOn ? UIManager.instance.abortSpriteLightmode : UIManager.instance.abortSpriteDarkmode;
            UIManager.instance.LightmodeOnEvent += AbortButtonToLightmode;
            UIManager.instance.LightmodeOffEvent += AbortButtonToDarkmode;
        }

        #region lightmode related methods

        private void AbortButtonToLightmode()
        {
            abortGameButton.image.sprite = UIManager.instance.abortSpriteLightmode;
        }

        private void AbortButtonToDarkmode()
        {
            abortGameButton.image.sprite = UIManager.instance.abortSpriteDarkmode;
        }

        #endregion

        #region noun related methods

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

        #endregion

        #region verb related methods

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

        public void StartVerbGame(VerbWord[] _verbWords)
        {
            gameObject.SetActive(true);
            verbObject.gameObject.SetActive(true);
            verbWords = _verbWords;
            activeWordIndex = 0;
            StartCoroutine(DisplayVerb());
            nextWordBtn.onClick.AddListener(NextVerb);
            verbObject.ResetToFinnishSide();
            UIManager.instance.LightmodeOnEvent += DisplayCurrentVerb;
            UIManager.instance.LightmodeOffEvent += DisplayCurrentVerb;

            //Set initial colors
            if (UIManager.instance.LightmodeOn)
                verbObject.LightsOn();
            else
                verbObject.LightsOff();
        }

        private IEnumerator DisplayVerb()
        {
            nextWordBtn.interactable = false;
            verbObject.gameObject.SetActive(false);
            DisplayCurrentVerb();
            yield return new WaitForSeconds(nextWordDelay);
            verbObject.gameObject.SetActive(true);
            yield return new WaitForSeconds(nextWordDelay);
            nextWordBtn.interactable = true;
        }

        private void NextVerb()
        {
            if (verbObject.state == FlashCardBase.State.Flipping || gameEnding) return;
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

        private void DisplayCurrentAdjective()
        {
            Debug.Log(activeWordIndex);
            adjectiveObject.wordFinnishText.text = adjectiveWords[activeWordIndex].finnishWord;
            adjectiveObject.wordSwedishBaseText.text = adjectiveWords[activeWordIndex].swedishWord;
            adjectiveObject.wordSwedishComparativeText.text = adjectiveWords[activeWordIndex].AdjectiveComparative();
            adjectiveObject.wordSwedishSuperlativeText.text = adjectiveWords[activeWordIndex].AdjectiveSuperlative();
        }

        public void StartAdjectiveGame(AdjectiveWord[] _adjectiveWords)
        {
            gameObject.SetActive(true);
            adjectiveObject.gameObject.SetActive(true);
            adjectiveWords = _adjectiveWords;
            activeWordIndex = 0;
            StartCoroutine(DisplayAdjective());
            nextWordBtn.onClick.AddListener(NextAdjective);
            adjectiveObject.ResetToFinnishSide();
            UIManager.instance.LightmodeOnEvent += DisplayCurrentAdjective;
            UIManager.instance.LightmodeOffEvent += DisplayCurrentAdjective;

            //Set initial colors
            if (UIManager.instance.LightmodeOn)
                adjectiveObject.LightsOn();
            else
                adjectiveObject.LightsOff();
        }

        private IEnumerator DisplayAdjective()
        {
            nextWordBtn.interactable = false;
            adjectiveObject.gameObject.SetActive(false);
            DisplayCurrentAdjective();
            yield return new WaitForSeconds(nextWordDelay);
            adjectiveObject.gameObject.SetActive(true);
            yield return new WaitForSeconds(nextWordDelay);
            nextWordBtn.interactable = true;
        }

        private void NextAdjective()
        {
            if (adjectiveObject.state == FlashCardBase.State.Flipping || gameEnding) return;
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

        private void EndGame()
        {
            gameEnding = true;
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
            gameEnding = false;
        }
    }
}
