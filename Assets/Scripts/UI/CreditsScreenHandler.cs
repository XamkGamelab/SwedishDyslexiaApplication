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
            if (UIManager.Instance.LightmodeOn)
            {
                ToLightmode();
            }
            else
            {
                ToDarkmode();
            }
            UIManager.Instance.LightmodeOnEvent += ToLightmode;
            UIManager.Instance.LightmodeOffEvent += ToDarkmode;
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
    }
}
