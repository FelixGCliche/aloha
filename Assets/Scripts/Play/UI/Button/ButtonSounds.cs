using UnityEngine;

namespace Game
{
    public class ButtonSounds : MonoBehaviour
    {
        [SerializeField] private AudioClip selectSound;
        [SerializeField] private AudioClip highlightSound;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        //Appeler avec l’évent trigger component
        public void PlaySelectSound()
        {
            audioSource.PlayOneShot(selectSound);
        }
        
        //Appeler avec l’évent trigger component
        public void PlayHighlightSound()
        {
            audioSource.PlayOneShot(highlightSound);
        }
    }
}