using SwedishApp.UI;
using TMPro;
using UnityEngine;

namespace SwedishApp
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TipsHandler : MonoBehaviour
    {
        private TextMeshProUGUI tipField;
        private readonly string[] tips = new string[]
        {
            //Note: a tip should be no more than 75 visible characters long! Like this.
            //If necessary, add soft hyphenation to the words to ease line changes.

            "Voit etsiä tiettyjä sanoja sanaston \"sanahaku\" toiminnon avulla!",
            "Kun koet hallitsevasi verbien kään\u00ADtämisen, kokeile niiden taivuttamista!",
            "Valtaosa verbeistä seuraavat luok\u00ADkansa (esim. I, II...) taivutussääntöjä.",
            "Substantiivien sanaluokka (esim. 1, 2...) kertoo, miten sana taipuu"
        };

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            tipField = GetComponent<TextMeshProUGUI>();
            tipField.RegisterDirtyLayoutCallback(() => UIManager.Instance.FixTextSpacing(tipField));
        }

        public void RandomizeTip()
        {
            int i = Random.Range(0, tips.Length);
            tipField.text = tips[i];
        }
    }
}
