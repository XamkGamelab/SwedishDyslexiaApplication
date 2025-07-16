using UnityEngine;
using UnityEngine.UI;

namespace SwedishApp.Core
{
    /// <summary>
    /// This class manages the audio assets and necessary functions for them
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;
        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("Audio manager is NULL!!");
                }
                return _instance;
            }
        }

        [Header("Music")]
        [SerializeField] private AudioSource menuMusic;
        //private bool _menuMusic1Playing = false;
        //private bool _menuMusic2Playing = false;
        //private bool _menuMusic3Playing = false;
        //private bool _buzzingPlaying = false;
        //private bool _gameMusicPlaying = false;

        [Header("SFX")]
        [SerializeField] private AudioSource menuSelect1;  // Beep
        [SerializeField] private AudioSource menuSelect2;  // Bonk
        [SerializeField] private AudioSource menuSelect3;  // Elevator
        [SerializeField] private AudioSource menuSelect4;  // Elevator 2
        [SerializeField] private AudioSource menuSelect5;  // Drum 1
        [SerializeField] private AudioSource menuSelect6;  // Drum 2
        [SerializeField] private AudioSource menuSelect7;  // Drum 3
        [SerializeField] private AudioSource menuSelect8;  // Drum 4

        [SerializeField] private AudioSource lightModeToggle;
        [SerializeField] private AudioSource correct;
        [SerializeField] private AudioSource incorrect;
        [SerializeField] private AudioSource inputSound;

        [Header("Audio settings")]
        private readonly float musicVolume = 1.0f;

        [SerializeField] public Slider volumeSlider;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this);  //Destroy is called at end of frame don't worry
                Debug.LogError($"Found more than one AudioManager, destroying duplicate. Fix this!");
            }
        }

        private void Start()
        {
            if (!PlayerPrefs.HasKey("musicVolume"))      // If there isn't previous saved values, set volume to default
            {
                PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
            }
            else
            {
                Load();
            }
        }

        private void AudioSettings()
        {
            //_menuMusic1.        volume      = musicVolume;
            //_menuMusic1.        pitch       = 0.7f;

            //_menuMusic2.        volume      = musicVolume;
            //_menuMusic2.        panStereo   = -0.3f;
            //_menuMusic2.        pitch       = 1.0f;

            //_menuMusic3.        volume      = musicVolume;
            //_menuMusic3.        panStereo   = -0.3f;
            //_menuMusic3.        pitch       = 1.0f;

            //_gameMusic.         volume      = musicVolume;
            //_gameMusic.         pitch       = 0.9f;

            //_buzzing.           volume      = 3.0f;
            //_buzzing.           panStereo   = -0.25f;

            menuSelect1.volume = volumeSlider.value /*1.0f * */;
            menuSelect2.volume = volumeSlider.value /*2.0f * */;
            menuSelect3.volume = volumeSlider.value /*0.05f * */;
            lightModeToggle.volume = volumeSlider.value /*1.0f * */;
            inputSound.volume = volumeSlider.value /*1.0f * */;
            correct.volume = volumeSlider.value /*0.5f * */;
            incorrect.volume = volumeSlider.value /*1.0f * */;
            menuSelect4.volume = volumeSlider.value;
            menuSelect5.volume = volumeSlider.value;
            menuSelect6.volume = musicVolume;
            menuSelect7.volume = musicVolume;
        }
        public void ChangeVolume()
        {
            AudioListener.volume = volumeSlider.value;
            Save();
        }

        private void Load()
        {
            volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }

        private void Save()
        {
            PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        }

        public void StartMusic(MusicData _musicData)
        {
            menuMusic.Stop();
            menuMusic.clip = _musicData.audioClip;
            menuMusic.volume = _musicData.volume;
            menuMusic.pitch = _musicData.pitch;
            menuMusic.Play();
            Debug.Log(_musicData.volume);
        }
        public void PlayMenuSelect1()
        {
            menuSelect1.Play();
            Debug.Log(volumeSlider.value);
        }
        public void PlayMenuSelect2()
        {
            menuSelect2.Play();
        }
        public void PlayMenuSelect3()
        {
            menuSelect3.Play();
        }
        public void PlayMenuSelect4()
        {
            menuSelect4.Play();
        }
        public void PlayMenuSelect5()
        {
            menuSelect5.Play();
        }
        public void PlayMenuSelect6()
        {
            menuSelect6.Play();
        }
        public void PlayMenuSelect7()
        {
            menuSelect7.Play();
        }
        public void PlayMenuSelect8()
        {
            menuSelect8.Play();
        }
        public void PlayLightModeToggle()
        {
            lightModeToggle.Play();
        }
        public void PlayInputSound()
        {
            inputSound.Play();
        }
        public void PlayCorrect()
        {
            correct.Play();
        }
        public void PlayIncorrect()
        {
            incorrect.Play();
        }
    }
}