using SwedishApp.UI;
using UnityEngine;

namespace SwedishApp.Words
{
    /// <summary>
    /// This class handles constructing nouns in their various forms while highlighting the
    /// word's "core".
    /// </summary>
    [System.Serializable]
    public class NounWord : Word
    {
        public enum EnOrEtt
        {
            en = 1, 
            ett = 2
        }

        public EnOrEtt enOrEtt;

        [Range(1, 5)]
        public int declensionClass;

        public string wordCore = "katt";
        [Header ("Epämääräinen artikkeli")]
        public string wordGenderStart = "en ";
        [Header ("Määräinen pääte")]
        public string wordGenderEnd = "en";
        [Header ("Epämääräinen monikko")]
        public bool wordPluralIsRegular = true;
            [Tooltip("If this form is irregular, set this variable to be the whole word")]
        public string wordPluralEnd = "er";
            [Tooltip ("Vaatiiko monikko \"flera\" sanan?")]
        public bool fleraPlural = false;
        private readonly string flera = "flera ";
        [Header ("Määräinen monikko")]
        public bool wordDefinitivePluralIsRegular = true;
            [Tooltip("If this form is irregular, set this variable to be the whole word")]
        public string wordDefinitivePluralEnd = "erna";

        public string NounWithGenderStart()
        {
            if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(wordGenderStart, colorTagStartLight, wordCore, colorTagEnd);
            }
            else
            {
                return string.Concat(wordGenderStart, colorTagStartDark, wordCore, colorTagEnd);
            }
        }

        public string NounWithGenderEnd()
        {
            if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, wordCore, colorTagEnd, wordGenderEnd);
            }
            else
            {
                return string.Concat(colorTagStartDark, wordCore, colorTagEnd, wordGenderEnd);
            }
        }

        public string PluralNoun()
        {
            if (UIManager.instance.LightmodeOn)
            {
                // Irregular
                if (fleraPlural && !wordPluralIsRegular)
                {   // Flera
                    return string.Concat(flera, colorTagStartLight, wordPluralEnd, colorTagEnd);
                }
                else if (!fleraPlural)
                {   // Not flera
                    return string.Concat(colorTagStartLight, wordPluralEnd, colorTagEnd);
                }
                // Regular
                else if (fleraPlural)
                {   // Flera
                    return string.Concat(flera, colorTagStartLight, wordCore, colorTagEnd, wordPluralEnd);
                }
                else
                {   // Not flera
                    return string.Concat(colorTagStartLight, wordCore, colorTagEnd, wordPluralEnd);
                }
            }
            else
            {
                // Irregular
                if (fleraPlural && !wordPluralIsRegular)
                {   // Flera
                    return string.Concat(flera, colorTagStartDark, wordPluralEnd, colorTagEnd);
                }
                else if (!fleraPlural && !wordPluralIsRegular)
                {   // Not flera
                    return string.Concat(colorTagStartDark, wordPluralEnd, colorTagEnd);
                }
                // Regular
                else if (fleraPlural)
                {   // Flera
                    return string.Concat(flera, colorTagStartDark, wordCore, colorTagEnd, wordPluralEnd);
                }
                else
                {   // Not flera
                    return string.Concat(colorTagStartDark, wordCore, colorTagEnd, wordPluralEnd);
                }
            }
        }

        public string PluralDefinitiveNoun()
        {
            // Irregular
            if (!wordDefinitivePluralIsRegular && UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, wordDefinitivePluralEnd, colorTagEnd);             // Light mode on
            }
            else if (!wordDefinitivePluralIsRegular)
            {
                return string.Concat(colorTagStartDark, wordDefinitivePluralEnd, colorTagEnd);              // Dark mode on
            }

            // Regular
            else if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, wordCore, colorTagEnd, wordDefinitivePluralEnd);   // Light mode on
            }
            else
            {
                return string.Concat(colorTagStartDark, wordCore, colorTagEnd, wordDefinitivePluralEnd);    // Dark mode on
            }
        }
    }
}
// This code's function names
/*
NounWithGenderStart
NounWithGenderEnd
PluralNoun
PluralDefinitiveNoun
*/