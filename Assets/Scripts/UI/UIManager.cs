using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace SwedishApp.UI
{
    /// <summary>
    /// This class handles the majority of UI functionality in this project.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        //Singleton
        public static UIManager instance { get; private set; }

        //Lightmode related
        public bool LightmodeOn { get; private set; } = false;
        public event Action LightmodeOnEvent;
        public event Action LightmodeOffEvent;

        //Font related
        [field: SerializeField] public TMP_FontAsset legibleFont { get; private set; }
        [field: SerializeField] public TMP_FontAsset basicFont { get; private set; }
        public int legibleSpacing { get; private set; } = 8;
        public int basicSpacing { get; private set; } = 0;
        [SerializeField] private Toggle toggleHyperlegible;
        [SerializeField] private Toggle fontSmallToggle;
        [SerializeField] private Toggle fontMediumToggle;
        [SerializeField] private Toggle fontLargeToggle;
        private bool fontSettingsSubscribed = false;
        public event Action LegibleModeOnEvent;
        public event Action LegibleModeOffEvent;
        public event Action FontSmallEvent;
        public event Action FontMediumEvent;
        public event Action FontLargeEvent;
        public event Action FontLegibleEvent;
        public event Action FontBasicEvent;

        enum FontSize
        {
            small = 1,
            medium,
            large
        }

        //Settings related
        private bool settingsOpen = false;
        [SerializeField] private Button toggleSettingsBtn;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private Button toggleLightmodeBtn;
        [SerializeField] private Slider toggledSlider;
        [SerializeField] private float lerpDuration = 0.06f;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);  //Destroy is called at end of frame don't worry
                Debug.LogError($"Found more than one UIManager, destroying duplicate. Fix this!");
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            fontSmallToggle.onValueChanged.AddListener((toggleOn) => PickFontSize(FontSize.small, toggleOn));
            fontMediumToggle.onValueChanged.AddListener((toggleOn) => PickFontSize(FontSize.medium, toggleOn));
            fontLargeToggle.onValueChanged.AddListener((toggleOn) => PickFontSize(FontSize.large, toggleOn));
            toggleHyperlegible.onValueChanged.AddListener((toggleOn) =>
            {
                if (toggleOn)
                {
                    LegibleModeOnEvent?.Invoke();
                }
                else
                {
                    LegibleModeOffEvent?.Invoke();
                }
            });
            
            toggleLightmodeBtn.onClick.AddListener(ToggleLightmode);
            toggleSettingsBtn.onClick.AddListener(ToggleSettingsMenu);
        }

        #region lightmode related methods

        private void ToggleLightmode()
        {
            if (LightmodeOn)
            {
                LightmodeOnEvent?.Invoke();
            }
            else
            {
                LightmodeOffEvent?.Invoke();
            }
            StartCoroutine(SliderLerp());
        }

        #endregion

        #region settings related methods

        private void PickFontSize(FontSize _size, bool _toggledOn)   //font size is an enum, 1=small, 2=medium, 3=large
        {
            if (!_toggledOn) return;

            switch (_size)
            {
                case FontSize.small:
                    fontSmallToggle.interactable = false;
                    fontMediumToggle.interactable = true;
                    fontLargeToggle.interactable = true;
                    fontMediumToggle.isOn = false;
                    fontLargeToggle.isOn = false;
                    FontSmallEvent?.Invoke();
                    break;
                case FontSize.medium:
                    fontSmallToggle.interactable = true;
                    fontMediumToggle.interactable = false;
                    fontLargeToggle.interactable = true;
                    fontSmallToggle.isOn = false;
                    fontLargeToggle.isOn = false;
                    FontMediumEvent?.Invoke();
                    break;
                case FontSize.large:
                    fontSmallToggle.interactable = true;
                    fontMediumToggle.interactable = true;
                    fontLargeToggle.interactable = false;
                    fontSmallToggle.isOn = false;
                    fontMediumToggle.isOn = false;
                    FontLargeEvent?.Invoke();
                    break;
            }
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

        #endregion
    }
}
