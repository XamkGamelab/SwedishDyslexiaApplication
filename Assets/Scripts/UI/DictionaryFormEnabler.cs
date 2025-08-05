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
        public VerbWord verbWord;
        public NounWord nounWord;
        public AdjectiveWord adjectiveWord;
        private WaitForSeconds hoverWait;
        private Coroutine delayCoroutine;
        private Coroutine checkerCoroutine;
        private bool pointerOnThis = false;
        private bool wasInit = false;


        private void Start()
        {
            hoverWait = new(hoverTime);
        }

        public void Init(VerbWord _verb)
        {
            verbWord = _verb;
            nounWord = null;
            adjectiveWord = null;
            wasInit = true;
        }

        public void Init(NounWord _noun)
        {
            nounWord = _noun;
            verbWord = null;
            adjectiveWord = null;
            wasInit = true;
        }

        public void Init(AdjectiveWord _adjective)
        {
            adjectiveWord = _adjective;
            verbWord = null;
            nounWord = null;
            wasInit = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!wasInit) return;
            pointerOnThis = true;
            checkerCoroutine ??= StartCoroutine(MousePosChecker());
            delayCoroutine ??= StartCoroutine(ShowDelay());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!wasInit) return;
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
