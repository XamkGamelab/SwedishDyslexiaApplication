using System.Collections.Generic;
using UnityEngine;

namespace SwedishApp.Words
{
    [CreateAssetMenu (menuName = "VerbList")]
    public class VerbList : ScriptableObject
    {
        public List<VerbWord> verbList;
    }
}
