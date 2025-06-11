using System.Collections.Generic;
using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class simply houses a list of pronouns to be used in minigames.
    /// </summary>
    [CreateAssetMenu(menuName = "PronounsList")]
    public class PronounsList : ScriptableObject
    {
        public List<PronounsWord> pronounsList;
    }
}
