using TMPro;
using UnityEngine;

namespace SwedishApp.Words
{
    public class HighlightTest : MonoBehaviour
    {
        [SerializeField] private TMP_InputField testField;
        private string wordCore = "Hello";
        private string wordEnd = "World";
        private string colorTagStart = "<color=#EFA00B>";
        private string colorTagEnd = "</color>";

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            testField = GetComponent<TMP_InputField>();
            testField.text = string.Concat(colorTagStart, wordCore, colorTagEnd, " " , wordEnd);
        }
    }
}