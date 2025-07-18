using UnityEngine;

namespace SwedishApp.Core
{
    /// <summary>
    /// This class holds the volume and pitch values used in the audio manager
    /// </summary>
    [System.Serializable]
    public class MusicData : MonoBehaviour
    {
        public AudioClip audioClip;
        public float volume = 1.0f;
        public float pitch;
    }
}
