using SwedishApp.Minigames;
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
        [Header("Notices")]
        [Tooltip("E.g. innan if the sentence before is positive, förrän if negative")]
        public string notices;
        public enum ConjugationClass
        {
            I = 1,
            II,
            III,
            IV
        }

        public ConjugationClass conjugationClass;

        public string wordCore = "var";
        public bool hyphenatesIrregularly = false;
        public string wordCoreWithIrregularHyphenation = "";

        [Header("Perusmuoto")]
        public bool baseformIsRegular = true;
        [Tooltip("If this form is irregular, set this variable to be the whole word")]
        public string baseformEnd = "a";

        [Header("Preesens")]
        public bool currentTenseIsRegular = false;
        [Tooltip("If this form is irregular, set this variable to be the whole word")]
        public string currentTenseEnd = "är";
        public string currentTenseFinnish = "on";

        [Header("Imperfekti")]
        public bool pastTenseIsRegular = true;
        [Tooltip("If this form is irregular, set this variable to be the whole word")]
        public string pastTenseEnd = "";
        public string pastTenseFinnish = "oli";

        [Header("Perfekti")]
        public bool pastPerfectTenseIsRegular = true;
        [Tooltip("Please remember to put a space at the end of this string!")]
        public string pastPerfectTenseStart = "har ";
        [Tooltip("If this form is irregular, set this variable to be the whole word")]
        public string pastPerfectTenseEnd = "it";
        public string pastPerfectTenseFinnish = "on ollut";

        [Header("Pluskvamperfekti")]
        public bool pastPlusPerfectTenseIsRegular = true;
        [Tooltip("Please remember to put a space at the end of this string!")]
        public string pastPlusPerfectTenseStart = "hade ";
        [Tooltip("If this form is irregular, set this variable to be the whole word")]
        public string pastPlusPerfectTenseEnd = "it";
        public string pastPlusPerfectTenseFinnish = "oli ollut";

        /// <summary>
        /// This outputs the word in its base form with the core highlighted.
        /// If the verb is irregular, highlight the entire word.
        /// </summary>
        /// <returns>Return described above.</returns>
        public string BaseformWord()
        {
            if (UIManager.instance.LightmodeOn)
            {
                if (baseformIsRegular)
                {
                    return string.Concat(colorTagStartLight, wordCore, colorTagEnd, baseformEnd);
                }
                else
                {
                    return string.Concat(colorTagStartLight, baseformEnd, colorTagEnd);
                }
            }
            else
            {
                if (baseformIsRegular)
                {
                    return string.Concat(colorTagStartDark, wordCore, colorTagEnd, baseformEnd);
                }
                else
                {
                    return string.Concat(colorTagStartDark, baseformEnd, colorTagEnd);
                }
            }
        }
        //if (baseformIsRegular && UIManager.instance.LightmodeOn)
        //{
        //    return string.Concat(colorTagStartLight, wordCore, colorTagEnd, baseformEnd);
        //}
        //else if (baseformIsRegular && !UIManager.instance.LightmodeOn)
        //{
        //    return string.Concat(colorTagStartDark, wordCore, colorTagEnd, baseformEnd);
        //}
        //else if (UIManager.instance.LightmodeOn)
        //{
        //    return string.Concat(colorTagStartLight, baseformEnd, colorTagEnd);
        //}
        //else
        //{
        //    return string.Concat(colorTagStartDark, baseformEnd, colorTagEnd);
        //}


        /// <summary>
        /// This outputs the word in its current tense with the core highlighted.
        /// If the verb is irregular, highlight the entire word.
        /// </summary>
        /// <returns>Return described above.</returns>
        public string CurrentTenseWord()
        {
            if (UIManager.instance.LightmodeOn)
            {
                string _actualCore = hyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;

                if (currentTenseIsRegular)
                {
                    return string.Concat(colorTagStartLight, _actualCore, colorTagEnd, currentTenseEnd);
                }
                else
                {
                    return string.Concat(colorTagStartLight, currentTenseEnd, colorTagEnd);
                }
            }
            else
            {
                string _actualCore = hyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;

                if (currentTenseIsRegular)
                {
                    return string.Concat(colorTagStartDark, _actualCore, colorTagEnd, currentTenseEnd);
                }
                else
                {
                    return string.Concat(colorTagStartDark, currentTenseEnd, colorTagEnd);
                }
            }

        }

        /// <summary>
        /// This outputs the word in its past tense with the core highlighted.
        /// If the verb is irregular, highlight the entire word.
        /// </summary>
        /// <returns>Return described above.</returns>
        public string PastTenseWord()
        {
            if (UIManager.instance.LightmodeOn)
            {
                string _actualCore = hyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;
                
                if (pastTenseIsRegular)
                {
                    return string.Concat(colorTagStartLight, _actualCore, colorTagEnd, pastTenseEnd);
                }
                else
                {
                    return string.Concat(colorTagStartLight, pastTenseEnd, colorTagEnd);
                }
            }
            else
            {
                string _actualCore = hyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;
                if (pastTenseIsRegular)
                {
                    return string.Concat(colorTagStartDark, _actualCore, colorTagEnd, pastTenseEnd);
                }
                else
                {
                    return string.Concat(colorTagStartDark, pastTenseEnd, colorTagEnd);
                }
            }
        }

        /// <summary>
        /// This outputs the word in its past perfect tense with the core highlighted.
        /// If the verb is irregular, highlight the entire word.
        /// </summary>
        /// <returns>Return described above.</returns>
        public string PastPerfectTenseWord()
        {
            string _actualCore = hyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;
            
            if (UIManager.instance.LightmodeOn)
            {
                if (pastPerfectTenseIsRegular)
                {
                    return string.Concat(pastPerfectTenseStart, colorTagStartLight, _actualCore, colorTagEnd, pastPerfectTenseEnd);
                }
                else
                {
                    return string.Concat(pastPerfectTenseStart, colorTagStartLight, pastPerfectTenseEnd, colorTagEnd);
                }
            }
            else
            {
                if (pastPerfectTenseIsRegular)
                {
                    return string.Concat(pastPerfectTenseStart, colorTagStartDark, _actualCore, colorTagEnd, pastPerfectTenseEnd);
                }
                else
                {
                    return string.Concat(pastPerfectTenseStart, colorTagStartDark, pastPerfectTenseEnd, colorTagEnd);
                }
            }
        }

        /// <summary>
        /// This outputs the word in its pluperfect past tense with the core highlighted.
        /// If the verb is irregular, highlight the entire word.
        /// </summary>
        /// <returns>Return described above.</returns>
        public string PastPlusPerfectTenseWord()
        {
            if (UIManager.instance.LightmodeOn)
            {
                string _actualCore = hyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;

                if (pastPlusPerfectTenseIsRegular)
                {
                    return string.Concat(pastPlusPerfectTenseStart, colorTagStartLight, _actualCore, colorTagEnd, pastPlusPerfectTenseEnd);
                }
                else
                {
                    return string.Concat(pastPlusPerfectTenseStart, colorTagStartLight, pastPlusPerfectTenseIsRegular, colorTagEnd);
                }
            }
            else
            {
                string _actualCore = hyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;
                
                if (pastPlusPerfectTenseIsRegular)
                {
                    return string.Concat(pastPlusPerfectTenseStart, colorTagStartDark, _actualCore, colorTagEnd, pastPlusPerfectTenseEnd);
                }
                else
                {
                    return string.Concat(pastPlusPerfectTenseStart, colorTagStartDark, pastPlusPerfectTenseIsRegular, colorTagEnd);
                }
            }
        }

        public string GetConjugatedSwedish(ConjugationMinigame.ConjugateInto _conjugateInto)
        {
            return _conjugateInto switch
            {
                ConjugationMinigame.ConjugateInto.preesens => CurrentTenseWord(),
                ConjugationMinigame.ConjugateInto.imperfekti => PastTenseWord(),
                ConjugationMinigame.ConjugateInto.perfekti => PastPerfectTenseWord(),
                ConjugationMinigame.ConjugateInto.pluskvamperfekti => PastPlusPerfectTenseWord(),
                _ => "",
            };
        }

        public string GetConjugatedFinnish(ConjugationMinigame.ConjugateInto _conjugateInto)
        {
            return _conjugateInto switch
            {
                ConjugationMinigame.ConjugateInto.preesens => currentTenseFinnish,
                ConjugationMinigame.ConjugateInto.imperfekti => pastTenseFinnish,
                ConjugationMinigame.ConjugateInto.perfekti => pastPerfectTenseFinnish,
                ConjugationMinigame.ConjugateInto.pluskvamperfekti => pastPlusPerfectTenseFinnish,
                _ => "",
            };
        }
    }
}
// This code's function names
/*
BaseformWord
CurrentTenseWord
PastTenseWord
PastPerfectTenseWord
PastPlusPerfectTenseWord
 */