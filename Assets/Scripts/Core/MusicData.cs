using UnityEngine;

namespace SwedishApp.Core
{
    [System.Serializable]
    public class MusicData : MonoBehaviour
    {
        public AudioClip audioClip;
        public float volume = 1.0f;
        public float pitch;
    }
}
