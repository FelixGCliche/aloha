using System.Collections;
using UnityEngine;

namespace Game
{
    // Author: David D
    public class EnemyIceBlockController : MonoBehaviour
    {
        [SerializeField] private float freezeTime = 4f;
        [SerializeField] private float massWhenFrozen = 1f;
        
        [Header("Audio")]
        [SerializeField] private AudioClip freezeSound;
        
        private BoxCollider2D boxCollider2D;
        private Enemy enemy;
        private SpriteRenderer spriteRenderer;
        private new Rigidbody2D rigidbody2D;
        private float defaultMass;
        private AudioSource audioSource;
        
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();
            enemy = GetComponentInParent<Enemy>();
            rigidbody2D = GetComponentInParent<Rigidbody2D>();
            defaultMass = rigidbody2D.mass;
            audioSource = GetComponentInParent<AudioSource>();
        }

        public void EnableFreeze()
        {
            spriteRenderer.enabled = true;
            boxCollider2D.enabled = true;
            enemy.enabled = false;
            rigidbody2D.mass = massWhenFrozen;
            audioSource.PlayOneShot(freezeSound);
            
            StartCoroutine(UnFreezeCoroutine());
        }
        
        public void DisableFreeze()
        {
            spriteRenderer.enabled = false;
            boxCollider2D.enabled = false;
            enemy.enabled = true;
            rigidbody2D.mass = defaultMass;
        }
        
        private IEnumerator UnFreezeCoroutine()
        {
            yield return new WaitForSeconds(freezeTime);
            DisableFreeze();
        }
    }
}