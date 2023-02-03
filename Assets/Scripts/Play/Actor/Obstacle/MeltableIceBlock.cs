using UnityEngine;

namespace Game
{
    // Author : Félix B
    public class MeltableIceBlock : ObjectTemperature, IEntity
    {
        private SpriteRenderer sprite;
        
        protected override Renderer Sprite => sprite;
        public Vector3 Position => transform.position;
       
        protected override void Awake()
        {
            base.Awake();

            sprite = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnEnable()
        {
            temperatureStats.OnChangeTempState += ChangeStates;
        }

        private void OnDisable()
        {
            temperatureStats.OnChangeTempState -= ChangeStates;
        }

        private void ChangeStates(TemperatureStats temperatureStats)
        {
            if (temperatureStats.TemperatureState == TempState.Hot)
                Destroy(gameObject);
        }
    }
}