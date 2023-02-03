using System.Collections;
using UnityEngine;

namespace Game
{
    // Author : Félix B
    public class ConcreteDashDestroyable : MonoBehaviour, IDashDestroyable
    {
        [SerializeField] private bool doorBreakingArtefactNeeded = true;

        [Header("Audio")]
        [SerializeField] private AudioClip breakingSound;

        private new Collider2D collider;
        private AudioSource audioSource;
        private SpriteRenderer spriteRenderer;

        public Collider2D Collider => collider;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            collider = GetComponentInChildren<Collider2D>();
            audioSource = GetComponent<AudioSource>();
        }

        public bool DoorBreakingArtefactNeeded => doorBreakingArtefactNeeded;

        public IEnumerator DestructionCountDown(float timeBeforeDestruction)
        {
            collider.enabled = false;
            audioSource.PlayOneShot(breakingSound);
            yield return new WaitForSeconds(timeBeforeDestruction);
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(breakingSound.length);
            gameObject.SetActive(false);
        }
    }
}