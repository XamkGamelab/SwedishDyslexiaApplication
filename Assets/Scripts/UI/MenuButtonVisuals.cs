using UnityEngine;
using UnityEngine.UI;

namespace SwedishApp.UI
{
    public class MenuButtonVisuals : MonoBehaviour
    {
        [SerializeField] private Image translateToSweImg;
        [SerializeField] private Image translateToFinImg;
        [SerializeField] private Image flashcardIcon;
        [SerializeField] private Sprite toSweDarkmode, toSweLightmode, toFinDarkmode, toFinLightmode, flashcardLightmode, flashcardDarkmode;

        private void Start()
        {
            UIManager.Instance.LightmodeOnEvent += ToLightmode;
            UIManager.Instance.LightmodeOffEvent += ToDarkmode;
        }

        private void ToLightmode()
        {
            translateToSweImg.sprite = toSweLightmode;
            translateToFinImg.sprite = toFinLightmode;
            flashcardIcon.sprite = flashcardLightmode;
        }

        private void ToDarkmode()
        {
            translateToSweImg.sprite = toSweDarkmode;
            translateToFinImg.sprite = toFinDarkmode;
            flashcardIcon.sprite = flashcardDarkmode;
        }
    }
}
