using SwedishApp.UI;
using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class handles constructing adjectives while highlighting the "core" of the word.
    /// </summary>
    [System.Serializable]
    public class AdjectiveWord : Word
    {
        // I added an indentation in front of the tooltips for easier legibility

        [Header("Perusmuoto")]
        public string wordCore = "varm";
            [Tooltip ("(e.g. 'a')")]
        public string wordDefinitiveEnd = "a";
            [Tooltip("(e.g. 't')")]
        public string wordEttEnd = "t";
        public bool wordDefinitivePluralIsRegular = true;
        public string wordDefinitivePluralIrregular = "";

        [Header ("Määrällinen muoto")]
        public readonly string wordDefinitiveStartEn = "den ";
        public readonly string wordDefinitiveStartEtt = "det ";
        public readonly string wordDefinitiveStartPlural = "de ";

        [Header("Komparatiivi")]
            [Tooltip("Check this box if the word is regular")]
        public bool comparativeIsRegular = true;
            [Tooltip("If this form is irregular, set this variable to be the whole word")]
        public string wordComparativeEnd = "are";
        [Tooltip("Check this box if the word is regular")]
        public bool comparativeDefinitiveIsRegular = true;

        [Header("Superlatiivi")]
            [Tooltip("Check this box if the word is regular")]
        public bool superlativeIsRegular = true;
            [Tooltip("(e.g. 'ast') If this form is irregular, set this variable to be the whole word")]
        public string wordSuperlativeEnd = "ast";
            [Tooltip("Check this box if the word is regular")]
        public bool superlativeDefinitiveIsRegular = true;
            [Tooltip("(e.g. 'aste') If this form is irregular, set this variable to be the whole word")]
        public string wordDefinitiveSuperlativeEnd = "aste";

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// e.g. "varm" or "stor"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveEn()
        {
            if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, wordCore, colorTagEnd);
            }
            else
            {
                return string.Concat(colorTagStartDark, wordCore, colorTagEnd);
            }
        }

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// e.g. "varmt" or "stort"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveEtt()
        {
            if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, wordCore, colorTagEnd, wordEttEnd);
            }
            else
            {
                return string.Concat(colorTagStartDark, wordCore, colorTagEnd, wordEttEnd);
            }
        }

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// e.g. "den varma" or "den stora"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveDefinitiveEn()
        {
            if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartEn, colorTagStartLight, wordCore, colorTagEnd, wordDefinitiveEnd);
            }
            else
            {
                return string.Concat(wordDefinitiveStartEn, colorTagStartDark, wordCore, colorTagEnd, wordDefinitiveEnd);
            }
        }

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// e.g. "det varma" or "det stora"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveDefinitiveEtt()
        {
            if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartEtt, colorTagStartLight, wordCore, colorTagEnd, wordDefinitiveEnd);
            }
            else
            {
                return string.Concat(wordDefinitiveStartEtt, colorTagStartDark, wordCore, colorTagEnd, wordDefinitiveEnd);
            }
        }

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// e.g. "de varma" or "de stora"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveDefinitivePlural()
        {
            if (UIManager.instance.LightmodeOn && wordDefinitivePluralIsRegular)
            {
                return string.Concat(wordDefinitiveStartPlural, colorTagStartLight, wordCore, colorTagEnd, wordDefinitiveEnd);
            }
            else if (!UIManager.instance.LightmodeOn && wordDefinitivePluralIsRegular)
            {
                return string.Concat(wordDefinitiveStartPlural, colorTagStartDark, wordCore, colorTagEnd, wordDefinitiveEnd);
            }

            else if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartPlural, colorTagStartLight, wordDefinitivePluralIrregular, colorTagEnd);
            }
            else 
            {
                return string.Concat(wordDefinitiveStartPlural, colorTagStartDark, wordDefinitivePluralIrregular, colorTagEnd);
            }
        }

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// If the adjective is irregular, highlight the entire word. 
        /// e.g. "varmare" or "större"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveComparative()
        {
            // Regular
            if (comparativeIsRegular && UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, wordCore, colorTagEnd, wordComparativeEnd);
            }
            else if (comparativeIsRegular && !UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartDark, wordCore, colorTagEnd, wordComparativeEnd);
            }

            // Irregular
            else if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, wordComparativeEnd, colorTagEnd);
            }
            else
            {
                return string.Concat(colorTagStartDark, wordComparativeEnd, colorTagEnd);
            }
        }

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// If the adjective is irregular, highlight the entire word. 
        /// e.g. "den varmare" or "den större"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveComparativeDefinitiveEn()
        {
            // Regular
            if (comparativeDefinitiveIsRegular && UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartEn, colorTagStartLight, wordCore, colorTagEnd, wordComparativeEnd);
            }
            else if (comparativeDefinitiveIsRegular && !UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartEn, colorTagStartDark, wordCore, colorTagEnd, wordComparativeEnd);
            }

            // Irregular
            else if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartEn, colorTagStartLight, wordComparativeEnd, colorTagEnd);
            }
            else
            {
                return string.Concat(wordDefinitiveStartEn, colorTagStartDark, wordComparativeEnd, colorTagEnd);
            }
        }

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// If the adjective is irregular, highlight the entire word. 
        /// e.g. "det varmare" or "det större"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveComparativeDefinitiveEtt()
        {
            // Regular
            if (comparativeDefinitiveIsRegular && UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartEtt, colorTagStartLight, wordCore, colorTagEnd, wordComparativeEnd);
            }
            else if (comparativeDefinitiveIsRegular && !UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartEtt, colorTagStartDark, wordCore, colorTagEnd, wordComparativeEnd);
            }

            // Irregular
            else if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartEtt, colorTagStartLight, wordComparativeEnd, colorTagEnd);
            }
            else
            {
                return string.Concat(wordDefinitiveStartEtt, colorTagStartDark, wordComparativeEnd, colorTagEnd);
            }
        }

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// If the adjective is irregular, highlight the entire word. 
        /// e.g. "de varmare" or "de större"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveComparativeDefinitivePlural()
        {
            // Regular
            if (comparativeDefinitiveIsRegular && UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartPlural, colorTagStartLight, wordCore, colorTagEnd, wordComparativeEnd);
            }
            else if (comparativeDefinitiveIsRegular && !UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartPlural, colorTagStartDark, wordCore, colorTagEnd, wordComparativeEnd);
            }

            // Irregular
            else if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartPlural, colorTagStartLight, wordComparativeEnd, colorTagEnd);
            }
            else
            {
                return string.Concat(wordDefinitiveStartPlural, colorTagStartDark, wordComparativeEnd, colorTagEnd);
            }
        }

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// If the adjective is irregular, highlight the entire word. 
        /// e.g. "varmast" or "störst"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveSuperlative()
        {
            // Regular
            if (superlativeIsRegular && UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, wordCore, colorTagEnd, wordSuperlativeEnd);
            }
            else if (comparativeIsRegular && !UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartDark, wordCore, colorTagEnd, wordSuperlativeEnd);
            }

            // Irregular
            else if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, wordSuperlativeEnd, colorTagEnd);
            }
            else
            {
                return string.Concat(colorTagStartDark, wordSuperlativeEnd, colorTagEnd);
            }
        }

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// If the adjective is irregular, highlight the entire word. 
        /// e.g. "den varmaste" or "den största"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveSuperlativeDefinitiveEn()
        {
            // Regular
            if (superlativeDefinitiveIsRegular && UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartEn, colorTagStartLight, wordCore, colorTagEnd, wordDefinitiveSuperlativeEnd);
            }
            else if (superlativeDefinitiveIsRegular && !UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartEn, colorTagStartDark, wordCore, colorTagEnd, wordDefinitiveSuperlativeEnd);
            }

            // Irregular
            else if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartEn, colorTagStartLight, wordDefinitiveSuperlativeEnd, colorTagEnd);
            }
            else
            {
                return string.Concat(wordDefinitiveStartEn, colorTagStartDark, wordDefinitiveSuperlativeEnd, colorTagEnd);
            }
        }

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// If the adjective is irregular, highlight the entire word. 
        /// e.g. "det varmaste" or "det största"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveSuperlativeDefinitiveEtt()
        {
            // Regular
            if (superlativeDefinitiveIsRegular && UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartEtt, colorTagStartLight, wordCore, colorTagEnd, wordDefinitiveSuperlativeEnd);
            }
            else if (superlativeDefinitiveIsRegular && !UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartEtt, colorTagStartDark, wordCore, colorTagEnd, wordDefinitiveSuperlativeEnd);
            }

            // Irregular
            else if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartEtt, colorTagStartLight, wordDefinitiveSuperlativeEnd, colorTagEnd);
            }
            else
            {
                return string.Concat(wordDefinitiveStartEtt, colorTagStartDark, wordDefinitiveSuperlativeEnd, colorTagEnd);
            }
        }

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// If the adjective is irregular, highlight the entire word. 
        /// e.g. "de varmaste" or "de största"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveSuperlativeDefinitivePlural()
        {
            // Regular
            if (superlativeDefinitiveIsRegular && UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartPlural, colorTagStartLight, wordCore, colorTagEnd, wordDefinitiveSuperlativeEnd);
            }
            else if (superlativeDefinitiveIsRegular && !UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartPlural, colorTagStartDark, wordCore, colorTagEnd, wordDefinitiveSuperlativeEnd);
            }

            // Irregular
            else if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordDefinitiveStartPlural, colorTagStartLight, wordDefinitiveSuperlativeEnd, colorTagEnd);
            }
            else
            {
                return string.Concat(wordDefinitiveStartPlural, colorTagStartDark, wordDefinitiveSuperlativeEnd, colorTagEnd);
            }
        }
    }
}