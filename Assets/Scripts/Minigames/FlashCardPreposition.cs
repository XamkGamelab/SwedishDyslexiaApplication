using TMPro;

namespace SwedishApp.Minigames
{
    /// <summary>
    /// Inheriting from <see cref="FlashCardBase"/>, this class houses
    /// additional text fields related to the word type.
    /// </summary>
    public class FlashCardPreposition : FlashCardBase
    {
        public TextMeshProUGUI wordNoticeText;
        public InfoHover infoHover;
        public void HideInfo()
        {
            infoHover.infoTextObject.SetActive(false);
        }
    }
}
