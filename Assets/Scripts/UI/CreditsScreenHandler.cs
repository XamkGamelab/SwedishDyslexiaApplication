using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SwedishApp.UI
{
    public class CreditsScreenHandler : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Image background;
        [SerializeField] private Image xgsLogo;
        [SerializeField] private Sprite xgsLogoDM;
        [SerializeField] private Sprite xgsLogoLM;
        private TextMeshProUGUI[] creditTexts;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        void Start()
        {
            creditTexts = transform.GetComponentsInChildren<TextMeshProUGUI>();
            if (UIManager.Instance.LightmodeOn) ToLightmode();
            else ToDarkmode();
            if (UIManager.Instance.HyperlegibleOn) ToLegibleFont();
            else ToBasicFont();
            
            UIManager.Instance.LightmodeOnEvent += ToLightmode;
            UIManager.Instance.LightmodeOffEvent += ToDarkmode;
            UIManager.Instance.LegibleModeOnEvent += ToLegibleFont;
            UIManager.Instance.LegibleModeOffEvent += ToBasicFont;
        }

        private void ToLightmode()
        {
            foreach (TextMeshProUGUI text in creditTexts)
            {
                text.color = UIManager.Instance.Darkgrey;
            }
            backButton.image.sprite = UIManager.Instance.AbortSpriteLightmode;
            background.color = UIManager.Instance.Lightgrey;
            xgsLogo.sprite = xgsLogoLM;
        }

        private void ToDarkmode()
        {
            foreach (TextMeshProUGUI text in creditTexts)
            {
                text.color = UIManager.Instance.Lightgrey;
            }
            backButton.image.sprite = UIManager.Instance.AbortSpriteDarkmode;
            background.color = UIManager.Instance.Darkgrey;
            xgsLogo.sprite = xgsLogoDM;
        }

        private void ToLegibleFont()
        {
            foreach (TextMeshProUGUI text in creditTexts)
            {
                text.font = UIManager.Instance.LegibleFont;
                text.characterSpacing = UIManager.Instance.LegibleSpacing;
            }
        }

        private void ToBasicFont()
        {
            foreach (TextMeshProUGUI text in creditTexts)
            {
                text.font = UIManager.Instance.BasicFont;
                text.characterSpacing = UIManager.Instance.BasicSpacing;
            }
        }
    }
}
