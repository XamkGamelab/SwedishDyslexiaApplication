using System.Collections.Generic;
using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class simply houses a list of adverb-related words to be used in minigames.
    /// </summary>
    [CreateAssetMenu(menuName = "AdverbList")]
    public class AdverbList : ScriptableObject
    {
        public List<GrammarWord> adverbList;
    }
}