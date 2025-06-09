using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SwedishApp.Input;
using SwedishApp.UI;
using SwedishApp.Words;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SwedishApp.Minigames
{
    public class DeclensionMinigame : MonoBehaviour
    {
        public enum DeclenateInto
        {
            indefinitiivi = 1,
            definitiivi,
            monikon_indefinitiivi,
            monikon_definitiivi
        }

        [Header("Variable References")]
        [SerializeField] private InputReader inputReader;
        [SerializeField] private Button abortBtn;
        [SerializeField] private TextMeshProUGUI swedishBaseWordTxt;
        [SerializeField] private TextMeshProUGUI declensionClassTxt;
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

        [Header("Lightmode-related")]
        [SerializeField] private Image conjugationClassImage;

        //Other game-related variables
        private DeclenateInto declenateInto;
        private int wordFormsCount;
        private Stack<NounWord> nounList;
        private NounWord activeWord;
        private string activeWordWantedForm;
        private string activeWordWantedFormNoHighlight;
        private bool formIsRegular;
        private bool wordWasChecked;
        private int correctWordsCount = 0;
        private bool wordWasCorrect = false;
        public bool gameIsEnding { get; private set; } = false;
        private List<TextMeshProUGUI> gameTextRefs;

        //Readonly
        private readonly Vector2 holderPos = new(0, -100f);
        private readonly string promptStart = "Taivuta muotoon:\n";
        private readonly string definitive = "definitiivi";
        private readonly string plural_indef = "monikon indefinitiivi";
        private readonly string plural_def = "monikon definitiivi";

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            wordFormsCount = System.Enum.GetNames(typeof(DeclenateInto)).Length;
            abortBtn.onClick.AddListener(() => EndGame(true));
        }

        #region lightmode-related

        /// <summary>
        /// This method sets every relevant UI element to its lightmode version
        /// </summary>
        private void ToLightmode()
        {
            abortBtn.image.sprite = UIManager.instance.abortSpriteLightmode;
            checkWordBtn.image.sprite = UIManager.instance.buttonSpriteLightmode;
            nextWordBtn.image.sprite = UIManager.instance.buttonSpriteLightmode;
            finnishHintBtn.image.sprite = UIManager.instance.buttonSpriteLightmode;
            conjugationClassImage.color = UIManager.instance.Darkgrey;
            gameTextRefs.ForEach((text) => text.color = UIManager.instance.Darkgrey);

            if (singleInputfields == null || singleInputfields.Count == 0) return;
            LightmodeInputFields();
        }

        /// <summary>
        /// This method sets every relevant UI element to its darkmode version
        /// </summary>
        private void ToDarkmode()
        {
            abortBtn.image.sprite = UIManager.instance.abortSpriteDarkmode;
            checkWordBtn.image.sprite = UIManager.instance.buttonSpriteDarkmode;
            nextWordBtn.image.sprite = UIManager.instance.buttonSpriteDarkmode;
            finnishHintBtn.image.sprite = UIManager.instance.buttonSpriteDarkmode;
            conjugationClassImage.color = UIManager.instance.Lightgrey;
            gameTextRefs.ForEach((text) => text.color = UIManager.instance.Lightgrey);

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
                colorBlock.normalColor = UIManager.instance.Darkgrey;
                colorBlock.selectedColor = UIManager.instance.LightmodeHighlight;
                colorBlock.highlightedColor = UIManager.instance.DarkgreyLighter;
                colorBlock.pressedColor = UIManager.instance.DarkgreyLighter;
                colorBlock.disabledColor = UIManager.instance.DarkgreyHalfAlpha;
                singleInputfields[i].colors = colorBlock;
                fieldTextRefs[i].color = UIManager.instance.Lightgrey;
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
                colorBlock.normalColor = UIManager.instance.Lightgrey;
                colorBlock.selectedColor = UIManager.instance.DarkmodeHighlight;
                colorBlock.highlightedColor = UIManager.instance.LightgreyDarker;
                colorBlock.pressedColor = UIManager.instance.LightgreyDarker;
                colorBlock.disabledColor = UIManager.instance.LightgreyHalfAlpha;
                singleInputfields[i].colors = colorBlock;
                fieldTextRefs[i].color = UIManager.instance.Darkgrey;
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
                text.font = UIManager.instance.legibleFont;
                text.characterSpacing = UIManager.instance.legibleSpacing;
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
                text.font = UIManager.instance.basicFont;
                text.characterSpacing = UIManager.instance.basicSpacing;
            });
            if (fieldTextRefs == null || fieldTextRefs.Count == 0) return;
            FieldsToBasicFont();
        }

        /// <summary>
        /// Sets input fields to legible font
        /// </summary>
        private void FieldsToHyperlegibleFont()
        {
            fieldTextRefs.ForEach((text) => text.font = UIManager.instance.legibleFont);
        }

        /// <summary>
        /// Sets input fields to basic font
        /// </summary>
        private void FieldsToBasicFont()
        {
            fieldTextRefs.ForEach((text) => text.font = UIManager.instance.basicFont);
        }

        #endregion

        #region game functionality

        /// <summary>
        /// This method initializes all the variables, GUI elements and event listeners
        /// relevant to the conjugation minigame. This is called to start the minigame, and
        /// eventually calls <see cref="InitializeNewWord"/> to show the first word.
        /// </summary>
        /// <param name="_verbList">List of VerbWord objects to use for this game</param>
        public void InitializeGame(List<NounWord> _nounList)
        {
            gameTextRefs = transform.GetComponentsInChildren<TextMeshProUGUI>(true).ToList();
            gameObject.SetActive(true);
            nextWordBtn.gameObject.SetActive(false);
            finnishHintTxt.text = "Hint";
            nounList = new(_nounList);
            correctWordsCount = 0;

            //Like and subscribe
            inputReader.SubmitEventCancelled += CheckWord;
            inputReader.SubmitEventHeld += NextWord;
            checkWordBtn.onClick.AddListener(CheckWord);
            nextWordBtn.onClick.AddListener(NextWord);
            finnishHintBtn.onClick.AddListener(ShowHint);
            UIManager.instance.LightmodeOnEvent += ToLightmode;
            UIManager.instance.LightmodeOffEvent += ToDarkmode;
            UIManager.instance.LegibleModeOnEvent += ToHyperlegibleFont;
            UIManager.instance.LegibleModeOffEvent += ToBasicFont;

            //set initial colors dependending on if lightmode is on or off
            if (UIManager.instance.LightmodeOn) ToLightmode();
            else ToDarkmode();
            if (UIManager.instance.hyperlegibleOn) ToHyperlegibleFont();
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
            activeWord = nounList.Pop();
            wordWasChecked = false;
            wordWasCorrect = false;
            declenateInto = (DeclenateInto)Random.Range((int)DeclenateInto.definitiivi, wordFormsCount + 1);
            singleInputfields = new();
            fieldTextRefs = new();

            //UGLY SWITCH CASE WARNING
            switch (declenateInto)
            {
                case DeclenateInto.definitiivi:
                    formIsRegular = true;
                    activeWordWantedForm = activeWord.NounWithGenderEnd();
                    instructionTxt.text = string.Concat(promptStart, definitive);
                    break;
                case DeclenateInto.monikon_indefinitiivi:
                    formIsRegular = activeWord.wordPluralIsRegular;
                    activeWordWantedForm = activeWord.PluralNoun();
                    instructionTxt.text = string.Concat(promptStart, plural_indef);
                    break;
                case DeclenateInto.monikon_definitiivi:
                    formIsRegular = activeWord.wordDefinitivePluralIsRegular;
                    activeWordWantedForm = activeWord.PluralDefinitiveNoun();
                    instructionTxt.text = string.Concat(promptStart, plural_def);
                    break;
                default:
                    string errorMessage = string.Concat($"Something went horribly wrong with randomly determining word declension form:\n",
                    $"declenateInto value was set to {(int)declenateInto} when only values between 2-4 can be safely handled! Ref: {typeof(DeclenateInto)}");
                    Debug.LogError(errorMessage);
                    return;
            }

            //Update GUI
            if (!formIsRegular) irregularHintObj.SetActive(true);
            else irregularHintObj.SetActive(false);
            swedishBaseWordTxt.text = activeWord.swedishWord;
            declensionClassTxt.text = activeWord.declensionClass.ToString();

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
            if (UIManager.instance.LightmodeOn) LightmodeInputFields();
            else DarkmodeInputFields();
            if (UIManager.instance.hyperlegibleOn) FieldsToHyperlegibleFont();
            else FieldsToBasicFont();
            singleInputfields[0].ActivateInputField();
        }

        /// <summary>
        /// This method handles checking if the word written in the input fields is correct.
        /// </summary>
        private void CheckWord()
        {
            if (gameIsEnding || !checkWordBtn.interactable) return;

            wordWasCorrect = false;
            int correctLettersCount = 0;
            int wordLetterCount = 0;

            //This bit is used to make sure that capitalized letters are also treated as correct
            List<char> chars = new();
            for (int i = 0; i < activeWordWantedFormNoHighlight.Length; i++)
            {
                if (activeWordWantedFormNoHighlight[i] == ' ')
                {
                    chars.Add(' ');
                    continue;
                }
                if (singleInputfields[i].text != "") chars.Add(singleInputfields[i].text[0]);
                else chars.Add(' ');
                wordLetterCount++;
            }
            string givenString = new(chars.ToArray());
            givenString.ToLower();

            //Enable/disable indicators to show if a letter was correct or not, also add to a counter of
            //correct letters, used for determining if the whole word was correct.
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
                wordWasCorrect = true;
                correctWordsCount++;
            }

            wordWasChecked = true;
            nextWordBtn.gameObject.SetActive(true);

            if (wordWasCorrect) Debug.Log("Word was correct!");
        }

        /// <summary>
        /// If there are words remaining in the word list, go to next word. Else, end game.
        /// </summary>
        private void NextWord()
        {
            if (!wordWasChecked || gameIsEnding) return;
            if (nounList == null || nounList.Count == 0)
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

        /// <summary>
        /// This method unsubscribes events that would break things, destroys the input fields,
        /// and resets some variables.
        /// </summary>
        /// <param name="_endInstantly">If true, ends game immediately, defaults to false
        /// to give time for an end-of-game animation to be played</param>
        private void EndGame(bool _endInstantly = false)
        {
            //Shouldn't need to reset any variables as they're set at the start of the game anyway
            inputReader.SubmitEventCancelled -= CheckWord;
            inputReader.SubmitEventHeld -= NextWord;
            checkWordBtn.onClick.RemoveListener(CheckWord);
            nextWordBtn.onClick.RemoveListener(NextWord);
            Destroy(inputFieldHandling.gameObject);
            singleInputfields = null;
            fieldTextRefs = null;

            if (!_endInstantly)
            {
                gameIsEnding = true;
                StartCoroutine(GameEndDelay());
            }
            else
            {
                gameObject.SetActive(false);
            }
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
            finnishHintTxt.text = activeWord.GetDeclenatedFinnish(declenateInto);
        }

        /// <summary>
        /// Coroutine, delays ending the game for a little end-of-game animation
        /// </summary>
        /// <returns></returns>
        private IEnumerator GameEndDelay()
        {
            yield return new WaitForSeconds(gameEndDelay);
            gameObject.SetActive(false);
            gameIsEnding = false;
        }

        #endregion
    }
}