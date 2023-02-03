using System.Collections;
using UnityEngine;

namespace Game
{
    // Author: David D
    public class SignalButton : MonoBehaviour
    {
        [SerializeField] private Color inactiveColor = Color.red;
        [SerializeField] private Color activeColor = Color.green;
        [SerializeField] [Min(0f)] private float activeTime = 2f;

        [Header("Audio")]
        [SerializeField] private AudioClip clickSound;
        
        private InteractSensor interactSensor;
        private SignalSender signalSender;
        private SpriteRenderer spriteRenderer;
        private AudioSource audioSource;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = inactiveColor;
            interactSensor = GetComponent<InteractSensor>();
            signalSender = GetComponent<SignalSender>();
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            interactSensor.OnInteract += UpdateState;
        }

        private void OnDisable()
        {
            interactSensor.OnInteract -= UpdateState;
        }

        private void UpdateState(InteractSensor interactSensor)
        {
            IEnumerator PressButton()
            {
                audioSource.PlayOneShot(clickSound);
                signalSender.IsActivated = true;
                ChangeColor();
                yield return new WaitForSeconds(activeTime);
                signalSender.IsActivated = false;
                ChangeColor();
                audioSource.PlayOneShot(clickSound);
            }
            
            if (!signalSender.RawActivated)
                StartCoroutine(PressButton());
        }

        private void ChangeColor()
        {
            spriteRenderer.color = signalSender.RawActivated ? activeColor : inactiveColor;
        }
    }
}