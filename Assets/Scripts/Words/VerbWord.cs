using SwedishApp.UI;
using UnityEngine;
using UnityEditor;

namespace SwedishApp.Words
{
    [System.Serializable]
    public class VerbWord : Word
    {
        public string wordCore = "var";
        [Header ("Perusmuoto")]
        public bool baseformIsRegular = true;
        [Tooltip ("If this form is irregular, set this variable to be the whole word")]
        public string baseformEnd = "a";
        [Header ("Preesens")]
        public bool currentTenseIsRegular = false;
        [Tooltip ("If this form is irregular, set this variable to be the whole word")]
        public string currentTenseEnd = "är";
        [Header ("Imperfekti")]
        public bool pastTenseIsRegular = true;
        [Tooltip ("If this form is irregular, set this variable to be the whole word")]
        public string pastTenseEnd = "";
        [Header ("Perfekti")]
        public bool pastPerfectTenseIsRegular = true;
        [Tooltip ("Please remember to put a space at the end of this string!")]
        public string pastPerfectTenseStart = "har ";
        [Tooltip ("If this form is irregular, set this variable to be the whole word")]
        public string pastPerfectTenseEnd = "it";
        [Header ("Pluskvamperfekti")]
        public bool pastPlusPerfectTenseIsRegular = true;
        [Tooltip ("Please remember to put a space at the end of this string!")]
        public string pastPlusPerfectTenseStart = "hade ";
        [Tooltip ("If this form is irregular, set this variable to be the whole word")]
        public string pastPlusPerfectTenseEnd = "it";

        private readonly string colorTagStartDark = "<color=#EFA00B>";
        private readonly string colorTagStartLight = "<color=#016FB9>";
        private readonly string colorTagEnd = "</color>";

        public string BaseformWord()
        {
            if (baseformIsRegular && UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, wordCore, colorTagEnd, baseformEnd);
            }
            else if (baseformIsRegular && !UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartDark, wordCore, colorTagEnd, baseformEnd);
            }
            else if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, baseformEnd, colorTagEnd);
            }
            else
            {
                return string.Concat(colorTagStartDark, baseformEnd, colorTagEnd);
            }
        }

        public string CurrentTenseWord()
        {
            if (currentTenseIsRegular && UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, wordCore, colorTagEnd, currentTenseEnd);
            }
            else if (currentTenseIsRegular && !UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartDark, wordCore, colorTagEnd, currentTenseEnd);
            }
            else if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, currentTenseEnd, colorTagEnd);
            }
            else
            {
                return string.Concat(colorTagStartDark, currentTenseEnd, colorTagEnd);
            }
        }

        public string PastTenseWord()
        {
            if (pastTenseIsRegular && UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, wordCore, colorTagEnd, pastTenseEnd);
            }
            else if (pastTenseIsRegular && !UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartDark, wordCore, colorTagEnd, pastTenseEnd);
            }
            else if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, pastTenseEnd, colorTagEnd);
            }
            else
            {
                return string.Concat(colorTagStartDark, pastTenseEnd, colorTagEnd);
            }
        }

        public string PastPerfectTenseWord()
        {
            if (pastPerfectTenseIsRegular && UIManager.instance.LightmodeOn)
            {
                return string.Concat(pastPerfectTenseStart, colorTagStartLight, wordCore, colorTagEnd, pastPerfectTenseEnd);
            }
            else if (pastPerfectTenseIsRegular && !UIManager.instance.LightmodeOn)
            {
                return string.Concat(pastPerfectTenseStart, colorTagStartDark, wordCore, colorTagEnd, pastPerfectTenseEnd);
            }
            else if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(pastPerfectTenseStart, colorTagStartLight, pastPerfectTenseEnd, colorTagEnd);
            }
            else
            {
                return string.Concat(pastPerfectTenseStart, colorTagStartDark, pastPerfectTenseEnd, colorTagEnd);
            }
        }

        public string PastPlusPerfectTenseWord()
        {
            if (pastPlusPerfectTenseIsRegular && UIManager.instance.LightmodeOn)
            {
                return string.Concat(pastPlusPerfectTenseStart, colorTagStartLight, wordCore, colorTagEnd, pastPlusPerfectTenseEnd);
            }
            else if (pastPlusPerfectTenseIsRegular && !UIManager.instance.LightmodeOn)
            {
                return string.Concat(pastPlusPerfectTenseStart, colorTagStartDark, wordCore, colorTagEnd, pastPlusPerfectTenseEnd);
            }
            else if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(pastPlusPerfectTenseStart, colorTagStartLight, pastPlusPerfectTenseIsRegular, colorTagEnd);
            }
            else
            {
                return string.Concat(pastPlusPerfectTenseStart, colorTagStartDark, pastPlusPerfectTenseIsRegular, colorTagEnd);
            }
        }
    }
}