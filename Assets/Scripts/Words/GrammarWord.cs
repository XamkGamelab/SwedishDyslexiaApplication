using UnityEngine;

namespace SwedishApp.Words
{
    [System.Serializable]
    public class GrammarWord : Word
    {
        [Header("Notices")]
        [Tooltip("E.g. innan if the sentence before is positive, f�rr�n if negative")]
        public string notices;
    }
}
