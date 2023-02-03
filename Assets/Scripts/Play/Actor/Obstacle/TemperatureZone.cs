using UnityEngine;

namespace Game
{
    // Author: Félix B, David D 
    public class TemperatureZone : MonoBehaviour
    {
        [SerializeField] [Range(-1f, 1f)] private float temperatureModificationFactor;

        private ISensor<ITemperature> temperatureTriggerSensor;

        private void Awake()
        {
            temperatureTriggerSensor = GetComponentInChildren<StickyTriggerSensor2D>().For<ITemperature>();
        }

        private void Update()
        {
            // Ne fonctionne pas avec OnSensed
            foreach (var sensedObject in temperatureTriggerSensor.SensedObjects)
                sensedObject.TemperatureStats.ZoneInfluence = temperatureModificationFactor;
        }

        private void OnEnable()
        {
            temperatureTriggerSensor.OnUnsensedObject += OnITemperatureUnsensed;
        }

        private void OnDisable()
        {
            temperatureTriggerSensor.OnUnsensedObject -= OnITemperatureUnsensed;
        }

        private void OnITemperatureUnsensed(ITemperature temperature)
        {
            temperature.TemperatureStats.ResetZone();
        }
    }
}