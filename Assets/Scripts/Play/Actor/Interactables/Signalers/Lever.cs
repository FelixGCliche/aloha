using UnityEngine;

namespace Game
{
    // Author: David D
    public class Lever : MonoBehaviour
    {
        [Header("Audio")] 
        [SerializeField] private AudioClip flickSound;
        
        private InteractSensor interactSensor;
        private SignalSender signalSender;
        private SpriteRenderer spriteRenderer;
        private AudioSource audioSource;
        
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            interactSensor = GetComponent<InteractSensor>();
            signalSender = GetComponent<SignalSender>();
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            interactSensor.OnInteract += OnInteracted;
        }

        private void OnDisable()
        {
            interactSensor.OnInteract -= OnInteracted;
        }

        private void OnInteracted(InteractSensor interactSensor)
        {
            audioSource.PlayOneShot(flickSound);
            signalSender.ToggleActivation();
            ChangeVisuals();
        }

        private void ChangeVisuals()
        {
            spriteRenderer.flipX = signalSender.RawActivated;
        }
    }
}