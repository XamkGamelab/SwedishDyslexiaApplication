using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class houses the additional editor input field for notices
    /// </summary>
    [System.Serializable]
    public class PhraseWord : Word
    {        
        [Header("Notices")]
        [Tooltip("E.g. tycka IV")]
        public string notices;
    }
}
