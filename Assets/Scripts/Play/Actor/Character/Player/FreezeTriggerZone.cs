using System.Collections;
using UnityEngine;

namespace Game
{
    // Author: David D
    public class FreezeTriggerZone : MonoBehaviour
    {
        [SerializeField] [Min(0f)] private float coolDownTime = 5f;
        
        [Header("Audio")]
        [SerializeField] private AudioClip freezePowerSound;
        
        private ISensor<Enemy> enemySensor;
        private bool isFreezing;
        private AudioSource audioSource;

        public float CoolDownTime => coolDownTime;

        private void Awake()
        {
            enemySensor = GetComponent<TriggerSensor2D>().For<Enemy>();
            audioSource = GetComponentInParent<AudioSource>();
            
            isFreezing = false;
        }

        public bool Freeze()
        {
            if (isFreezing) return false;
            
            StartCoroutine(FreezeRoutine());
            return true;
        }

        private IEnumerator FreezeRoutine()
        {
            isFreezing = true;
            audioSource.PlayOneShot(freezePowerSound);
            foreach (var enemy in enemySensor.SensedObjects) enemy.Freeze();
            yield return new WaitForSeconds(coolDownTime);
            isFreezing = false;
        }
    }
}