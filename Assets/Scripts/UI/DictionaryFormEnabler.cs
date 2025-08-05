using System.Collections;
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

        private void Start()
        {
            hoverWait = new(hoverTime);
        }

        public void Init(string[] _wordForms)
        {
            wordForms = _wordForms;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            pointerOnThis = true;
            checkerCoroutine ??= StartCoroutine(MousePosChecker());
            // if (!wordFormHolder.mouseOnHolder)
            // {
                delayCoroutine ??= StartCoroutine(ShowDelay());
            // }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            pointerOnThis = false;
            // Debug.Log(wordFormHolder.hasBeenHovered);
            // if ((wordFormHolder.mouseOnHolder && wordFormHolder.hasBeenHovered) || (!wordFormHolder.mouseOnHolder && wordFormHolder.hasBeenHovered))
            // {
            //     delayCoroutine = null;
            //     wordFormHolder.gameObject.SetActive(false);
            // }
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
            wordFormHolder.InitWords(wordForms, this);
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
