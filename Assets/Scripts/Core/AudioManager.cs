using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Audio;
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
        [SerializeField] private float defaultVolume = 1f;
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private Slider volumeSlider;

        [Header("Music")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioClip[] musicArray;
        private int musicIndex = 0;

        [Header("SFX")]
        [SerializeField] private AudioSource sfxSource;

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
            float savedVol = PlayerPrefs.GetFloat("volume", defaultVolume);
            SetVolume(savedVol);
            volumeSlider.value = savedVol;
            volumeSlider.onValueChanged.AddListener(SetVolume);
            musicSource.Play();
        }

        private void Update()
        {
            if (!musicSource.isPlaying) NextMusic();
        }

        private void NextMusic()
        {
            musicIndex++;
            if (musicIndex >= musicArray.Length)
            {
                musicIndex = 0;
            }

            musicSource.clip = musicArray[musicIndex];
            musicSource.Play();
        }

        private void SetVolume(float _volume)
        {
            if (_volume < 1) _volume = 0.0001f;

            mixer.SetFloat("Volume", Mathf.Log10(_volume / 100) * 20f);
            PlayerPrefs.SetFloat("volume", volumeSlider.value);


            mixer.GetFloat("Volume", out float test);
            Debug.Log($"new adjusted volume {test}");
        }

        public void ToggleMute(bool _mute)
        {
            musicSource.mute = _mute;
            sfxSource.mute = _mute;
        }

        public void PlayClip(AudioClip _clipToPlay)
        {
            sfxSource.PlayOneShot(_clipToPlay);
        }

        public void PlayMusic(AudioClip _musicToPlay)
        {
            musicSource.clip = _musicToPlay;
            musicSource.Play();
        }
    }
}