using System.Collections.Generic;
using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class simply houses a list of question-related words to be used in minigames.
    /// </summary>
    [CreateAssetMenu(menuName = "QuestionList")]
    public class QuestionList : ScriptableObject
    {
        public List<GrammarWord> questionList;
    }
}