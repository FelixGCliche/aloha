using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David D
    public class Artefact : MonoBehaviour
    {
        [SerializeField] private ArtefactType artefactType = ArtefactType.None;
        [SerializeField] [Tooltip("Nom du Trigger de L'animation")] private string animationTriggerName = "Openning";
        [SerializeField] private Sprite collectedSprite;
        
        private OnArtefactCollectEventChannel artefactCollectEventChannel;
        private ISensor<IArtefactCollector> artefactCollectorSensor;
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private SignalSender signalSender;

        public ArtefactType ArtefactType => artefactType;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            signalSender = GetComponent<SignalSender>();
            artefactCollectEventChannel = Finder.OnArtefactCollectEventChannel;
            artefactCollectorSensor = GetComponent<TriggerSensor2D>().For<IArtefactCollector>();
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            artefactCollectorSensor.OnSensedObject += OnArtefactCollected;
        }

        private void OnDisable()
        {
            artefactCollectorSensor.OnSensedObject -= OnArtefactCollected;
        }

        private void OnArtefactCollected(IArtefactCollector artefactCollector)
        {
            if (artefactCollector == null) return;

            signalSender.IsActivated = true;
            artefactCollectEventChannel.Publish(artefactType);
            artefactCollector.CollectArtefact(artefactType);
            StartCoroutine(DisableRoutine());
        }

        private IEnumerator DisableRoutine()
        {
            animator.SetTrigger(animationTriggerName);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            gameObject.GetComponent<TriggerSensor2D>().enabled = false;
        }

        public void SetToAlreadyCollected()
        {
            animator.enabled = false;
            spriteRenderer.sprite = collectedSprite;
        }
    }
}