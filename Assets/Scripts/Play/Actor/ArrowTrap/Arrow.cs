using System.Collections;
using UnityEngine;

namespace Game
{
    // Author: David D
    public class Arrow : MonoBehaviour, IEntity
    {
        [SerializeField] [Min(0f)] private float timeAlive = 8f;
        [SerializeField] [Range(0f, 1f)] private float damage = 0.2f;

        [Header("Audio")] 
        [SerializeField] private AudioClip arrowHitSound;
        
        private BoxCollider2D boxCollider2D;
        private bool hitSurface;
        private Rigidbody2D rigidbody;
        private ISensor<Collider2D> colliderSensor;
        private AudioSource audioSource;

        public Vector3 Position { get; }

        private void Awake()
        {
            hitSurface = false;
            boxCollider2D = GetComponent<BoxCollider2D>();
            rigidbody = GetComponent<Rigidbody2D>();
            colliderSensor = GetComponent<ColliderSensor2D>().For<Collider2D>();
            audioSource = GetComponent<AudioSource>();
        }
        
        private void OnEnable()
        {
            colliderSensor.OnSensedObject += OnAnyCollisionSensed;
            colliderSensor.OnUnsensedObject += OnAnyCollisionUnsensed;
            
            boxCollider2D.enabled = true;
            hitSurface = false;
            rigidbody.constraints = RigidbodyConstraints2D.None;
            
            StartCoroutine(StartDisableTimer());
        }

        private void OnDisable()
        {
            colliderSensor.OnSensedObject -= OnAnyCollisionSensed;
            colliderSensor.OnUnsensedObject -= OnAnyCollisionUnsensed;
        }

        private void Update()
        {
            if (hitSurface) return;
            
            transform.right = -rigidbody.velocity;
        }

        private void OnAnyCollisionUnsensed(Collider2D other) {/*Needed because of a bug*/}

        private void OnAnyCollisionSensed(Collider2D other)
        {
            audioSource.PlayOneShot(arrowHitSound);
            
            if (!hitSurface && other.GetComponent<Player>()!= null)
            {
                other.GetComponent<Player>()?.Hurt(damage);
                gameObject.SetActive(false);
                StopCoroutine(StartDisableTimer());
            }
            
            hitSurface = true;
            boxCollider2D.enabled = false;
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private IEnumerator StartDisableTimer()
        {
            yield return new WaitForSeconds(timeAlive);
            gameObject.SetActive(false);
        }
    }
}