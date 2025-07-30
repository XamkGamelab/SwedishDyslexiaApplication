using System.Collections.Generic;
using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class simply houses a list of preposition-related words to be used in minigames.
    /// </summary>
    [CreateAssetMenu(menuName = "PrepositionList")]
    public class PrepositionList : ScriptableObject
    {
        public List<GrammarWord> prepositionList;
    }
}