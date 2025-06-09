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
        [Header("Epämääräinen artikkeli")]
        public string wordGenderStart = "en ";
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
                if (wordPluralIsRegular)
                {
                    if (fleraPlural)
                    {
                        return string.Concat(flera, colorTagStartLight, wordCore, colorTagEnd, wordPluralEnd);
                    }
                    else
                    {
                        return string.Concat(colorTagStartLight, wordCore, colorTagEnd, wordPluralEnd);
                    }
                }
                else
                {
                    if (fleraPlural)
                    {
                        return string.Concat(flera, colorTagStartLight, wordPluralEnd, colorTagEnd);
                    }
                    else
                    {
                        return string.Concat(colorTagStartLight, wordPluralEnd, colorTagEnd);
                    }
                }
            }
            else
            {
                if (wordPluralIsRegular)
                {
                    if (fleraPlural)
                    {
                        return string.Concat(flera, colorTagStartDark, wordCore, colorTagEnd, wordPluralEnd);
                    }
                    else
                    {
                        return string.Concat(colorTagStartDark, wordCore, colorTagEnd, wordPluralEnd);
                    }
                }
                else
                {
                    if (fleraPlural)
                    {
                        return string.Concat(flera, colorTagStartDark, wordPluralEnd, colorTagEnd);
                    }
                    else
                    {
                        return string.Concat(colorTagStartDark, wordPluralEnd, colorTagEnd);
                    }
                }
            }
        }

        public string PluralDefinitiveNoun()
        {
            if (UIManager.instance.LightmodeOn)
            {
                if (wordDefinitivePluralIsRegular)
                {
                    return string.Concat(colorTagStartLight, wordCore, colorTagEnd, wordDefinitivePluralEnd);   // Light mode on
                }
                else
                {
                    return string.Concat(colorTagStartLight, wordDefinitivePluralEnd, colorTagEnd);             // Light mode on
                }
            }
            else
            {
                if (wordDefinitivePluralIsRegular)
                {
                    return string.Concat(colorTagStartDark, wordCore, colorTagEnd, wordDefinitivePluralEnd);    // Dark mode on
                }
                else
                {
                    return string.Concat(colorTagStartDark, wordDefinitivePluralEnd, colorTagEnd);              // Dark mode on
                }
            }
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