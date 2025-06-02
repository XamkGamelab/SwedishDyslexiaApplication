using System.Collections;
using System.Collections.Generic;
using SwedishApp.Input;
using SwedishApp.Words;
using TMPro;
using UnityEngine;

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

        [Header("Variable References")]
        [SerializeField] private TextMeshProUGUI finnishWordTxt;
        [SerializeField] private TextMeshProUGUI swedishBaseWordTxt;
        [SerializeField] private TextMeshProUGUI conjugationClassTxt;
        [SerializeField] private GameObject inputfieldHolder;
        [SerializeField] private GameObject singleInputfield;
        private InputFieldHandling inputFieldHandling;
        private List<TMP_InputField> singleInputfields;
        private List<TextMeshProUGUI> fieldTextRefs;

        private void Start()
        {
            wordFormsCount = System.Enum.GetNames(typeof(ConjugateInto)).Length;
        }

        public void InitializeGame(List<VerbWord> _verbList, ConjugateInto _conjugateInto)
        {
            gameObject.SetActive(true);
            conjugateInto = _conjugateInto;
            verbList = new(_verbList);
        }

        private void NewTask(bool _overrideRequirement = false)
        {
            //Check if this method can be reasonably run; if not, return
            if (!wordWasChecked && !_overrideRequirement) return;

            //Get new active word & set relevant variables
            activeWord = verbList.Pop();
            wordWasChecked = false;
            conjugateInto = (ConjugateInto)Random.Range((int)ConjugateInto.preesens, wordFormsCount);
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
                    activeWordWantedForm = activeWord.PastTenseWord();
                    break;
                case ConjugateInto.pluskvamperfekti:
                    formIsRegular = activeWord.pastPlusPerfectTenseIsRegular;
                    activeWordWantedForm = activeWord.PastPlusPerfectTenseWord();
                    break;
                default:
                    string errorMessage = string.Concat($"Something went horribly wrong with randomly determining word conjugation form:\n",
                    $"conjugateInto value was set to {conjugateInto} when only values between 2-5 can be safely handled!");
                    Debug.LogError(errorMessage);
                    return;
            }

            //Update GUI
            finnishWordTxt.text = activeWord.finnishWord;   //CHANGE THIS TO WORK WITH THE CURRENT FORM
            swedishBaseWordTxt.text = activeWord.swedishWord;
            conjugationClassTxt.text = string.Concat("Taivutusluokka ", activeWord.conjugationClass.ToString());

            //Instantiate input field -related objects
            inputFieldHandling = Instantiate(inputfieldHolder, transform).GetComponent<InputFieldHandling>();
            List<char> chars = new();
            bool ignoreLetters = false;

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

                singleInputfields.Add(Instantiate(singleInputfield, inputFieldHandling.transform).GetComponent<TMP_InputField>());
                fieldTextRefs.Add(singleInputfields[i].transform.Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>());
                chars.Add(activeWordWantedForm[i]);
            }
            activeWordWantedFormNoHighlight = new(chars.ToArray());
            Debug.Log($"Active word without highlight tags should be: {activeWordWantedFormNoHighlight}");
        }

        private void CheckWord()
        {
            if (verbList == null || verbList.Count == 0)
            {
                EndGame();
            }
            
            wordWasChecked = true;
        }

        private void EndGame()
        {

        }
    }
}
