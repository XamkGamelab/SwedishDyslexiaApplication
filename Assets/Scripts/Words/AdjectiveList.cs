using System.Collections.Generic;
using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class simply houses a list of adjectives to be used in minigames.
    /// </summary>
    [CreateAssetMenu(menuName = "AdjectiveList")]
    public class AdjectiveList : ScriptableObject
    {
        public List<AdjectiveWord> adjectiveList;
    }
}
