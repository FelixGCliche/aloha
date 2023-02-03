using UnityEngine;

namespace Game
{
    // Author : Louis RD, David D, Félix GC, William L
    public abstract class Enemy : Character
    {
        [SerializeField] protected TempState enemyType;

        private EnemyIceBlockController enemyIceBlockController;
        private ISensor<IHurtableTemperature> hurtableTemperatureSensor;

        protected EnemyIceBlockController EnemyIceBlockController => enemyIceBlockController;

        protected new void Awake()
        {
            base.Awake();
            
            hurtableTemperatureSensor = GetComponent<ColliderSensor2D>().For<IHurtableTemperature>();
            enemyIceBlockController = GetComponentInChildren<EnemyIceBlockController>();
        }

        protected void Update()
        {
            foreach (var sensedHurtableTemperature in hurtableTemperatureSensor.SensedObjects)
                OnSenseCollision(sensedHurtableTemperature);
        }

        public void Freeze()
        {
            enemyIceBlockController.EnableFreeze();
        }

        protected void Move(Vector2 direction)
        {
            Mover.Move(direction);
        }

        private void OnSenseCollision(IHurtableTemperature sensedHurtableTemperature)
        {
            sensedHurtableTemperature.HurtTemperatureFromEnemy(enemyType);
        }
    }
}