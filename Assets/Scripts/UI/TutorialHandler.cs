using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SwedishApp.UI
{
    public class TutorialHandler : MonoBehaviour
    {
        [Header("MAKE SURE THE UNIQUE ID IS UNIQUE")]
        public string tutUniqueId;
        [SerializeField] private Button tutBackgroundButton;
        [SerializeField] private Button tutInfoButton;
        [SerializeField] private Image bgImage;
        [SerializeField] private Image infoImage;
        [SerializeField] private Sprite infoSpriteLightmode;
        [SerializeField] private Sprite infoSpriteDarkmode;
        [SerializeField] private TextMeshProUGUI tutorialText;
        public bool shouldBeVisible { get; private set; } = false;

        private void Start()
        {
            tutBackgroundButton.onClick.AddListener(CloseTutorial);
            tutInfoButton.onClick.AddListener(CloseTutorial);
        }

        public void ShowTutorial()
        {
            shouldBeVisible = true;
            if (!UIManager.Instance.TutorialsOff)
            {
                gameObject.SetActive(true);
            }
        }

        public void ResetSeen()
        {
            PlayerPrefs.SetInt(tutUniqueId, 0);
        }

        public bool TutorialSeen()
        {
            return PlayerPrefs.GetInt(tutUniqueId) == 1;
        }

        private void CloseTutorial()
        {
            gameObject.SetActive(false);
            shouldBeVisible = false;
            PlayerPrefs.SetInt(tutUniqueId, 1);
        }

        public void ToLightmode()
        {
            infoImage.sprite = infoSpriteLightmode;
            tutorialText.color = UIManager.Instance.Darkgrey;
            bgImage.sprite = UIManager.Instance.ButtonSpriteLightmode;
        }

        public void ToDarkmode()
        {
            infoImage.sprite = infoSpriteDarkmode;
            tutorialText.color = UIManager.Instance.Lightgrey;
            bgImage.sprite = UIManager.Instance.ButtonSpriteDarkmode;
        }

        public void ToLegibleFont()
        {
            tutorialText.font = UIManager.Instance.LegibleFont;
            tutorialText.characterSpacing = UIManager.Instance.LegibleSpacing;
        }

        public void ToBasicFont()
        {
            tutorialText.font = UIManager.Instance.BasicFont;
            tutorialText.characterSpacing = UIManager.Instance.BasicSpacing;
        }
    }
}
