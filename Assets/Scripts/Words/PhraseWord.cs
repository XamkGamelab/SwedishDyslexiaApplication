using UnityEngine;

namespace SwedishApp.Words
{
    [System.Serializable]
    public class PhraseWord : Word
    {
        [Header("Notices")]
        [Tooltip("E.g. tycka IV")]
        public string notices;
    }
}
