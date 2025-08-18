using System.Collections;
using TMPro;
using UnityEngine;

namespace SwedishApp.UI
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
            "Kun koet osaavasi substantiivien kään\u00ADtämisen, kokeile myös taivuttamista!",
            "Valtaosa verbeistä seuraavat luok\u00ADkansa (esim. I, II...) taivutussääntöjä.",
            "Joidenkin sanojen opettelukorteissa on lisätietoa sanan käytöstä!",
            "Substantiivien sanaluokka (esim. 1, 2...) kertoo, miten sana taipuu.",
            "Yritä muistella peruskoulun muistisääntöjä, kuten KoPuTuS-X tai KonSuKiePre"
        };

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            tipField = GetComponent<TextMeshProUGUI>();
            tipField.RegisterDirtyLayoutCallback(() => UIManager.Instance.FixTextSpacing(tipField));
            StartCoroutine(CycleTipsTEST());
        }

        private IEnumerator CycleTipsTEST()
        {
            int i = 0;

            while (true)
            {
                yield return new WaitForSeconds(3f);
                tipField.text = tips[i];
                i++;
                if (i >= tips.Length) i = 0;
            }
        }

        public void RandomizeTip()
        {
            int i = Random.Range(0, tips.Length);
            tipField.text = tips[i];
        }
    }
}
