using System.Collections.Generic;
using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class simply houses a list of numbers to be used in minigames.
    /// </summary>
    [CreateAssetMenu(menuName = "NumberList")]
    public class NumberList : ScriptableObject
    {
        public List<NumberWord> numberList;
    }
}