using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    // Author: David Dorion, Felix B
    public class TemporaryPlatform : MonoBehaviour
    {
        [Header("Temporary Platform")] 
        [SerializeField] [Min(0f)] private float respawnDelay = 2f;
        [SerializeField] [Min(0f)] private float disappearDelay = 2f;
        [SerializeField] [Min(0f)] private float fadeInDuration = 0.2f;
        [SerializeField] [Min(0f)] private float fadeOutDuration = 0.2f;
        [SerializeField] [Min(0f)] private float defaultShakeSpeed = 0.5f;
        [SerializeField] [Min(0f)] private float disappearShakeSpeed = 0.7f;
        [SerializeField] [Min(0f)] private string shakeSpeedParamName = "_ShakeUvSpeed";

        [Header("Audio")] 
        [SerializeField] private AudioClip rocksTumblingSound;

        private BoxCollider2D boxCollider2D;
        private SpriteRenderer spriteRenderer;
        private Vector2 startPosition;
        private bool hasReappeared;
        private ISensor<Character> characterSensor;
        private Material mat;
        private AudioSource audioSource;
        
        private void Awake()
        {
            startPosition = transform.position;
            boxCollider2D = GetComponent<BoxCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            mat = spriteRenderer.material;

            characterSensor = GetComponentInChildren<TriggerSensor2D>().For<Character>();

            audioSource = GetComponent<AudioSource>();
            
            hasReappeared = false;
        }

        private void OnEnable()
        {
            characterSensor.OnSensedObject += OnSensedCharacter;
            characterSensor.OnUnsensedObject += OnSensedCharacter;
        }
        private void OnDisable()
        {
            characterSensor.OnSensedObject -= OnSensedCharacter;
            characterSensor.OnUnsensedObject -= OnSensedCharacter;
        }

        private void Update()
        {
            var shakeSpeed = hasReappeared ? defaultShakeSpeed:disappearShakeSpeed;
            
             mat.SetFloat(shakeSpeedParamName,shakeSpeed);
        }

        private void OnSensedCharacter(Character character)
        {
             if (hasReappeared) return;
            
             StartCoroutine(DisappearRoutine());
        }
        
        private void OnunsensedCharacter(Character character) { /* Needed because of a bug*/ }
        
        private IEnumerator DisappearRoutine()
        {
            //Il commence à trembler fortement au contact.
            hasReappeared = true;
            yield return new WaitForSeconds(disappearDelay);
            //Il disparait.
            boxCollider2D.enabled = false;
            audioSource.PlayOneShot(rocksTumblingSound);
            mat.DOFade(0, fadeOutDuration);
            yield return spriteRenderer.DOFade(0, fadeOutDuration).WaitForCompletion();
            //Il se replace et attend de réaparaitre.
            transform.position = startPosition;
            yield return new WaitForSeconds(respawnDelay);
            //Il commence à réaparaitre.
            spriteRenderer.DOFade(1, fadeInDuration);
            yield return mat.DOFade(1, fadeInDuration).WaitForCompletion();;
            boxCollider2D.enabled = true;
            //Il est réaparu.
            hasReappeared = false;
        }
    }
}