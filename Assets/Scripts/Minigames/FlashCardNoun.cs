using TMPro;

namespace SwedishApp.Minigames
{
    /// <summary>
    /// Inheriting from <see cref="FlashCardBase"/>, this class houses
    /// additional text fields related to the word type.
    /// </summary>
    public class FlashCardNoun : FlashCardBase
    {
        public TextMeshProUGUI wordSwedishDefinitiveText;
        public TextMeshProUGUI wordSwedishPluralText;
        public TextMeshProUGUI wordSwedishDefinitivePluralText;
        public TextMeshProUGUI wordDeclensionClassText;
    }
}
