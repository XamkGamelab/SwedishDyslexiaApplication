using SwedishApp.UI;
using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class handles constructing verbs while highlighting the "core" of the word
    /// </summary>
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

        /// <summary>
        /// This outputs the word in its base form with the core highlighted.
        /// If the verb is irregular, highlight the entire word.
        /// </summary>
        /// <returns>Return described above.</returns>
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

        /// <summary>
        /// This outputs the word in its current tense with the core highlighted.
        /// If the verb is irregular, highlight the entire word.
        /// </summary>
        /// <returns>Return described above.</returns>
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

        /// <summary>
        /// This outputs the word in its past tense with the core highlighted.
        /// If the verb is irregular, highlight the entire word.
        /// </summary>
        /// <returns>Return described above.</returns>
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

        /// <summary>
        /// This outputs the word in its past perfect tense with the core highlighted.
        /// If the verb is irregular, highlight the entire word.
        /// </summary>
        /// <returns>Return described above.</returns>
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

        /// <summary>
        /// This outputs the word in its pluperfect past tense with the core highlighted.
        /// If the verb is irregular, highlight the entire word.
        /// </summary>
        /// <returns>Return described above.</returns>
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