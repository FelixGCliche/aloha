using System.Collections;
using System.Linq;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David D
    public class ArrowTrap : MonoBehaviour
    {
        [SerializeField][Min(0f)] private float reloadTime = 1.5f;
        [SerializeField][Min(0f)] private float arrowSpeed = 20f;

        [Header("Audio")]
        [SerializeField] private AudioClip shootingSound;

        private ArrowFactory arrowFactory;
        private ISensor<Player> playerSensor;
        private bool readyToFire;
        private Vector2 spawnOffset;
        private AudioSource audioSource;

        private void Awake()
        {
            arrowFactory = Finder.ArrowFactory;

            var spriteRenderer = GetComponent<SpriteRenderer>();
            var sprite = spriteRenderer.sprite;

            spawnOffset = new Vector2(-sprite.texture.width / sprite.pixelsPerUnit + 0.01f, 0);
            spawnOffset.Rotate(transform.rotation.eulerAngles.z);

            playerSensor = GetComponent<TriggerSensor2D>().For<Player>();

            audioSource = GetComponent<AudioSource>();

            readyToFire = true;
        }

        private void Update()
        {
            if (playerSensor.SensedObjects.Any() && readyToFire)
                FireArrow();
        }

        private void FireArrow()
        {
            IEnumerator ArrowCoolDownRoutine()
            {
                audioSource.PlayOneShot(shootingSound);
                readyToFire = false;
                yield return new WaitForSeconds(reloadTime);
                readyToFire = true;
            }
            
            StartCoroutine(ArrowCoolDownRoutine());
            var arrow = arrowFactory.GetNextAvailableArrow(transform.rotation, transform.position, spawnOffset,
                -transform.right * arrowSpeed);
        }
    }
}