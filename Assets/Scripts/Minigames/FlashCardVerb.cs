using TMPro;

namespace SwedishApp.Minigames
{
    /// <summary>
    /// Inheriting from <see cref="FlashCardBase"/>, this class houses
    /// additional text fields related to the word type.
    /// </summary>
    public class FlashCardVerb : FlashCardBase
    {
        public TextMeshProUGUI wordSwedishCurrentText;
        public TextMeshProUGUI wordSwedishPastText;
        public TextMeshProUGUI wordSwedishPastPerfectText;
        public TextMeshProUGUI wordSwedishPastPlusPerfectText;
        public TextMeshProUGUI wordConjugationClassText;
    }
}
