using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SwedishApp.UI
{
    public class DictionaryFormHolder : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
    {
        [SerializeField] private Transform wordHolder;
        [SerializeField] private GameObject wordFormPrefab;
        [HideInInspector] public DictionaryFormEnabler lastEnabler;
        private List<TextMeshProUGUI> currentFields;
        public bool mouseOnHolder = true;
        public bool hasBeenHovered = false;

        public void InitWords(string[] _words, DictionaryFormEnabler _lastEnabler)
        {
            if (lastEnabler == _lastEnabler) return;
            lastEnabler = _lastEnabler;

            if (currentFields == null) currentFields = new();
            else
            {
                UIManager.instance.RemoveFromTextLists(currentFields);
                currentFields = new();
            }

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < _words.Length; i++)
            {
                TextMeshProUGUI wordForm = Instantiate(wordFormPrefab, wordHolder).GetComponent<TextMeshProUGUI>();
                wordForm.text = _words[i];
                currentFields.Add(wordForm);
            }
        }

        private void OnEnable()
        {
            hasBeenHovered = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            mouseOnHolder = true;
            hasBeenHovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            mouseOnHolder = false;
            // StartCoroutine(ExitHandler());
        }

        private IEnumerator ExitHandler()
        {
            yield return null;
            if (!lastEnabler.pointerOnThis)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
