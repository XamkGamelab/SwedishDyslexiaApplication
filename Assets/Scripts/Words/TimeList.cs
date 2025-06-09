using System.Collections.Generic;
using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class simply houses a list of time-related words to be used in minigames.
    /// </summary>
    [CreateAssetMenu(menuName = "TimeList")]
    public class TimeList : ScriptableObject
    {
        public List<TimeWord> timeList;
    }
}