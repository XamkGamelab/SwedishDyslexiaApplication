using TMPro;
using UnityEngine;

namespace SwedishApp.UI
{
    public class DictionaryEntry : MonoBehaviour
    {
        public enum WordType
        {
            verb = 0,
            noun,
            adjective,
            number,
            time,
            grammar,
            pronoun,
            phrase
        }
        public TextMeshProUGUI FinnishWordTxt;
        public TextMeshProUGUI SwedishWordTxt;
        public WordType wordType;
    }
}
