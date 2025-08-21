using SwedishApp.Minigames;
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

        public readonly string indefinitive = " (epäm.)";
        public readonly string definitive = " (määr.)";
        public string finnishPlural = "kissat";

        public string wordCore = "katt";

        public bool hyphenatesIrregularly = false;
        public string wordCoreWithIrregularHyphenation = "";

        [Header("Epämääräinen artikkeli")]
        public string wordGenderStart = "en ";
        public string wordIndefinitiveEnd = "";

        [Header("Määräinen pääte")]
        public string wordGenderEnd = "en";

        [Header("Epämääräinen monikko")]
        public bool wordPluralIsRegular = true;
        [Tooltip("If this form is irregular, set this variable to be the whole word")]
        public string wordPluralEnd = "er";
        [Tooltip("Vaatiiko monikko \"flera\" sanan?")]
        public bool fleraPlural = false;
        private readonly string flera = "flera ";

        [Header("Määräinen monikko")]
        public string wordDefinitivePluralEnd = "na";

        public string NounWithGenderStart()
        {
            if (UIManager.Instance.LightmodeOn)
            {
                return string.Concat(wordGenderStart, colorTagStartLight, wordCore, colorTagEnd, wordIndefinitiveEnd);
            }
            else
            {
                return string.Concat(wordGenderStart, colorTagStartDark, wordCore, colorTagEnd, wordIndefinitiveEnd);
            }
        }

        public string NounWithGenderEnd()
        {
            string _actualCore = hyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;

            if (UIManager.Instance.LightmodeOn)
            {
                return string.Concat(colorTagStartLight, _actualCore, colorTagEnd, wordGenderEnd);
            }
            else
            {
                return string.Concat(colorTagStartDark, _actualCore, colorTagEnd, wordGenderEnd);
            }
        }

        public string PluralNoun()
        {
            string _actualCore = hyphenatesIrregularly ? wordCoreWithIrregularHyphenation : wordCore;
            string colorTagStart = UIManager.Instance.LightmodeOn ? colorTagStartLight : colorTagStartDark;

            if (wordPluralIsRegular)
            {
                if (fleraPlural)
                {
                    return string.Concat(flera, colorTagStart, _actualCore, colorTagEnd, wordPluralEnd);
                }
                else
                {
                    return string.Concat(colorTagStart, _actualCore, colorTagEnd, wordPluralEnd);
                }
            }
            else
            {
                if (fleraPlural)
                {
                    return string.Concat(flera, colorTagStart, wordPluralEnd, colorTagEnd);
                }
                else
                {
                    return string.Concat(colorTagStart, wordPluralEnd, colorTagEnd);
                }
            }
        }

        public string PluralDefinitiveNoun()
        {
            return string.Concat(PluralNoun(), wordDefinitivePluralEnd);
        }

        public string GetDeclenated(DeclensionMinigame.DeclenateInto _declenateInto)
        {
            return _declenateInto switch
            {
                DeclensionMinigame.DeclenateInto.definitiivi => NounWithGenderEnd(),
                DeclensionMinigame.DeclenateInto.monikon_indefinitiivi => PluralNoun(),
                DeclensionMinigame.DeclenateInto.monikon_definitiivi => PluralDefinitiveNoun(),
                _ => "",
            };
        }

        public string GetDeclenatedFinnish(DeclensionMinigame.DeclenateInto _declenateInto)
        {
            return _declenateInto switch
            {
                DeclensionMinigame.DeclenateInto.definitiivi => string.Concat(finnishWord, definitive),
                DeclensionMinigame.DeclenateInto.monikon_indefinitiivi => string.Concat(finnishPlural, indefinitive),
                DeclensionMinigame.DeclenateInto.monikon_definitiivi => string.Concat(finnishPlural, definitive),
                _ => "",
            };
        }
    }
}