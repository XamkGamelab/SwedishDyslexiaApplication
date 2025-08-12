using System.Collections;
using System.Collections.Generic;
using SwedishApp.Words;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SwedishApp.UI
{
    public class DictionaryFormHolder : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
    {
        [SerializeField] private GameObject wordFormPrefab;
        [HideInInspector] public DictionaryFormEnabler lastEnabler;
        private List<TextMeshProUGUI> currentFields;
        [HideInInspector] public bool mouseOnHolder = true;

        public void InitHolder(VerbWord _verb, DictionaryFormEnabler _lastEnabler)
        {
            string[] wordForms =
            {
                _verb.BaseformWord(),
                _verb.CurrentTenseWord(),
                _verb.PastTenseWord(),
                _verb.PastPerfectTenseWord(),
                _verb.PastPlusPerfectTenseWord()
            };

            InitWords(wordForms);
        }

        public void InitHolder(NounWord _noun, DictionaryFormEnabler _lastEnabler)
        {
            string[] wordForms =
            {
                _noun.NounWithGenderStart(),
                _noun.NounWithGenderEnd(),
                _noun.PluralNoun(),
                _noun.PluralDefinitiveNoun()
            };

            InitWords(wordForms);
        }

        public void InitHolder(AdjectiveWord _adjective, DictionaryFormEnabler _lastEnabler)
        {
            string[] wordForms =
            {
                _adjective.HighlightedSwedishWord(),
                _adjective.AdjectiveComparative(),
                _adjective.AdjectiveSuperlative()
            };

            InitWords(wordForms);
        }

        public void InitWords(string[] _words)
        {
            if (currentFields == null) currentFields = new();
            else
            {
                UIManager.Instance.RemoveFromTextLists(currentFields);
                currentFields = new();
            }

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < _words.Length; i++)
            {
                TextMeshProUGUI wordForm = Instantiate(wordFormPrefab, transform).GetComponent<TextMeshProUGUI>();
                wordForm.text = _words[i];
                currentFields.Add(wordForm);
                if (UIManager.Instance.HyperlegibleOn)
                {
                    wordForm.font = UIManager.Instance.LegibleFont;
                    wordForm.characterSpacing = UIManager.Instance.LegibleSpacing;
                }
                else
                {
                    wordForm.font = UIManager.Instance.BasicFont;
                    wordForm.characterSpacing = UIManager.Instance.BasicSpacing;
                }
                if (UIManager.Instance.LightmodeOn) wordForm.color = UIManager.Instance.Darkgrey;
                else wordForm.color = UIManager.Instance.Lightgrey;
                
                UIManager.Instance.FixTextSpacing(wordForm);
            }
            UIManager.Instance.AddToTextLists(currentFields);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            mouseOnHolder = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            mouseOnHolder = false;
        }
    }
}
