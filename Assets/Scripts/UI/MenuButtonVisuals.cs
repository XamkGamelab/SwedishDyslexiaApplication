using UnityEngine;
using UnityEngine.UI;

namespace SwedishApp.UI
{
    public class MenuButtonVisuals : MonoBehaviour
    {
        [SerializeField] private Image translateToSweImg;
        [SerializeField] private Image translateToFinImg;
        [SerializeField] private Sprite toSweDarkmode, toSweLightmode, toFinDarkmode, toFinLightmode;

        private void Start()
        {
            UIManager.instance.LightmodeOnEvent += ToLightmode;
            UIManager.instance.LightmodeOffEvent += ToDarkmode;
        }

        private void ToLightmode()
        {
            translateToSweImg.sprite = toSweLightmode;
            translateToFinImg.sprite = toFinLightmode;
        }
        
        private void ToDarkmode()
        {
            translateToSweImg.sprite = toSweDarkmode;
            translateToFinImg.sprite = toFinDarkmode;
        }
    }
}
