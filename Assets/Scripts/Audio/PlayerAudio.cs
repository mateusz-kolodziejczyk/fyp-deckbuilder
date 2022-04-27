using UnityEngine;

namespace Audio
{
    public class PlayerAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource sfxSource;

        public void PlayAudio()
        {
            sfxSource.Play();
        }
    }
}
