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
            if (UIManager.instance.LightmodeOn)
            {
                ToLightmode();
            }
            else
            {
                ToDarkmode();
            }
            UIManager.instance.LightmodeOnEvent += ToLightmode;
            UIManager.instance.LightmodeOffEvent += ToDarkmode;
        }

        private void ToLightmode()
        {
            foreach (TextMeshProUGUI text in creditTexts)
            {
                text.color = UIManager.instance.Darkgrey;
            }
            backButton.image.sprite = UIManager.instance.abortSpriteLightmode;
            background.color = UIManager.instance.Lightgrey;
            xgsLogo.sprite = xgsLogoLM;
        }

        private void ToDarkmode()
        {
            foreach (TextMeshProUGUI text in creditTexts)
            {
                text.color = UIManager.instance.Lightgrey;
            }
            backButton.image.sprite = UIManager.instance.abortSpriteDarkmode;
            background.color = UIManager.instance.Darkgrey;
            xgsLogo.sprite = xgsLogoDM;
        }
    }
}
