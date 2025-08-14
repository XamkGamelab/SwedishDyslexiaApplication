using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    /// This class handles the verb conjugation minigame.
    /// </summary>
    public class ConjugationMinigame : MonoBehaviour
    {
        public enum ConjugateInto
        {
            perusmuoto = 1,
            preesens,
            imperfekti,
            perfekti,
            pluskvamperfekti
        }

        [Header("Variable References")]
        [SerializeField] private InputReader inputReader;
        [SerializeField] private Button abortBtn;
        [SerializeField] private TextMeshProUGUI swedishBaseWordTxt;
        [SerializeField] private TextMeshProUGUI conjugationClassTxt;
        [SerializeField] private TextMeshProUGUI instructionTxt;
        [SerializeField] private GameObject irregularHintObj;
        [SerializeField] private Button finnishHintBtn;
        [SerializeField] private TextMeshProUGUI finnishHintTxt;
        [SerializeField] private TextMeshProUGUI translatedCounter;
        [SerializeField] private TextMeshProUGUI correctCounter;
        [SerializeField] private Button checkWordBtn;
        [SerializeField] private Button nextWordBtn;
        [SerializeField] private GameObject inputfieldHolder;
        [SerializeField] private GameObject singleInputfield;
        [Tooltip("Value between 0 and 1; percentage")]
        [SerializeField] private float goodScoreThreshold = 0.5f;
        [SerializeField] private float newWordDelay = 1f;
        [SerializeField] private int allowedMissedInputsCount = 2;

        //Input field related references
        private InputFieldHandling inputFieldHandling;
        private List<TMP_InputField> singleInputfields;
        private List<TextMeshProUGUI> fieldTextRefs;

        [Header("Lightmode-related")]
        [SerializeField] private Image conjugationClassImage;

        //Other game-related variables
        private ConjugateInto conjugateInto;
        private int wordFormsCount;
        private Queue<VerbWord> verbList;
        private List<Word> wordsToImprove;
        private VerbWord activeWord;
        private string activeWordWantedForm;
        private string activeWordWantedFormNoHighlight;
        private bool formIsRegular;
        private bool wordWasChecked;
        private bool gotScoreForWord = false;
        private int score = 0;
        private int playedWordsCount = -1;
        private int activeGameWordCount = 0;
        private bool wordWasCorrect = false;
        private List<TextMeshProUGUI> gameTextRefs;

        //Events
        public event Action WordCorrectEvent;
        public event Action WordIncorrectEvent;

        //Readonly
        private readonly Vector2 holderPos = new(0, -100f);
        private readonly string promptStart = "Taivuta muotoon:\n";

        private void Start()
        {
            wordFormsCount = System.Enum.GetNames(typeof(ConjugateInto)).Length;
            abortBtn.onClick.AddListener(AbortGame);
            swedishBaseWordTxt.RegisterDirtyLayoutCallback(() => UIManager.Instance.FixTextSpacing(swedishBaseWordTxt));
        }

        #region lightmode-related

        /// <summary>
        /// This method sets every relevant UI element to its lightmode version
        /// </summary>
        private void ToLightmode()
        {
            abortBtn.image.sprite = UIManager.Instance.AbortSpriteLightmode;
            checkWordBtn.image.sprite = UIManager.Instance.ButtonSpriteLightmode;
            nextWordBtn.image.sprite = UIManager.Instance.ButtonSpriteLightmode;
            finnishHintBtn.image.sprite = UIManager.Instance.ButtonSpriteLightmode;
            conjugationClassImage.color = UIManager.Instance.Darkgrey;
            gameTextRefs.ForEach((text) => text.color = UIManager.Instance.Darkgrey);

            if (singleInputfields == null || singleInputfields.Count == 0) return;
            LightmodeInputFields();
        }

        /// <summary>
        /// This method sets every relevant UI element to its darkmode version
        /// </summary>
        private void ToDarkmode()
        {
            abortBtn.image.sprite = UIManager.Instance.AbortSpriteDarkmode;
            checkWordBtn.image.sprite = UIManager.Instance.ButtonSpriteDarkmode;
            nextWordBtn.image.sprite = UIManager.Instance.ButtonSpriteDarkmode;
            finnishHintBtn.image.sprite = UIManager.Instance.ButtonSpriteDarkmode;
            conjugationClassImage.color = UIManager.Instance.Lightgrey;
            gameTextRefs.ForEach((text) => text.color = UIManager.Instance.Lightgrey);

            if (singleInputfields == null || singleInputfields.Count == 0) return;
            DarkmodeInputFields();
        }

        /// <summary>
        /// This method handles setting all relevant colors for each of the word's input fields
        /// to their light mode version
        /// </summary>
        private void LightmodeInputFields()
        {
            for (int i = 0; i < singleInputfields.Count; i++)
            {
                var colorBlock = singleInputfields[i].colors;
                colorBlock.normalColor = UIManager.Instance.Darkgrey;
                colorBlock.selectedColor = UIManager.Instance.LightmodeHighlight;
                colorBlock.highlightedColor = UIManager.Instance.DarkgreyLighter;
                colorBlock.pressedColor = UIManager.Instance.DarkgreyLighter;
                colorBlock.disabledColor = UIManager.Instance.DarkgreyHalfAlpha;
                singleInputfields[i].colors = colorBlock;
                fieldTextRefs[i].color = UIManager.Instance.Lightgrey;
            }
        }

        /// <summary>
        /// This method handles setting all relevant colors for each of the word's input fields
        /// to their dark mode version
        /// </summary>
        private void DarkmodeInputFields()
        {
            for (int i = 0; i < singleInputfields.Count; i++)
            {
                var colorBlock = singleInputfields[i].colors;
                colorBlock.normalColor = UIManager.Instance.Lightgrey;
                colorBlock.selectedColor = UIManager.Instance.DarkmodeHighlight;
                colorBlock.highlightedColor = UIManager.Instance.LightgreyDarker;
                colorBlock.pressedColor = UIManager.Instance.LightgreyDarker;
                colorBlock.disabledColor = UIManager.Instance.LightgreyHalfAlpha;
                singleInputfields[i].colors = colorBlock;
                fieldTextRefs[i].color = UIManager.Instance.Darkgrey;
            }
        }

        #endregion

        #region font-related

        /// <summary>
        /// This method handles setting all of this minigame's text elements to the hyperlegible font
        /// </summary>
        private void ToHyperlegibleFont()
        {
            gameTextRefs.ForEach((text) =>
            {
                text.font = UIManager.Instance.LegibleFont;
                text.characterSpacing = UIManager.Instance.LegibleSpacing;
            });
            if (fieldTextRefs == null || fieldTextRefs.Count == 0) return;
            FieldsToHyperlegibleFont();
        }

        /// <summary>
        /// This method handles setting all of this minigame's text elements to the basic font
        /// </summary>
        private void ToBasicFont()
        {
            gameTextRefs.ForEach((text) =>
            {
                text.font = UIManager.Instance.BasicFont;
                text.characterSpacing = UIManager.Instance.BasicSpacing;
            });
            if (fieldTextRefs == null || fieldTextRefs.Count == 0) return;
            FieldsToBasicFont();
        }

        /// <summary>
        /// Sets input fields to legible font
        /// </summary>
        private void FieldsToHyperlegibleFont()
        {
            fieldTextRefs.ForEach((text) => text.font = UIManager.Instance.LegibleFont);
        }
        
        /// <summary>
        /// Sets input fields to basic font
        /// </summary>
        private void FieldsToBasicFont()
        {
            fieldTextRefs.ForEach((text) => text.font = UIManager.Instance.BasicFont);
        }

        #endregion

        #region game functionality

        /// <summary>
        /// This method initializes all the variables, GUI elements and event listeners
        /// relevant to the conjugation minigame. This is called to start the minigame, and
        /// eventually calls <see cref="InitializeNewWord"/> to show the first word.
        /// </summary>
        /// <param name="_verbList">List of VerbWord objects to use for this game</param>
        public void InitializeGame(List<VerbWord> _verbList)
        {
            gameTextRefs = transform.GetComponentsInChildren<TextMeshProUGUI>(true).ToList();
            gameObject.SetActive(true);
            nextWordBtn.gameObject.SetActive(false);
            finnishHintTxt.text = "Hint";
            verbList = new(_verbList);
            wordsToImprove = new();
            score = 0;
            playedWordsCount = -1;
            activeGameWordCount = verbList.Count;
            correctCounter.text = "0";

            //Like and subscribe
            inputReader.SubmitEventCancelled += CheckWord;
            inputReader.SubmitEventHeld += NextWord;
            checkWordBtn.onClick.AddListener(CheckWord);
            nextWordBtn.onClick.AddListener(NextWord);
            finnishHintBtn.onClick.AddListener(ShowHint);
            UIManager.Instance.LightmodeOnEvent += ToLightmode;
            UIManager.Instance.LightmodeOffEvent += ToDarkmode;
            UIManager.Instance.LegibleModeOnEvent += ToHyperlegibleFont;
            UIManager.Instance.LegibleModeOffEvent += ToBasicFont;

            //set initial colors dependending on if lightmode is on or off
            if (UIManager.Instance.LightmodeOn) ToLightmode();
            else ToDarkmode();
            if (UIManager.Instance.HyperlegibleOn) ToHyperlegibleFont();
            else ToBasicFont();

            //set the first task
            InitializeNewWord();
        }

        /// <summary>
        /// This method sets up all relevant UI elements to reflect the game's currently active word,
        /// and then instantiates input fields for the word to be written into.
        /// </summary>
        private void InitializeNewWord()
        {
            //Get new active word & set relevant variables
            activeWord = verbList.Dequeue();
            wordWasChecked = false;
            wordWasCorrect = false;
            gotScoreForWord = false;
            conjugateInto = (ConjugateInto)UnityEngine.Random.Range((int)ConjugateInto.imperfekti, wordFormsCount + 1);
            instructionTxt.text = string.Concat(promptStart, conjugateInto);
            singleInputfields = new();
            fieldTextRefs = new();
            playedWordsCount++;

            //UGLY SWITCH CASE WARNING
            switch (conjugateInto)
            {
                case ConjugateInto.preesens:
                    formIsRegular = activeWord.currentTenseIsRegular;
                    activeWordWantedForm = activeWord.CurrentTenseWord();
                    break;
                case ConjugateInto.imperfekti:
                    formIsRegular = activeWord.pastTenseIsRegular;
                    activeWordWantedForm = activeWord.PastTenseWord();
                    break;
                case ConjugateInto.perfekti:
                    formIsRegular = activeWord.pastPerfectTenseIsRegular;
                    activeWordWantedForm = activeWord.PastPerfectTenseWord();
                    break;
                case ConjugateInto.pluskvamperfekti:
                    formIsRegular = activeWord.pastPlusPerfectTenseIsRegular;
                    activeWordWantedForm = activeWord.PastPlusPerfectTenseWord();
                    break;
                default:
                    string errorMessage = string.Concat($"Something went horribly wrong with randomly determining word conjugation form:\n",
                    $"conjugateInto value was set to {(int)conjugateInto} when only values between 2-5 can be safely handled! Ref: {typeof(ConjugateInto)}");
                    Debug.LogError(errorMessage);
                    return;
            }

            //Update GUI
            if (!formIsRegular) irregularHintObj.SetActive(true);
            else irregularHintObj.SetActive(false);
            swedishBaseWordTxt.text = activeWord.swedishWord;
            conjugationClassTxt.text = activeWord.conjugationClass.ToString();
            translatedCounter.text = string.Concat(playedWordsCount, "/", activeGameWordCount);

            //Instantiate input field -related objects
            inputFieldHandling = Instantiate(inputfieldHolder, transform).GetComponent<InputFieldHandling>();
            inputFieldHandling.GetComponent<RectTransform>().localPosition = holderPos;
            activeWordWantedFormNoHighlight = Helpers.CleanWord(activeWordWantedForm);

            Debug.Log(activeWordWantedFormNoHighlight);

            for (int i = 0; i < activeWordWantedFormNoHighlight.Length; i++)
            {
                int indexHolder = i;

                //Grab refs
                singleInputfields.Add(Instantiate(singleInputfield, inputFieldHandling.transform).GetComponent<TMP_InputField>());
                fieldTextRefs.Add(singleInputfields[i].transform.Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>());
                if (activeWordWantedFormNoHighlight[i] == ' ' || activeWordWantedFormNoHighlight == "")
                {
                    singleInputfields[i].interactable = false;
                    singleInputfields[i].image.enabled = false;
                }

                //Like and subscribe
                singleInputfields[i].onValueChanged.AddListener((s) => inputFieldHandling.GoNextField());
                singleInputfields[i].onSelect.AddListener((s) => inputFieldHandling.GetActiveIndex(indexHolder));
            }
            if (UIManager.Instance.LightmodeOn) LightmodeInputFields();
            else DarkmodeInputFields();
            if (UIManager.Instance.HyperlegibleOn) FieldsToHyperlegibleFont();
            else FieldsToBasicFont();
            singleInputfields[0].ActivateInputField();
        }

        /// <summary>
        /// This method handles checking if the word written in the input fields is correct.
        /// </summary>
        private void CheckWord()
        {
            if (!checkWordBtn.interactable) return;

            wordWasCorrect = false;
            int correctLettersCount = 0;
            int wordLetterCount = 0;
            int missedInputsCount = 0;

            //This bit is used to make sure that capitalized letters are also treated as correct
            List<char> chars = new();
            for (int i = 0; i < activeWordWantedFormNoHighlight.Length; i++)
            {
                if (activeWordWantedFormNoHighlight[i] == ' ')
                    chars.Add(' ');
                else if (singleInputfields[i].text != "")
                    chars.Add(singleInputfields[i].text[0]);
                else
                    chars.Add(' ');
                wordLetterCount++;
            }
            string givenString = new(chars.ToArray());
            givenString = givenString.ToLower();

            //Enable/disable indicators to show if a letter was correct or not, also add to a counter of
            //correct letters, used for determining if the whole word was correct.
            for (int i = 0; i < activeWordWantedFormNoHighlight.Length; i++)
            {
                if (activeWordWantedFormNoHighlight[i] == ' ')
                {
                    chars.Add(' ');
                    correctLettersCount++;
                }
                else if (singleInputfields[i].text == "")
                    missedInputsCount++;
                else if (givenString[i] == activeWordWantedFormNoHighlight[i])
                {
                    //Activate "correct" indicator
                    singleInputfields[i].transform.GetChild(0).gameObject.SetActive(true);
                    singleInputfields[i].transform.GetChild(1).gameObject.SetActive(false);
                    correctLettersCount++;
                }
                else
                {
                    //Activate "incorrect" indicator
                    singleInputfields[i].transform.GetChild(0).gameObject.SetActive(false);
                    singleInputfields[i].transform.GetChild(1).gameObject.SetActive(true);
                }
            }

            if (correctLettersCount == wordLetterCount) wordWasCorrect = true;

            if (wordWasCorrect)
            {
                if (!gotScoreForWord)
                {
                    gotScoreForWord = true;
                    score++;
                }
                correctCounter.text = score.ToString();
                WordCorrectEvent?.Invoke();
                Debug.Log("Word was correct!");
            }
            else
            {
                WordIncorrectEvent?.Invoke();
            }

            if (missedInputsCount <= allowedMissedInputsCount)
            {
                wordWasChecked = true;
                nextWordBtn.gameObject.SetActive(true);
            }

            singleInputfields[0].Select();
        }

        /// <summary>
        /// If there are words remaining in the word list, go to next word. Else, end game.
        /// </summary>
        private void NextWord()
        {
            if (!wordWasChecked) return;

            Destroy(inputFieldHandling.gameObject);
            if (verbList == null || verbList.Count == 0)
            {
                CompleteGame();
                return;
            }

            if (!wordWasCorrect) wordsToImprove.Add(activeWord);
            StartCoroutine(NextWordDelay());
        }

        /// <summary>
        /// This method unsubscribes events that would break things, destroys the input fields,
        /// and resets some variables.
        /// </summary>
        /// <param name="_endInstantly">If true, ends game immediately, defaults to false
        /// to give time for an end-of-game animation to be played</param>
        private void AbortGame()
        {
            //Shouldn't need to reset any variables as they're set at the start of the game anyway
            UnsubcribeEvents();
            Destroy(inputFieldHandling.gameObject);
            UIManager.Instance.TriggerTipChange();
            gameObject.SetActive(false);
        }

        private void CompleteGame()
        {
            UnsubcribeEvents();
            gameObject.SetActive(false);
            UIManager.Instance.TriggerTipChange();

            UIManager.Instance.ActivateMinigameEndscreen(_maxScore: activeGameWordCount, _realScore: score,
                _goodScoreThreshold: goodScoreThreshold, _wordsToImprove: wordsToImprove);
        }

        private void UnsubcribeEvents()
        {
            inputReader.SubmitEventCancelled -= CheckWord;
            inputReader.SubmitEventHeld -= NextWord;
            UIManager.Instance.LightmodeOnEvent -= ToLightmode;
            UIManager.Instance.LightmodeOffEvent -= ToDarkmode;
            UIManager.Instance.LegibleModeOnEvent -= ToHyperlegibleFont;
            UIManager.Instance.LegibleModeOffEvent -= ToBasicFont;
            checkWordBtn.onClick.RemoveListener(CheckWord);
            nextWordBtn.onClick.RemoveListener(NextWord);
        }

        /// <summary>
        /// Coroutine used for delaying showing the next word for the purposes of a little
        /// animation perhaps.
        /// </summary>
        /// <returns></returns>
        private IEnumerator NextWordDelay()
        {
            checkWordBtn.interactable = false;
            finnishHintTxt.text = "Hint";
            nextWordBtn.gameObject.SetActive(false);
            yield return new WaitForSeconds(newWordDelay);
            checkWordBtn.interactable = true;
            InitializeNewWord();
        }

        /// <summary>
        /// This method is subscribed to the hint button, and when clicked it shows the
        /// finnish version of the desired word.
        /// </summary>
        private void ShowHint()
        {
            if (!checkWordBtn.interactable) return;
            finnishHintTxt.text = activeWord.GetConjugatedFinnish(conjugateInto);
        }
        
        #endregion
    }
}
