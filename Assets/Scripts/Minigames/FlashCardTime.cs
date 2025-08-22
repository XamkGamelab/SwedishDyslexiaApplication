using SwedishApp.UI;
using TMPro;

namespace SwedishApp.Minigames
{
    /// <summary>
    /// Inheriting from <see cref="FlashCardBase"/>, this class houses
    /// additional text fields related to the word type.
    /// </summary>
    public class FlashCardTime : FlashCardBase
    {
        public TextMeshProUGUI wordMonthNumber;

        public override void LightsOn()
        {
            base.LightsOn();
            wordMonthNumber.color = UIManager.Instance.DarkmodeHighlight;
        }

        public override void LightsOff()
        {
            base.LightsOff();
            wordMonthNumber.color = UIManager.Instance.LightmodeHighlight;
        }
    }
}