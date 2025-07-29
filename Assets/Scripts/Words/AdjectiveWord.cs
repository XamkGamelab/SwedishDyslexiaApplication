using SwedishApp.UI;
using TMPro;
using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class handles constructing adjectives while highlighting the "core" of the word.
    /// </summary>
    [System.Serializable]
    public class AdjectiveWord : Word
    {
        [Header("Perusmuoto")]
        public string wordCore = "varm";
        public bool genderedHyphenatesIrregularly = false;
        public bool comparativeHyphenatesIrregularly = false;
        public bool superlativeHyphenatesIrregularly = false;
        public bool wordUsesMerMest = false;
        public string wordCoreWithIrregularHyphenation = "";
            [Tooltip("(e.g. 'n') Leave blank if just adding 't' to the core word makes the word's gender into ett")]
        public string wordEnEnd = "";
            [Tooltip("(e.g. 't')")]
        public string wordEttEnd = "t";
            [Tooltip("Check this box if the word is regular")]
        public bool definitiveIsRegular = true;
            [Tooltip("(e.g. 'a') If this form is irregular, set this variable to be the whole word")]
        public string wordDefinitiveEnd = "a";
            [Tooltip("Check this box if the word is regular")]
        public bool definitivePluralIsRegular = true;
            [Tooltip("If a 4th form exists, set this variable to that word")]
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
        public readonly string mer = "mer ";

        [Header("Superlatiivi")]
            [Tooltip("Check this box if the word is regular")]
        public bool superlativeIsRegular = true;
            [Tooltip("(e.g. 'ast') If this form is irregular, set this variable to be the whole word")]
        public string wordSuperlativeEnd = "ast";
            [Tooltip("Check this box if the word is regular")]
        public bool superlativeDefinitiveIsRegular = true;
            [Tooltip("(e.g. 'aste') If this form is irregular, set this variable to be the whole word")]
        public string wordDefinitiveSuperlativeEnd = "aste";
        public readonly string mest = "mest ";

        public string HighlightedSwedishWord()
        {
            if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, swedishWord, colorTagEnd);
            }
            else
            {
                return string.Concat(colorTagStartDark, swedishWord, colorTagEnd);
            }
        }

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// e.g. "varm" or "stor"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveEn()
        {
            string _actualCore = genderedHyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;

            if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, _actualCore, colorTagEnd, wordEnEnd);
            }
            else
            {
                return string.Concat(colorTagStartDark, _actualCore, colorTagEnd, wordEnEnd);
            }
        }

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// e.g. "varmt" or "stort"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveEtt()
        {
            string _actualCore = genderedHyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;

            if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, _actualCore, colorTagEnd, wordEttEnd);
            }
            else
            {
                return string.Concat(colorTagStartDark, _actualCore, colorTagEnd, wordEttEnd);
            }
        }

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// e.g. "den varma" or "den stora"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveDefinitiveEn()
        {
            string _actualCore = genderedHyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;

            if (UIManager.instance.LightmodeOn)
            {
                if (definitiveIsRegular)
                {
                    return string.Concat(wordDefinitiveStartEn, colorTagStartLight, _actualCore, colorTagEnd, wordDefinitiveEnd);
                }
                else
                {
                    return string.Concat(wordDefinitiveStartEn, colorTagStartLight, wordDefinitiveEnd, colorTagEnd);
                }
            }
            else
            {
                if (definitiveIsRegular)
                {
                    return string.Concat(wordDefinitiveStartEn, colorTagStartDark, _actualCore, colorTagEnd, wordDefinitiveEnd);
                }
                else
                {
                    return string.Concat(wordDefinitiveStartEn, colorTagStartDark, wordDefinitiveEnd, colorTagEnd);
                }
            }
        }

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// e.g. "det varma" or "det stora"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveDefinitiveEtt()
        {
            string _actualCore = genderedHyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;

            if (UIManager.instance.LightmodeOn)
            {
                if (definitiveIsRegular)
                {
                    return string.Concat(wordDefinitiveStartEtt, colorTagStartLight, _actualCore, colorTagEnd, wordDefinitiveEnd);
                }
                else
                {
                    return string.Concat(wordDefinitiveStartEtt, colorTagStartLight, wordDefinitiveEnd, colorTagEnd);
                }
            }
            else
            {
                if (definitiveIsRegular)
                {
                    return string.Concat(wordDefinitiveStartEtt, colorTagStartDark, _actualCore, colorTagEnd, wordDefinitiveEnd);
                }
                else
                {
                    return string.Concat(wordDefinitiveStartEtt, colorTagStartDark, wordDefinitiveEnd, colorTagEnd);
                }
            }
        }

        /// <summary>
        /// This outputs the resulting adjective with its core highlighted.
        /// e.g. "de varma" or "de stora"
        /// </summary>
        /// <returns>Return described above.</returns>
        public string AdjectiveDefinitivePlural()
        {
            string _actualCore = genderedHyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;

            if (UIManager.instance.LightmodeOn)
            {
                if (definitivePluralIsRegular)
                {   // Regular
                    return string.Concat(wordDefinitiveStartPlural, colorTagStartLight, _actualCore, colorTagEnd, wordDefinitiveEnd);
                }
                else
                {   // Irregular
                    return string.Concat(wordDefinitiveStartPlural, colorTagStartLight, wordDefinitivePluralIrregular, colorTagEnd);
                }
            }
            else
            {
                if (definitivePluralIsRegular)
                {   // Regular
                    return string.Concat(wordDefinitiveStartPlural, colorTagStartDark, _actualCore, colorTagEnd, wordDefinitiveEnd);
                }
                else
                {   // Irregular
                    return string.Concat(wordDefinitiveStartPlural, colorTagStartDark, wordDefinitivePluralIrregular, colorTagEnd);

                }
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
            string _actualCore = comparativeHyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;

            if (UIManager.instance.LightmodeOn)
            {
                if (wordUsesMerMest)
                {   // Regular
                    return string.Concat(mer, colorTagStartLight, _actualCore, colorTagEnd);
                }
                else if (comparativeIsRegular)
                {   // Regular
                    return string.Concat(colorTagStartLight, _actualCore, colorTagEnd, wordComparativeEnd);
                }
                else
                {   // Irregular
                    return string.Concat(colorTagStartLight, wordComparativeEnd, colorTagEnd);
                }
            }
            else    // Dark mode
            {
                if (wordUsesMerMest)
                {   // Regular
                    return string.Concat(mer, colorTagStartDark, _actualCore, colorTagEnd);
                }
                else if (comparativeIsRegular)
                {   // Regular
                    return string.Concat(colorTagStartDark, _actualCore, colorTagEnd, wordComparativeEnd);
                }
                else
                {   // Irregular
                    return string.Concat(colorTagStartDark, wordComparativeEnd, colorTagEnd);
                }
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
            string _actualCore = comparativeHyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;

            if (UIManager.instance.LightmodeOn)
            {
                if (comparativeDefinitiveIsRegular)
                {   // Regular
                    return string.Concat(wordDefinitiveStartEn, colorTagStartLight, _actualCore, colorTagEnd, wordComparativeEnd);
                }
                else
                {   // Irregular
                    return string.Concat(wordDefinitiveStartEn, colorTagStartLight, wordComparativeEnd, colorTagEnd);
                }
            }
            else    // Dark mode
            {
                if (comparativeDefinitiveIsRegular)
                {   // Regular
                    return string.Concat(wordDefinitiveStartEn, colorTagStartDark, _actualCore, colorTagEnd, wordComparativeEnd);
                }
                else
                {   // Irregular
                    return string.Concat(wordDefinitiveStartEn, colorTagStartDark, wordComparativeEnd, colorTagEnd);
                }
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
            string _actualCore = comparativeHyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;

            if (UIManager.instance.LightmodeOn)
            {
                if (comparativeDefinitiveIsRegular)
                {   // Regular
                    return string.Concat(wordDefinitiveStartEtt, colorTagStartLight, _actualCore, colorTagEnd, wordComparativeEnd);
                }
                else
                {   // Irregular
                    return string.Concat(wordDefinitiveStartEtt, colorTagStartLight, wordComparativeEnd, colorTagEnd);
                }
            }
            else    // Dark mode
            {
                if (comparativeDefinitiveIsRegular)
                {   // Regular
                    return string.Concat(wordDefinitiveStartEtt, colorTagStartDark, _actualCore, colorTagEnd, wordComparativeEnd);
                }
                else
                {   // Irregular
                    return string.Concat(wordDefinitiveStartEtt, colorTagStartDark, wordComparativeEnd, colorTagEnd);
                }
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
            string _actualCore = comparativeHyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;

            if (UIManager.instance.LightmodeOn)
            {
                if (comparativeDefinitiveIsRegular)
                {   // Regular
                    return string.Concat(wordDefinitiveStartPlural, colorTagStartLight, _actualCore, colorTagEnd, wordComparativeEnd);
                }
                else
                {   // Irregular
                    return string.Concat(wordDefinitiveStartPlural, colorTagStartLight, wordComparativeEnd, colorTagEnd);
                }
            }
            else    // Dark mode
            {
                if (comparativeDefinitiveIsRegular)
                {   // Regular
                    return string.Concat(wordDefinitiveStartPlural, colorTagStartDark, _actualCore, colorTagEnd, wordComparativeEnd);
                }
                else
                {   // Irregular
                    return string.Concat(wordDefinitiveStartPlural, colorTagStartDark, wordComparativeEnd, colorTagEnd);
                }
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
            string _actualCore = superlativeHyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;

            if (UIManager.instance.LightmodeOn)
            {
                if (wordUsesMerMest)
                {   // Regular
                    return string.Concat(mest, colorTagStartLight, _actualCore, colorTagEnd);
                }
                else if (superlativeIsRegular)
                {   // Regular
                    return string.Concat(colorTagStartLight, _actualCore, colorTagEnd, wordSuperlativeEnd);
                }
                else
                {   // Irregular
                    return string.Concat(colorTagStartLight, wordSuperlativeEnd, colorTagEnd);
                }
            }
            else    // Dark mode
            {
                if (wordUsesMerMest)
                {   // Regular
                    return string.Concat(mest, colorTagStartDark, _actualCore, colorTagEnd);
                }
                else if (superlativeIsRegular)
                {   // Regular
                    return string.Concat(colorTagStartDark, _actualCore, colorTagEnd, wordSuperlativeEnd);
                }
                else
                {   // Irregular
                    return string.Concat(colorTagStartDark, wordSuperlativeEnd, colorTagEnd);
                }
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
            string _actualCore = superlativeHyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;

            if (UIManager.instance.LightmodeOn)
            {
                if (superlativeDefinitiveIsRegular)
                {   // Regular
                    return string.Concat(wordDefinitiveStartEn, colorTagStartLight, _actualCore, colorTagEnd, wordDefinitiveSuperlativeEnd);
                }
                else
                {   // Irregular
                    return string.Concat(wordDefinitiveStartEn, colorTagStartLight, wordDefinitiveSuperlativeEnd, colorTagEnd);
                }
            }
            else    // Dark mode
            {
                if (superlativeDefinitiveIsRegular)
                {   // Regular
                    return string.Concat(wordDefinitiveStartEn, colorTagStartDark, _actualCore, colorTagEnd, wordDefinitiveSuperlativeEnd);
                }
                else
                {   // Irregular
                    return string.Concat(wordDefinitiveStartEn, colorTagStartDark, wordDefinitiveSuperlativeEnd, colorTagEnd);
                }
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
            string _actualCore = superlativeHyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;

            if (UIManager.instance.LightmodeOn)
            {
                if (superlativeDefinitiveIsRegular)
                {   // Regular
                    return string.Concat(wordDefinitiveStartEtt, colorTagStartLight, _actualCore, colorTagEnd, wordDefinitiveSuperlativeEnd);
                }
                else
                {   // Irregular
                    return string.Concat(wordDefinitiveStartEtt, colorTagStartLight, wordDefinitiveSuperlativeEnd, colorTagEnd);
                }
            }
            else    // Dark mode
            {
                if (superlativeDefinitiveIsRegular)
                {   // Regular
                    return string.Concat(wordDefinitiveStartEtt, colorTagStartDark, _actualCore, colorTagEnd, wordDefinitiveSuperlativeEnd);
                }
                else
                {   // Irregular
                    return string.Concat(wordDefinitiveStartEtt, colorTagStartDark, wordDefinitiveSuperlativeEnd, colorTagEnd);
                }
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
            string _actualCore = superlativeHyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;

            if (UIManager.instance.LightmodeOn)
            {
                if (definitivePluralIsRegular)
                {   // Regular
                    return string.Concat(wordDefinitiveStartPlural, colorTagStartLight, _actualCore, colorTagEnd, wordDefinitiveSuperlativeEnd);
                }
                else
                {   // Irregular
                    return string.Concat(wordDefinitiveStartPlural, colorTagStartLight, wordDefinitiveSuperlativeEnd, colorTagEnd);
                }
            }
            else    // Dark mode
            {
                if (definitivePluralIsRegular)
                {   // Regular
                    return string.Concat(wordDefinitiveStartPlural, colorTagStartDark, _actualCore, colorTagEnd, wordDefinitiveSuperlativeEnd);
                }
                else
                {   // Irregular
                    return string.Concat(wordDefinitiveStartPlural, colorTagStartDark, wordDefinitiveSuperlativeEnd, colorTagEnd);
                }
            }
        }
    }
}
// This code's function names
/*
AdjectiveEn
AdjectiveEtt
AdjectiveDefinitiveEn
AdjectiveDefinitiveEtt
AdjectiveDefinitivePlural
AdjectiveComparative
AdjectiveComparativeDefinitiveEn
AdjectiveComparativeDefinitiveEtt
AdjectiveComparativeDefinitivePlural
AdjectiveSuperlative
AdjectiveSuperlativeDefinitiveEn
AdjectiveSuperlativeDefinitiveEtt
AdjectiveSuperlativeDefinitivePlural
*/