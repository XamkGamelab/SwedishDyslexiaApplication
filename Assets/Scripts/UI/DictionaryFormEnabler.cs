using System;
using System.Collections;
using SwedishApp.Words;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SwedishApp.UI
{
    public class DictionaryFormEnabler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [HideInInspector] public DictionaryFormHolder wordFormHolder;
        [SerializeField] private float hoverTime;
        private string[] wordForms;
        private WaitForSeconds hoverWait;
        private Coroutine delayCoroutine;
        private Coroutine checkerCoroutine;
        public bool pointerOnThis { get; private set; } = false;
        public VerbWord verbWord;
        public NounWord nounWord;
        public AdjectiveWord adjectiveWord;

        private void Start()
        {
            hoverWait = new(hoverTime);
        }

        public void Init(VerbWord _verb)
        {
            verbWord = _verb;
        }

        public void Init(NounWord _noun)
        {
            nounWord = _noun;
        }

        public void Init(AdjectiveWord _adjective)
        {
            adjectiveWord = _adjective;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            pointerOnThis = true;
            checkerCoroutine ??= StartCoroutine(MousePosChecker());
            delayCoroutine ??= StartCoroutine(ShowDelay());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            pointerOnThis = false;
        }

        private IEnumerator ShowDelay()
        {
            yield return hoverWait;
            if (!wordFormHolder.mouseOnHolder && !pointerOnThis)
            {
                delayCoroutine = null;
                yield break;
            }

            wordFormHolder.gameObject.SetActive(true);

            if (verbWord != null) wordFormHolder.InitHolder(verbWord, this);
            else if (nounWord != null) wordFormHolder.InitHolder(nounWord, this);
            else if (adjectiveWord != null) wordFormHolder.InitHolder(adjectiveWord, this);
            wordFormHolder.transform.position = transform.position;
            delayCoroutine = null;
        }

        private IEnumerator MousePosChecker()
        {
            while (true)
            {
                if (!wordFormHolder.mouseOnHolder && !pointerOnThis)
                {
                    wordFormHolder.gameObject.SetActive(false);
                    break;
                }
                yield return null;
            }

            checkerCoroutine = null;
        }
    }
}
