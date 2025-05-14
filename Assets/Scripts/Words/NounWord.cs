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
        [Range(1, 5)]
        public int declensionClass;

        public string wordCore = "katt";
        [Header ("Epämääräinen artikkeli")]
        public string wordGenderStart = "en";
        [Header ("Määräinen pääte")]
        public string wordGenderEnd = "en";
        [Header ("Epämääräisen monikon pääte")]
        public string wordPluralEnd = "er";
        [Tooltip ("Vaatiiko monikko \"flera\" sanan?")]
        public bool fleraPlural = false;
        private readonly string flera = "flera";
        [Header ("Määräisen monikon pääte")]
        public string wordKnownPluralEnd = "erna";

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
            if (fleraPlural && UIManager.instance.LightmodeOn)
            {
                return string.Concat(flera, colorTagStartLight, wordCore, colorTagEnd, wordPluralEnd);
            }
            else if (fleraPlural && !UIManager.instance.LightmodeOn)
            {
                return string.Concat(flera, colorTagStartDark, wordCore, colorTagEnd, wordPluralEnd);
            }
            else if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, wordCore, colorTagEnd, wordPluralEnd);
            }
            else
            {
                return string.Concat(colorTagStartDark, wordCore, colorTagEnd, wordPluralEnd);
            }
        }

        public string PluralKnownNoun()
        {
            if (UIManager.instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, wordCore, colorTagEnd, wordKnownPluralEnd);
            }
            else
            {
                return string.Concat(colorTagStartDark, wordCore, colorTagEnd, wordKnownPluralEnd);
            }
        }
    }
}
