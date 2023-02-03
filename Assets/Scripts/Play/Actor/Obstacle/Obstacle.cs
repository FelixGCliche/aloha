using UnityEngine;

namespace Game
{
    // Author : Félix GC
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 1.0f)] private float damage = 1.0f;

        private ISensor<IHurtable> hurtableSensor;

        private void Awake()
        {
            hurtableSensor = GetComponent<ColliderSensor2D>().For<IHurtable>();
        }

        private void Update()
        {
            foreach (var sensedHurtable in hurtableSensor.SensedObjects)
                OnSenseCollision(sensedHurtable);
        }

        private void OnSenseCollision(IHurtable sensedHurtable)
        {
            if (sensedHurtable != null)
                sensedHurtable.Hurt(damage);
        }
    }
}