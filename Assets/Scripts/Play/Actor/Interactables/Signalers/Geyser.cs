using Harmony;
using UnityEngine;

namespace Game
{
    //Author : William L
    public class Geyser : MonoBehaviour 
    {
        [SerializeField] private float thrusterStrength;
        [Tooltip("Nom du paramètre qui s'occupe d'activer l'animation du geyser")]
        [SerializeField] private string animatorTriggerParameter = "isTriggered";

        [Header("Audio")]
        [SerializeField] private AudioClip geyserSound;
        
        private InteractSensor interactSensor;
        private Rigidbody2D playerRigidbody;
        private Animator animator;
        private GameMemory gameMemory;
        private AudioSource audioSource;

        private void Awake()
        {
            interactSensor = GetComponent<InteractSensor>();
            playerRigidbody = Finder.Player.GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            gameMemory = Finder.GameMemory;
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            interactSensor.OnInteract += OnActivated;
        }

        private void OnDisable()
        {
            interactSensor.OnInteract -= OnActivated;
        }

        private void OnActivated(InteractSensor interactSensor)
        {
            if(gameMemory.HasCollectedArtefact(ArtefactType.Geyser))
            {
                audioSource.PlayOneShot(geyserSound);
                playerRigidbody.velocity = (playerRigidbody.velocity * Vector2.right) + (Vector2.up * thrusterStrength);
                ActivateAnimation();
            }
        }

        private void ActivateAnimation()
        {
            animator.SetTrigger(animatorTriggerParameter);
        }
    }
}