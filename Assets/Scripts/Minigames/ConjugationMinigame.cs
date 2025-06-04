using System.Collections;
using System.Collections.Generic;
using SwedishApp.Input;
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

        private ConjugateInto conjugateInto;
        private int wordFormsCount;
        private Stack<VerbWord> verbList;
        private VerbWord activeWord;
        private string activeWordWantedForm;
        private string activeWordWantedFormNoHighlight;
        private bool formIsRegular;
        private bool wordWasChecked;
        private int correctWordsCount = 0;
        private bool wordWasCorrect = false;
        public bool gameIsEnding { get; private set; } = false;

        [Header("Variable References")]
        [SerializeField] private InputReader inputReader;
        // [SerializeField] private TextMeshProUGUI finnishWordTxt;
        [SerializeField] private TextMeshProUGUI swedishBaseWordTxt;
        [SerializeField] private TextMeshProUGUI conjugationClassTxt;
        [SerializeField] private TextMeshProUGUI instructionTxt;
        [SerializeField] private GameObject irregularHintObj;
        [SerializeField] private Button finnishHintBtn;
        [SerializeField] private TextMeshProUGUI finnishHintTxt;
        [SerializeField] private Button checkWordBtn;
        [SerializeField] private Button nextWordBtn;
        [SerializeField] private GameObject inputfieldHolder;
        [SerializeField] private GameObject singleInputfield;
        [SerializeField] private float newWordDelay = 1f;
        [SerializeField] private float gameEndDelay = 1.5f;

        //Input field related references
        private InputFieldHandling inputFieldHandling;
        private List<TMP_InputField> singleInputfields;
        private List<TextMeshProUGUI> fieldTextRefs;

        //Readonly
        private readonly Vector2 holderPos = new(0, -100f);
        private readonly string promptStart = "Taivuta muotoon:\n";

        private void Start()
        {
            wordFormsCount = System.Enum.GetNames(typeof(ConjugateInto)).Length;
        }

        public void InitializeGame(List<VerbWord> _verbList)
        {
            gameObject.SetActive(true);
            nextWordBtn.gameObject.SetActive(false);
            finnishHintTxt.text = "Hint";
            verbList = new(_verbList);
            correctWordsCount = 0;

            //Like and subscribe
            inputReader.SubmitEventCancelled += CheckWord;
            inputReader.SubmitEventHeld += NextWord;
            checkWordBtn.onClick.AddListener(CheckWord);
            nextWordBtn.onClick.AddListener(NextWord);
            finnishHintBtn.onClick.AddListener(ShowHint);

            InitializeNewWord();
        }

        private void InitializeNewWord()
        {
            //Get new active word & set relevant variables
            activeWord = verbList.Pop();
            wordWasChecked = false;
            conjugateInto = (ConjugateInto)Random.Range((int)ConjugateInto.imperfekti, wordFormsCount+1);
            instructionTxt.text = string.Concat(promptStart, conjugateInto);
            singleInputfields = new();
            fieldTextRefs = new();

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
                    $"conjugateInto value was set to {conjugateInto} when only values between 2-5 can be safely handled! Ref: {typeof(ConjugateInto)}");
                    Debug.LogError(errorMessage);
                    return;
            }

            //Update GUI
            if (!formIsRegular) irregularHintObj.SetActive(true);
            else irregularHintObj.SetActive(false);

            // finnishWordTxt.text = activeWord.finnishWord;   //CHANGE THIS TO WORK WITH THE CURRENT FORM
            swedishBaseWordTxt.text = activeWord.swedishWord;
            conjugationClassTxt.text = activeWord.conjugationClass.ToString();

            //Instantiate input field -related objects
            inputFieldHandling = Instantiate(inputfieldHolder, transform).GetComponent<InputFieldHandling>();
            inputFieldHandling.GetComponent<RectTransform>().localPosition = holderPos;
            List<char> chars = new();
            bool ignoreLetters = false;
            int indexer = 0;

            for (int i = 0; i < activeWordWantedForm.Length; i++)
            {
                //This part handles ignoring the word's highlight tags. Should eventually be replaced by a system
                //that builds the word without the highlight directly in the word's class
                if (activeWordWantedForm[i] == '<')
                {
                    ignoreLetters = true;
                    continue;
                }
                else if (activeWordWantedForm[i] == '>')
                {
                    ignoreLetters = false;
                    continue;
                }
                if (ignoreLetters) continue;
                int indexHolder = indexer;

                //Grab refs
                singleInputfields.Add(Instantiate(singleInputfield, inputFieldHandling.transform).GetComponent<TMP_InputField>());
                Debug.Log(indexer + " " + singleInputfields.Count.ToString());
                fieldTextRefs.Add(singleInputfields[indexer].transform.Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>());
                if (activeWordWantedForm[i] == ' ' || activeWordWantedForm == "")
                {
                    singleInputfields[indexer].interactable = false;
                    singleInputfields[indexer].image.enabled = false;
                }
                chars.Add(activeWordWantedForm[i]);

                //Like and subscribe
                singleInputfields[indexer].onValueChanged.AddListener((s) => inputFieldHandling.GoNextField());
                singleInputfields[indexer].onSelect.AddListener((s) => inputFieldHandling.GetActiveIndex(indexHolder));

                indexer++;
            }
            activeWordWantedFormNoHighlight = new(chars.ToArray());
            singleInputfields[0].ActivateInputField();
            Debug.Log($"Active word without highlight tags should be: {activeWordWantedFormNoHighlight}");
        }

        private void CheckWord()
        {
            if (gameIsEnding || !checkWordBtn.interactable) return;

            int correctLettersCount = 0;
            int wordLetterCount = 0;

            List<char> chars = new();
            for (int i = 0; i < activeWordWantedFormNoHighlight.Length; i++)
            {
                if (activeWordWantedFormNoHighlight[i] == ' ')
                {
                    chars.Add(' ');
                    continue;
                }
                chars.Add(singleInputfields[i].text[0]);
                wordLetterCount++;
            }
            string givenString = new(chars.ToArray());
            givenString.ToLower();

            for (int i = 0; i < activeWordWantedFormNoHighlight.Length; i++)
            {
                if (activeWordWantedFormNoHighlight[i] == ' ' || singleInputfields[i].text == "") continue;
                if (givenString[i] == activeWordWantedFormNoHighlight[i])
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

            if (correctLettersCount == wordLetterCount)
            {
                wordWasCorrect = true;      //PROBABLY PLAY SOME SORT OF AN EFFECT FOR CORRECT ANSWER HERE
                correctWordsCount++;
            }

            wordWasChecked = true;
            nextWordBtn.gameObject.SetActive(true);
        }

        private void NextWord()
        {
            if (!wordWasChecked || gameIsEnding) return;
            if (verbList == null || verbList.Count == 0)
            {
                EndGame();
                return;
            }
            DeleteOldWord();
            StartCoroutine(NextWordDelay());
        }

        private void DeleteOldWord()
        {
            Destroy(inputFieldHandling.gameObject);
        }

        private void EndGame()
        {
            //Shouldn't need to reset any variables as they're set at the start of the game anyway
            gameIsEnding = true;
            inputReader.SubmitEventCancelled -= CheckWord;
            inputReader.SubmitEventHeld -= NextWord;
            checkWordBtn.onClick.RemoveListener(CheckWord);
            nextWordBtn.onClick.RemoveListener(NextWord);
            Destroy(inputFieldHandling.gameObject);
            StartCoroutine(GameEndDelay());
        }

        private IEnumerator NextWordDelay()
        {
            checkWordBtn.interactable = false;
            finnishHintTxt.text = "Hint";
            nextWordBtn.gameObject.SetActive(false);
            yield return new WaitForSeconds(newWordDelay);
            checkWordBtn.interactable = true;
            InitializeNewWord();
        }

        private void ShowHint()
        {
            if (!checkWordBtn.interactable) return;
            activeWord.GetConjugatedFinnish(conjugateInto);
        }

        private IEnumerator GameEndDelay()
        {
            yield return new WaitForSeconds(gameEndDelay);
            gameObject.SetActive(false);
            gameIsEnding = false;
        }
    }
}
