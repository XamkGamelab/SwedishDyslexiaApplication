using System.Collections.Generic;
using System.Linq;
using SwedishApp.Input;
using SwedishApp.Words;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SwedishApp.UI
{
    public class DictionaryHandler : MonoBehaviour
    {
        [Header("UI")]
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
        [SerializeField] private TextMeshProUGUI verbHeader, nounHeader, adjectiveHeader, numberHeader,
        timeHeader, grammarHeader, pronounHeader, phraseHeader;
        private List<DictionaryEntry> dictionaryEntries;
        private List<TextMeshProUGUI> textFields;
        private List<Image> spacers;

        [Header("Other variables")]
        [SerializeField] private Button searchButton;
        [SerializeField] private TMP_InputField searchField;

        private void DictionarySearcher(string _searchTerm)
        {
            foreach (DictionaryEntry entry in dictionaryEntries)
            {
                if (entry.FinnishWordTxt.text.Contains(_searchTerm, System.StringComparison.CurrentCultureIgnoreCase)
                    || entry.SwedishWordTxt.text.Contains(_searchTerm, System.StringComparison.CurrentCultureIgnoreCase))
                {

                }
                else
                {

                }
            }
        }

        /// <summary>
        /// This method hides all entries that don't include the term in the search bar.
        /// </summary>
        private void CallDictionarySearch(string _searchTerm)
        {
            DictionarySearcher(_searchTerm);
        }

        private void SearchFromButton()
        {
            CallDictionarySearch(searchField.text);
        }

        public void InitializeDictionary()
        {
            textFields = new();
            spacers = new();
            searchButton.onClick.AddListener(SearchFromButton);
            searchField.onSubmit.AddListener(CallDictionarySearch);

            verbList.verbList.ForEach(verb =>
            {
                DictionaryEntryWithForm entry = Instantiate(verbEntry, verbHolder).GetComponent<DictionaryEntryWithForm>();
                entry.FinnishWordTxt.text = verb.finnishWord;
                entry.SwedishWordTxt.text = verb.swedishWord;
                entry.WordClassTxt.text = verb.conjugationClass.ToString();
                dictionaryEntries.Add(entry);
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
                dictionaryEntries.Add(entry);
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
                dictionaryEntries.Add(entry);
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
                spacers.Add(Instantiate(spacer, adjectiveHolder).GetComponent<Image>());
            });

            timeList.timeList.ForEach(time =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, timeWordHolder).GetComponent<DictionaryEntry>();
                entry.FinnishWordTxt.text = time.finnishWord;
                entry.SwedishWordTxt.text = time.swedishWord;
                dictionaryEntries.Add(entry);
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
                spacers.Add(Instantiate(spacer, timeWordHolder).GetComponent<Image>());
            });

            numberList.numberList.ForEach(number =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, numberHolder).GetComponent<DictionaryEntry>();
                entry.FinnishWordTxt.text = number.finnishWord;
                entry.SwedishWordTxt.text = number.swedishWord;
                dictionaryEntries.Add(entry);
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
                spacers.Add(Instantiate(spacer, numberHolder).GetComponent<Image>());
            });

            grammarList.grammarList.ForEach(grammar =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, grammarHolder).GetComponent<DictionaryEntry>();
                entry.FinnishWordTxt.text = grammar.finnishWord;
                entry.SwedishWordTxt.text = grammar.swedishWord;
                dictionaryEntries.Add(entry);
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
                spacers.Add(Instantiate(spacer, grammarHolder).GetComponent<Image>());
            });

            pronounList.pronounList.ForEach(pronoun =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, pronounHolder).GetComponent<DictionaryEntry>();
                entry.FinnishWordTxt.text = pronoun.finnishWord;
                entry.SwedishWordTxt.text = pronoun.swedishWord;
                dictionaryEntries.Add(entry);
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
                spacers.Add(Instantiate(spacer, pronounHolder).GetComponent<Image>());
            });

            phraseList.phraseList.ForEach(phrase =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, phraseHolder).GetComponent<DictionaryEntry>();
                entry.FinnishWordTxt.text = phrase.finnishWord;
                entry.SwedishWordTxt.text = phrase.swedishWord;
                dictionaryEntries.Add(entry);
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
