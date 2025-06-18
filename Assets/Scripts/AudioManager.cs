using UnityEngine;

namespace RAY
{
    public class AudioManager : MonoBehaviour
    {
        [Header("音效來源")]
        public AudioSource audioSource;

        [Header("音效剪輯")]
        public AudioClip backgroundMusic;
        public AudioClip correctSound;
        public AudioClip wrongSound;
        public AudioClip winSound;

        private void Start()
        {
            if (backgroundMusic != null && audioSource != null)
            {
                audioSource.clip = backgroundMusic;
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        public void PlayCorrect()
        {
            if (correctSound != null)
                audioSource.PlayOneShot(correctSound);
        }

        public void PlayWrong()
        {
            if (wrongSound != null)
                audioSource.PlayOneShot(wrongSound);
        }

        public void PlayWin()
        {
            if (winSound != null)
                audioSource.PlayOneShot(winSound);
        }
    }
}
