using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SwedishApp.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance { get; private set; }
        [SerializeField] private Button toggleSettingsBtn;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private Button toggleLightmodeBtn;
        [SerializeField] private Slider toggledSlider;
        [SerializeField] private float lerpDuration = 0.06f;
        public bool LightmodeOn { get; private set; } = false;
        private bool settingsOpen = false;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
                Debug.LogError($"Found more than one UIManager, destroying duplicate. Fix this!");
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            toggleLightmodeBtn.onClick.AddListener(ToggleSlider);
            toggleSettingsBtn.onClick.AddListener(ToggleSettingsMenu);
        }

        private void ToggleSettingsMenu()
        {
            if (settingsOpen)
            {
                settingsMenu.SetActive(false);
                settingsOpen = false;
            }
            else
            {
                settingsMenu.SetActive(true);
                settingsOpen = true;
            }
        }

        private void ToggleSlider()
        {
            StartCoroutine(SliderLerp());
        }

        private IEnumerator SliderLerp()
        {
            toggleLightmodeBtn.interactable = false;
            float timer = 0f;

            if (LightmodeOn)
            {
                while (timer < lerpDuration)
                {
                    timer += Time.deltaTime;
                    toggledSlider.value = Mathf.Lerp(1f, 0f, timer / lerpDuration);
                    yield return null;
                }
                LightmodeOn = false;
            }
            else
            {
                while (timer < lerpDuration)
                {
                    timer += Time.deltaTime;
                    toggledSlider.value = Mathf.Lerp(0f, 1f, timer / lerpDuration);
                    yield return null;
                }
                LightmodeOn = true;
            }

            toggleLightmodeBtn.interactable = true;
        }
    }
}
