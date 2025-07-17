using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class houses the additional editor input field for notices
    /// </summary>
    [System.Serializable]
    public class GrammarWord : Word
    {
        [Header("Notices")]
        [Tooltip("E.g. innan if the sentence before is positive, förrän if negative")]
        public string notices;
    }
}