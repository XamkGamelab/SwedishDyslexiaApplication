using UnityEngine;

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
        [SerializeField] private AudioSource _menuMusic1;   // Not depressing I hope
        [SerializeField] private AudioSource _menuMusic2;   // Dyslexiyass
        [SerializeField] private AudioSource _gameMusic;    // Background sound
        public bool _menuMusic1Playing = false;
        public bool _menuMusic2Playing = false;
        public bool _buzzingPlaying = false;
        public bool _gameMusicPlaying = false;

        [Header("SFX")]
        [SerializeField] private AudioSource _menuSelect1;  // Beep
        [SerializeField] private AudioSource _menuSelect2;  // Bonk
        [SerializeField] private AudioSource _menuSelect3;  // Elevator
        [SerializeField] private AudioSource _buzzing;

        [SerializeField] private AudioSource _lightModeToggle;
        [SerializeField] private AudioSource _inputSound;
        [SerializeField] private AudioSource _correct;
        [SerializeField] private AudioSource _incorrect;

        private void Awake()
        {
            AudioManager._instance = this;
        }
        public void StartMenuMusic1()
        {
            if (!_menuMusic1Playing)
            {
                _menuMusic1.Play();
                _menuMusic2.Stop();
                _gameMusic.Stop();
                _buzzing.Stop();
                _menuMusic1.volume = 0.5f;
                _menuMusic1.pitch = 0.7f;
                _menuMusic1Playing = true;
                _menuMusic2Playing = false;
                _gameMusicPlaying = false;
                _buzzingPlaying = false;
            }
        }
        public void StartMenuMusic2()  // Alternative music
        {
            if (!_menuMusic2Playing)
            {
                _menuMusic1.Stop();
                _menuMusic2.Play();
                _gameMusic.Stop();
                _buzzing.Stop();
                _menuMusic2.volume = 0.5f;
                _menuMusic2.pitch = 1.0f;
                _menuMusic1Playing = false;
                _menuMusic2Playing = true;
                _gameMusicPlaying = false;
                _buzzingPlaying = false;
            }
        }

        public void StartGameMusic()
        {
            if (!_gameMusicPlaying)
            {
                _menuMusic1.Stop();
                _menuMusic2.Stop();
                _gameMusic.Play();
                _buzzing.Stop();
                _gameMusic.volume = 0.5f;
                _gameMusic.pitch = 0.9f;
                _menuMusic1Playing = false;
                _menuMusic2Playing = false;
                _gameMusicPlaying = true;
                _buzzingPlaying = false;
            }
        }

        public void StartBuzzing()  // Buzzing
        {
            if (!_buzzingPlaying)
            {
                _menuMusic1.Stop();
                _menuMusic2.Stop();
                _gameMusic.Stop();
                _buzzing.Play();
                _buzzing.volume = 0.5f;
                _menuMusic1Playing = false;
                _menuMusic2Playing = false;
                _gameMusicPlaying = false;
                _buzzingPlaying = true;
            }
        }

        public void PlayMenuSelect1()
        {
            _menuSelect1.Play();
            _menuSelect1.volume = 0.5f;
        }
        public void PlayMenuSelect2()
        {
            _menuSelect2.Play();
            _menuSelect2.volume = 0.5f;
        }
        public void PlayMenuSelect3()
        {
            _menuSelect3.Play();
            _menuSelect3.volume = 0.5f;
        }
        public void PlayLightModeToggle()
        {
            _lightModeToggle.Play();
            _lightModeToggle.volume = 1.0f;
            _lightModeToggle.panStereo = -0.6f;
        }
        public void PlayInputSound()
        {
            _inputSound.Play();
            _inputSound.volume = 0.5f;
        }
        public void PlayCorrect()
        {
            _correct.Play();
            _correct.volume = 0.5f;
        }
        public void PlayIncorrect()
        {
            _incorrect.Play();
            _incorrect.volume = 0.5f;
        }
    }

}

// Tutorial 1: https://adamwreed93.medium.com/how-to-create-and-utilize-an-audio-manager-in-unity-627123d2483
// Tutorial 2: https://medium.com/@cwagoner78/creating-an-organized-sound-system-and-playing-sounds-in-unity-82cbb48060ff