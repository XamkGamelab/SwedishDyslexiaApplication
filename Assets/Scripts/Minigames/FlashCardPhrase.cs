using TMPro;

namespace SwedishApp.Minigames
{
    public class FlashCardPhrase : FlashCardBase
    {
        public TextMeshProUGUI wordNoticeText;
        public InfoHover infoHover;
        public void HideInfo()
        {
            infoHover.infoTextObject.SetActive(false);
        }
    }
}
