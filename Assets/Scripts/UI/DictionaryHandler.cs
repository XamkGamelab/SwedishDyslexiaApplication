using System.Collections.Generic;
using SwedishApp.Words;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SwedishApp.UI
{
    public class DictionaryHandler : MonoBehaviour
    {
        [SerializeField] private Transform verbHolder;
        [SerializeField] private Transform nounHolder;
        [SerializeField] private Transform adjectiveHolder;
        [SerializeField] private Transform timeWordHolder;
        [SerializeField] private Transform numberHolder;
        [SerializeField] private Transform grammarHolder;
        [SerializeField] private Transform pronounHolder;
        [SerializeField] private Transform phraseHolder;
        [SerializeField] private GameObject spacer;
        [SerializeField] private GameObject verbEntry;
        [SerializeField] private GameObject nounEntry;
        [SerializeField] private GameObject baseEntry;
        [SerializeField] private VerbList verbList;
        [SerializeField] private NounList nounList;
        [SerializeField] private AdjectiveList adjectiveList;
        [SerializeField] private TimeList timeList;
        [SerializeField] private NumberList numberList;
        [SerializeField] private GrammarList grammarList;
        [SerializeField] private PronounList pronounList;
        [SerializeField] private PhraseList phraseList;
        [SerializeField] private Button closeButton;
        private List<TextMeshProUGUI> textFields;
        private List<Image> spacers;

        public void InitializeDictionary()
        {
            textFields = new();
            spacers = new();

            verbList.verbList.ForEach(verb =>
            {
                DictionaryEntryWithForm entry = Instantiate(verbEntry, verbHolder).GetComponent<DictionaryEntryWithForm>();
                entry.FinnishWordTxt.text = verb.finnishWord;
                entry.SwedishWordTxt.text = verb.swedishWord;
                entry.WordClassTxt.text = verb.conjugationClass.ToString();
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
                textFields.Add(entry.WordClassTxt);
                spacers.Add(Instantiate(spacer, verbHolder).GetComponent<Image>());
            });

            nounList.nounList.ForEach(noun =>
            {
                DictionaryEntryWithForm entry = Instantiate(nounEntry, nounHolder).GetComponent<DictionaryEntryWithForm>();
                entry.FinnishWordTxt.text = noun.finnishWord;
                entry.SwedishWordTxt.text = noun.swedishWord;
                entry.WordClassTxt.text = noun.declensionClass.ToString();
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
                textFields.Add(entry.WordClassTxt);
                spacers.Add(Instantiate(spacer, nounHolder).GetComponent<Image>());
            });

            adjectiveList.adjectiveList.ForEach(adjective =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, adjectiveHolder).GetComponent<DictionaryEntry>();
                entry.FinnishWordTxt.text = adjective.finnishWord;
                entry.SwedishWordTxt.text = adjective.swedishWord;
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
                spacers.Add(Instantiate(spacer, adjectiveHolder).GetComponent<Image>());
            });

            timeList.timeList.ForEach(time =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, timeWordHolder).GetComponent<DictionaryEntry>();
                entry.FinnishWordTxt.text = time.finnishWord;
                entry.SwedishWordTxt.text = time.swedishWord;
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
                spacers.Add(Instantiate(spacer, timeWordHolder).GetComponent<Image>());
            });

            numberList.numberList.ForEach(number =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, numberHolder).GetComponent<DictionaryEntry>();
                entry.FinnishWordTxt.text = number.finnishWord;
                entry.SwedishWordTxt.text = number.swedishWord;
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
                spacers.Add(Instantiate(spacer, numberHolder).GetComponent<Image>());
            });

            grammarList.grammarList.ForEach(grammar =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, grammarHolder).GetComponent<DictionaryEntry>();
                entry.FinnishWordTxt.text = grammar.finnishWord;
                entry.SwedishWordTxt.text = grammar.swedishWord;
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
                spacers.Add(Instantiate(spacer, grammarHolder).GetComponent<Image>());
            });

            pronounList.pronounList.ForEach(pronoun =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, pronounHolder).GetComponent<DictionaryEntry>();
                entry.FinnishWordTxt.text = pronoun.finnishWord;
                entry.SwedishWordTxt.text = pronoun.swedishWord;
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
                spacers.Add(Instantiate(spacer, pronounHolder).GetComponent<Image>());
            });

            phraseList.phraseList.ForEach(phrase =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, phraseHolder).GetComponent<DictionaryEntry>();
                entry.FinnishWordTxt.text = phrase.finnishWord;
                entry.SwedishWordTxt.text = phrase.swedishWord;
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
                spacers.Add(Instantiate(spacer, phraseHolder).GetComponent<Image>());
            });

            closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        public List<TextMeshProUGUI> GetDictionaryTextFields()
        {
            return textFields;
        }

        public List<Image> GetSpacerImages()
        {
            return spacers;
        }
    }
}
