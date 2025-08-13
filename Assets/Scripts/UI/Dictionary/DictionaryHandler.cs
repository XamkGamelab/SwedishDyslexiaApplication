using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [SerializeField] private Transform adverbHolder;
        [SerializeField] private Transform prepositionHolder;
        [SerializeField] private Transform questionHolder;
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
        [SerializeField] private GrammarList adverbList;
        [SerializeField] private GrammarList prepositionList;
        [SerializeField] private GrammarList questionList;
        [SerializeField] private PronounList pronounList;
        [SerializeField] private PhraseList phraseList;
        [SerializeField] private Button closeButton;
        [SerializeField]
        private GameObject verbHeader, nounHeader, adjectiveHeader,
        timeHeader, numberHeader, grammarHeader, adverbHeader,
        prepositionHeader, questionHeader, pronounHeader, phraseHeader;
        [SerializeField] private DictionaryFormHolder wordFormHolder;
        private List<GameObject> headerObjects;
        private Dictionary<DictionaryEntry, Image> dictionaryEntries;
        private Dictionary<DictionaryEntry, Image> matches;
        private List<TextMeshProUGUI> textFields;
        private List<Image> spacers;
        [SerializeField] private Image searchImg;
        [SerializeField] private Sprite searchSpriteLightmode;
        [SerializeField] private Sprite searchSpriteDarkmode;

        [Header("Other variables")]
        [SerializeField] private Button searchButton;
        [SerializeField] private TMP_InputField searchField;
        [SerializeField] private float searchClearDelay = 0.25f;
        [SerializeField] private int maxComparisonsPerUpdate = 30;
        private bool searchClearWaiting = false;
        private bool searchInProgress = false;
        private bool allObjectsActive = true;

        private void Start()
        {
            searchField.onValueChanged.AddListener((s) => StartSearchClearCoroutine());
            UIManager.Instance.LightmodeOnEvent += ToLightmode;
            UIManager.Instance.LightmodeOffEvent += ToDarkmode;
            closeButton.onClick.AddListener(() =>
            {
                UIManager.Instance.TriggerTipChange();
                gameObject.SetActive(false);
            });
        }

        /// <summary>
        /// This method returns a list of dictionary entries matching a string
        /// </summary>
        private IEnumerator DictionarySearcher(string _searchTerm)
        {
            int loopCount = 0;
            matches = new();
            searchInProgress = true;

            foreach (var keyValuePair in dictionaryEntries)
            {
                string finCleanWord = GetCleanedWord(keyValuePair.Key.FinnishWordTxt.text);
                string sweCleanWord = GetCleanedWord(keyValuePair.Key.SwedishWordTxt.text);

                if (finCleanWord.Contains(_searchTerm, System.StringComparison.CurrentCultureIgnoreCase)
                 || sweCleanWord.Contains(_searchTerm, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    matches.Add(keyValuePair.Key, keyValuePair.Value);
                }

                loopCount++;
                if (loopCount == maxComparisonsPerUpdate)
                {
                    loopCount = 0;
                    yield return null;
                }
            }

            yield return null;

            foreach (var keyValuePair in dictionaryEntries)
            {
                keyValuePair.Key.gameObject.SetActive(false);
                keyValuePair.Value.gameObject.SetActive(false);
            }

            headerObjects.ForEach((header) => header.SetActive(false));

            foreach (var keyValuePair in matches)
            {
                keyValuePair.Key.gameObject.SetActive(true);
                keyValuePair.Value.gameObject.SetActive(true);
                switch (keyValuePair.Key.wordType)
                {
                    case DictionaryEntry.WordType.verb:
                        verbHeader.SetActive(true);
                        break;
                    case DictionaryEntry.WordType.noun:
                        nounHeader.SetActive(true);
                        break;
                    case DictionaryEntry.WordType.adjective:
                        adjectiveHeader.SetActive(true);
                        break;
                    case DictionaryEntry.WordType.time:
                        timeHeader.SetActive(true);
                        break;
                    case DictionaryEntry.WordType.number:
                        numberHeader.SetActive(true);
                        break;
                    case DictionaryEntry.WordType.grammar:
                        grammarHeader.SetActive(true);
                        break;
                    case DictionaryEntry.WordType.adverb:
                        adverbHeader.SetActive(true);
                        break;
                    case DictionaryEntry.WordType.preposition:
                        prepositionHeader.SetActive(true);
                        break;
                    case DictionaryEntry.WordType.question:
                        questionHeader.SetActive(true);
                        break;
                    case DictionaryEntry.WordType.pronoun:
                        pronounHeader.SetActive(true);
                        break;
                    case DictionaryEntry.WordType.phrase:
                        phraseHeader.SetActive(true);
                        break;
                }
            }

            searchField.ActivateInputField();
            searchInProgress = false;
            allObjectsActive = false;
        }
        private void CallDictionarySearch(string _searchTerm)
        {
            if (searchInProgress) return;
            if (_searchTerm == "") ReactivateEntries();
            else StartCoroutine(DictionarySearcher(_searchTerm));
        }

        private void SearchFromButton()
        {
            if (searchInProgress) return;
            if (searchField.text == "") ReactivateEntries();
            else StartCoroutine(DictionarySearcher(searchField.text));
        }

        private void StartSearchClearCoroutine()
        {
            if (searchField.text != "")
            {
                searchClearWaiting = false;
                return;
            }
            else if (!searchClearWaiting)
            {
                searchClearWaiting = true;
                StartCoroutine(SearchClearWait());
            }
        }

        private IEnumerator SearchClearWait()
        {
            float timer = 0f;
            while (timer < searchClearDelay)
            {
                if (!searchClearWaiting || allObjectsActive) yield break;
                timer += Time.deltaTime;
                yield return null;
            }

            ReactivateEntries();

            searchClearWaiting = false;
        }

        private void ReactivateEntries()
        {
            if (allObjectsActive) return;
            headerObjects.ForEach((header) => header.SetActive(true));
            foreach (var keyValuePair in dictionaryEntries)
            {
                keyValuePair.Key.gameObject.SetActive(true);
                keyValuePair.Value.gameObject.SetActive(true);
            }
            allObjectsActive = true;
        }

        public void InitializeDictionary()
        {
            textFields = new();
            spacers = new();
            dictionaryEntries = new();
            headerObjects = new()
            {
                verbHeader,
                nounHeader,
                adjectiveHeader,
                timeHeader,
                numberHeader,
                grammarHeader,
                adverbHeader,
                prepositionHeader,
                questionHeader,
                pronounHeader,
                phraseHeader
            };
            searchButton.onClick.AddListener(SearchFromButton);
            searchField.onSubmit.AddListener(CallDictionarySearch);

            verbList.verbList.ForEach(verb =>
            {
                DictionaryEntryWithForm entry = Instantiate(verbEntry, verbHolder).GetComponent<DictionaryEntryWithForm>();
                Image _spacer = Instantiate(spacer, verbHolder).GetComponent<Image>();
                entry.FinnishWordTxt.text = verb.finnishWord;
                entry.SwedishWordTxt.text = verb.swedishWord;
                entry.WordClassTxt.text = verb.conjugationClass.ToString();
                entry.wordType = DictionaryEntry.WordType.verb;
                dictionaryEntries.Add(entry, _spacer);
                DictionaryFormEnabler formEnabler = entry.SwedishWordTxt.GetComponent<DictionaryFormEnabler>();
                formEnabler.wordFormHolder = wordFormHolder;
                formEnabler.Init(verb);
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
                textFields.Add(entry.WordClassTxt);
            });

            nounList.nounList.ForEach(noun =>
            {
                DictionaryEntryWithForm entry = Instantiate(nounEntry, nounHolder).GetComponent<DictionaryEntryWithForm>();
                Image _spacer = Instantiate(spacer, nounHolder).GetComponent<Image>();
                entry.FinnishWordTxt.text = noun.finnishWord;
                entry.SwedishWordTxt.text = noun.swedishWord;
                entry.WordClassTxt.text = noun.declensionClass.ToString();
                entry.wordType = DictionaryEntry.WordType.noun;
                dictionaryEntries.Add(entry, _spacer);
                DictionaryFormEnabler formEnabler = entry.SwedishWordTxt.GetComponent<DictionaryFormEnabler>();
                formEnabler.wordFormHolder = wordFormHolder;
                formEnabler.Init(noun);
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
                textFields.Add(entry.WordClassTxt);
            });

            adjectiveList.adjectiveList.ForEach(adjective =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, adjectiveHolder).GetComponent<DictionaryEntry>();
                Image _spacer = Instantiate(spacer, adjectiveHolder).GetComponent<Image>();
                entry.FinnishWordTxt.text = adjective.finnishWord;
                entry.SwedishWordTxt.text = adjective.swedishWord;
                entry.wordType = DictionaryEntry.WordType.adjective;
                dictionaryEntries.Add(entry, _spacer);
                DictionaryFormEnabler formEnabler = entry.SwedishWordTxt.GetComponent<DictionaryFormEnabler>();
                formEnabler.wordFormHolder = wordFormHolder;
                formEnabler.Init(adjective);
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
            });

            timeList.timeList.ForEach(time =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, timeWordHolder).GetComponent<DictionaryEntry>();
                Image _spacer = Instantiate(spacer, timeWordHolder).GetComponent<Image>();
                entry.FinnishWordTxt.text = time.finnishWord;
                entry.SwedishWordTxt.text = time.swedishWord;
                entry.wordType = DictionaryEntry.WordType.time;
                dictionaryEntries.Add(entry, _spacer);
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
            });

            numberList.numberList.ForEach(number =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, numberHolder).GetComponent<DictionaryEntry>();
                Image _spacer = Instantiate(spacer, numberHolder).GetComponent<Image>();
                entry.FinnishWordTxt.text = number.finnishWord;
                entry.SwedishWordTxt.text = number.swedishWord;
                entry.wordType = DictionaryEntry.WordType.number;
                dictionaryEntries.Add(entry, _spacer);
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
            });

            grammarList.grammarList.ForEach(grammar =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, grammarHolder).GetComponent<DictionaryEntry>();
                Image _spacer = Instantiate(spacer, grammarHolder).GetComponent<Image>();
                entry.FinnishWordTxt.text = grammar.finnishWord;
                entry.SwedishWordTxt.text = grammar.swedishWord;
                entry.wordType = DictionaryEntry.WordType.grammar;
                dictionaryEntries.Add(entry, _spacer);
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
            });

            adverbList.grammarList.ForEach(adverb =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, adverbHolder).GetComponent<DictionaryEntry>();
                Image _spacer = Instantiate(spacer, adverbHolder).GetComponent<Image>();
                entry.FinnishWordTxt.text = adverb.finnishWord;
                entry.SwedishWordTxt.text = adverb.swedishWord;
                entry.wordType = DictionaryEntry.WordType.adverb;
                dictionaryEntries.Add(entry, _spacer);
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
            });

            prepositionList.grammarList.ForEach(preposition =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, prepositionHolder).GetComponent<DictionaryEntry>();
                Image _spacer = Instantiate(spacer, prepositionHolder).GetComponent<Image>();
                entry.FinnishWordTxt.text = preposition.finnishWord;
                entry.SwedishWordTxt.text = preposition.swedishWord;
                entry.wordType = DictionaryEntry.WordType.preposition;
                dictionaryEntries.Add(entry, _spacer);
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
            });

            questionList.grammarList.ForEach(question =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, questionHolder).GetComponent<DictionaryEntry>();
                Image _spacer = Instantiate(spacer, questionHolder).GetComponent<Image>();
                entry.FinnishWordTxt.text = question.finnishWord;
                entry.SwedishWordTxt.text = question.swedishWord;
                entry.wordType = DictionaryEntry.WordType.question;
                dictionaryEntries.Add(entry, _spacer);
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
            });

            pronounList.pronounList.ForEach(pronoun =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, pronounHolder).GetComponent<DictionaryEntry>();
                Image _spacer = Instantiate(spacer, pronounHolder).GetComponent<Image>();
                entry.FinnishWordTxt.text = pronoun.finnishWord;
                entry.SwedishWordTxt.text = pronoun.swedishWord;
                entry.wordType = DictionaryEntry.WordType.pronoun;
                dictionaryEntries.Add(entry, _spacer);
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
            });

            phraseList.phraseList.ForEach(phrase =>
            {
                DictionaryEntry entry = Instantiate(baseEntry, phraseHolder).GetComponent<DictionaryEntry>();
                Image _spacer = Instantiate(spacer, phraseHolder).GetComponent<Image>();
                entry.FinnishWordTxt.text = phrase.finnishWord;
                entry.SwedishWordTxt.text = phrase.swedishWord;
                entry.wordType = DictionaryEntry.WordType.phrase;
                dictionaryEntries.Add(entry, _spacer);
                textFields.Add(entry.FinnishWordTxt);
                textFields.Add(entry.SwedishWordTxt);
            });
        }

        public List<TextMeshProUGUI> GetDictionaryTextFields()
        {
            return textFields;
        }

        public List<Image> GetSpacerImages()
        {
            return dictionaryEntries.Values.ToList();
        }

        private string GetCleanedWord(string _oldWord)
        {
            string wordToClean = _oldWord.Replace(@"\u00AD", null);
            List<char> letterList = new();
            bool ignoreLetters = false;

            foreach (char c in wordToClean)
            {
                if (c == '<')
                {
                    ignoreLetters = true;
                    continue;
                }
                else if (c == '>')
                {
                    ignoreLetters = false;
                    continue;
                }
                else if (ignoreLetters)
                {
                    continue;
                }

                letterList.Add(c);
            }

            return new(letterList.ToArray());
        }

        private void ToLightmode()
        {
            searchImg.sprite = searchSpriteLightmode;
            searchField.selectionColor = UIManager.Instance.LightmodeHighlightTransparent;
        }

        private void ToDarkmode()
        {
            searchImg.sprite = searchSpriteDarkmode;
            searchField.selectionColor = UIManager.Instance.DarkmodeHighlightTransparent;
        }
    }
}
