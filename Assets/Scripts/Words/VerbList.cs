using System.Collections.Generic;
using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class simply houses a list of verbs to be used in minigames.
    /// </summary>
    [CreateAssetMenu (menuName = "VerbList")]
    public class VerbList : ScriptableObject
    {
        public List<VerbWord> verbList;
    }
}