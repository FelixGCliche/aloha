using UnityEngine;

namespace Game
{
    // Author : Félix B
    public class VitalStats : MonoBehaviour
    {
        [Header("HealthPoints")] [SerializeField] [Range(0f, 1f)]
        private float initialLifePoints = 1f;
        
        public float InitialLifePoints => initialLifePoints;
        public float LifePoints { get; private set; }
        public bool IsDead => LifePoints <= 0;

        private void Start()
        {
            ResetVitals();
        }

        public void TakeDamage(float damageAmount)
        {
            LifePoints -= damageAmount;
        }

        public void ResetVitals()
        {
            LifePoints = initialLifePoints;
        }
    }
}