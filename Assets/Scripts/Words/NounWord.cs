using SwedishApp.UI;

namespace SwedishApp.Words
{
    [System.Serializable]
    public class NounWord : Word
    {
        public string wordCore = "katt";
        public string wordGenderStart = "en";
        public string wordGenderEnd = "en";
        public string wordPluralEnd = "er";
        public bool fleraPlural = false;
        private readonly string flera = "flera";
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
