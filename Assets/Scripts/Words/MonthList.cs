using System.Collections.Generic;
using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class simply houses a list of months to be used in minigames.
    /// </summary>
    [CreateAssetMenu(menuName = "MonthList")]
    public class MonthList : ScriptableObject
    {
        public List<MonthWord> monthList;
    }
}
