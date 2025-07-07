using UnityEngine;
using UnityEngine.UI;

namespace SwedishApp.Core
{
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
        [SerializeField] private AudioSource _menuSelect1;  // Beep
        [SerializeField] private AudioSource _menuSelect2;  // Bonk
        [SerializeField] private AudioSource _menuSelect3;  // Elevator
        [SerializeField] private AudioSource _menuSelect4;  // Elevator 2
        [SerializeField] private AudioSource _menuSelect5;  // Drum 1
        [SerializeField] private AudioSource _menuSelect6;  // Drum 2
        [SerializeField] private AudioSource _menuSelect7;  // Drum 3
        [SerializeField] private AudioSource _menuSelect8;  // Drum 4

        [SerializeField] private AudioSource _lightModeToggle;
        [SerializeField] private AudioSource _inputSound;
        [SerializeField] private AudioSource _correct;
        [SerializeField] private AudioSource _incorrect;

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
            if(!PlayerPrefs.HasKey("musicVolume"))      // If there isn't previous saved values, set volume to default
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

            _menuSelect1.       volume = volumeSlider.value /*1.0f * */;
            _menuSelect2.       volume = volumeSlider.value /*2.0f * */;
            _menuSelect3.       volume = volumeSlider.value /*0.05f * */;
            _lightModeToggle.   volume = volumeSlider.value /*1.0f * */;
            _inputSound.        volume = volumeSlider.value /*1.0f * */;
            _correct.           volume = volumeSlider.value /*0.5f * */;
            _incorrect.         volume = volumeSlider.value /*1.0f * */;
            _menuSelect4.       volume = volumeSlider.value;
            _menuSelect5.       volume = volumeSlider.value;
            _menuSelect6.       volume = musicVolume;
            _menuSelect7.       volume = musicVolume;
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
            _menuSelect1.Play();
            Debug.Log(volumeSlider.value);
        }
        public void PlayMenuSelect2()
        {
            _menuSelect2.Play();
        }
        public void PlayMenuSelect3()
        {
            _menuSelect3.Play();
        }
        public void PlayMenuSelect4()
        {
            _menuSelect4.Play();
        }
        public void PlayMenuSelect5()
        {
            _menuSelect5.Play();
        }
        public void PlayMenuSelect6()
        {
            _menuSelect6.Play();
        }
        public void PlayMenuSelect7()
        {
            _menuSelect7.Play();
        }
        public void PlayMenuSelect8()
        {
            _menuSelect8.Play();
        }
        public void PlayLightModeToggle()
        {
            _lightModeToggle.Play();
        }
        public void PlayInputSound()
        {
            _inputSound.Play();
        }
        public void PlayCorrect()
        {
            _correct.Play();
        }
        public void PlayIncorrect()
        {
            _incorrect.Play();
        }
    }
}