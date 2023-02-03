using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David D, Félix B
    public class PlayerTemperatureAura : MonoBehaviour
    {
        [SerializeField] [Range(0f, 1f)] private float hotTemperatureModificationFactor = 0.75f;
        [SerializeField] [Range(-1f, 0f)] private float coldTemperatureModificationFactor = -0.75f;

        private ISensor<ITemperature> objectTriggerSensor;
        private Player player;
        private ParticleSystem coldParticleSystem;
        private ParticleSystem hotParticleSystem;

        private void Awake()
        {
            player = GetComponentInParent<Player>();
            objectTriggerSensor = GetComponentInChildren<TriggerSensor2D>().For<ITemperature>();
            var particalSystems = GetComponentsInChildren<ParticleSystem>();
            coldParticleSystem = particalSystems.WithName(GameObjects.ColdParticles);
            hotParticleSystem = particalSystems.WithName(GameObjects.HotParticles);
            coldParticleSystem.Stop();
            hotParticleSystem.Stop();
        }

        private void Start()
        {
            ChangeParticleSystem();
        }

        private void OnEnable()
        {
            objectTriggerSensor.OnSensedObject += OnITemperatureZoneEnter;
            objectTriggerSensor.OnUnsensedObject += OnITemperatureZoneExit;
        }

        private void OnDisable()
        {
            objectTriggerSensor.OnSensedObject -= OnITemperatureZoneEnter;
            objectTriggerSensor.OnUnsensedObject -= OnITemperatureZoneExit;
        }

        private void OnITemperatureZoneEnter(ITemperature temperature)
        {
            var activeState = player.PlayerTemperatureState;
            
            switch (activeState)
            {
                case TempState.Hot:
                    temperature.TemperatureStats.AuraInfluence = hotTemperatureModificationFactor;
                    break;
                case TempState.Frozen:
                    temperature.TemperatureStats.AuraInfluence = coldTemperatureModificationFactor;
                    break;
            }
        }

        private void OnITemperatureZoneExit(ITemperature temperature)
        {
            temperature.TemperatureStats.AuraInfluence = 0f;
        }

        public void OnPlayerStateChange()
        {
            ChangeParticleSystem();
            
            if (objectTriggerSensor.SensedObjects == null) return;

            foreach (var temp in objectTriggerSensor.SensedObjects)
                if (player.TemperatureStats != temp.TemperatureStats)
                    OnITemperatureZoneEnter(temp);
        }

        private void ChangeParticleSystem()
        {
            switch (player.PlayerTemperatureState)
            {
                case TempState.Frozen:
                    hotParticleSystem.Stop();
                    coldParticleSystem.Play();
                    break;
                case TempState.Hot:
                    coldParticleSystem.Stop();
                    hotParticleSystem.Play();
                    break;
            }
        }
    }
}