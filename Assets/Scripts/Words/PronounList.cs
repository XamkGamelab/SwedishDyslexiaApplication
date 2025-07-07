using System.Collections.Generic;
using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class simply houses a list of pronouns to be used in minigames.
    /// </summary>
    [CreateAssetMenu(menuName = "PronounList")]
    public class PronounList : ScriptableObject
    {
        public List<PronounWord> pronounList;
    }
}
