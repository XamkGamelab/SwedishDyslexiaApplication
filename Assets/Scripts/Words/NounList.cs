using System.Collections.Generic;
using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class simply houses a list of nouns to be used in minigames.
    /// </summary>
    [CreateAssetMenu (menuName = "NounList")]
    public class NounList : ScriptableObject
    {
        public List<NounWord> nounList;
    }
}
