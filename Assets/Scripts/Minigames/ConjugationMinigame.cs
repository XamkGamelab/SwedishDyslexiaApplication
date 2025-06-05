using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using SwedishApp.Input;
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
        private List<TextMeshProUGUI> gameTextRefs;

        //Readonly
        private readonly Vector2 holderPos = new(0, -100f);
        private readonly string promptStart = "Taivuta muotoon:\n";

        private void Start()
        {
            wordFormsCount = System.Enum.GetNames(typeof(ConjugateInto)).Length;
        }

        #region lightmode-related

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

        private void FieldsToHyperlegibleFont()
        {
            fieldTextRefs.ForEach((text) => text.font = UIManager.instance.legibleFont);
        }
        
        private void FieldsToBasicFont()
        {
            fieldTextRefs.ForEach((text) => text.font = UIManager.instance.basicFont);
        }

        #endregion

        #region game functionality

        public void InitializeGame(List<VerbWord> _verbList)
        {
            gameTextRefs = transform.GetComponentsInChildren<TextMeshProUGUI>(true).ToList();
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

        private void InitializeNewWord()
        {
            //Get new active word & set relevant variables
            activeWord = verbList.Pop();
            wordWasChecked = false;
            conjugateInto = (ConjugateInto)Random.Range((int)ConjugateInto.imperfekti, wordFormsCount + 1);
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
            if (UIManager.instance.LightmodeOn) LightmodeInputFields();
            else DarkmodeInputFields();
            if (UIManager.instance.hyperlegibleOn) FieldsToHyperlegibleFont();
            else FieldsToBasicFont();
            singleInputfields[0].ActivateInputField();
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
            singleInputfields = null;
            fieldTextRefs = null;
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
            finnishHintTxt.text = activeWord.GetConjugatedFinnish(conjugateInto);
        }

        private IEnumerator GameEndDelay()
        {
            yield return new WaitForSeconds(gameEndDelay);
            gameObject.SetActive(false);
            gameIsEnding = false;
        }
        
        #endregion
    }
}
