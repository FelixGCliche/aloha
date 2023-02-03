using UnityEngine;

namespace Game
{
    // Author: David D, Félix B
    public interface ITemperature
    {
        TemperatureStats TemperatureStats { get; }
    }

    public class TemperatureStats : MonoBehaviour
    {
        [Header("TemperatureStats")]
        [Tooltip("Vitesse à laquelle la température varie")]
        [SerializeField] [Range(0f, 0.5f)] private float tempSensibility = 0.2f;
        [SerializeField] [Range(0f, 1f)] private float minTemperature = 0f;
        [SerializeField] [Range(0f, 1f)] private float maxTemperature = 1f;
        [SerializeField] [Range(0f, 1f)] private float temperature = 0.5f;
        [SerializeField] [Range(0f, 1f)]
        [Tooltip("Si la température de l'objet est plus basse que cette valeur, il va être considéré froid, sinon s'il est plus grand ou égal, il est considéré chaud.")]
        private float stateChangeThreshold = 0.5f;

        // Only 3 sources that can influence temperature changes: the object itself, the player and temperature zone
        private float objectInfluence; // Itself
        private float zoneInfluence; // Zone
        private float auraInfluence; // Player

        public float MinTemperature => minTemperature;
        public float MaxTemperature => maxTemperature;
        public float ZoneInfluence { set => zoneInfluence = value; }
        public float ObjectInfluence { set => objectInfluence = value; }
        public float AuraInfluence { set => auraInfluence = value; }
        private float TempVariation => (objectInfluence + zoneInfluence + auraInfluence) / 3;// Which state the temperature will gravitate to
        private float TempVariationStrength =>TempVariation * tempSensibility; // Variation strength
        public float Temperature => temperature;
        public bool HasReachMinTemperature => temperature <= minTemperature;
        public bool HasReachMaxTemperature => temperature >= maxTemperature;
        public TempState TemperatureState => temperature < stateChangeThreshold ? TempState.Frozen : TempState.Hot;
        public float TemperatureTresholdsRange => (maxTemperature - minTemperature);
        public event OnColdThresholdEvent OnColdThreshold;
        public event OnHeatThresholdEvent OnHeatThreshold;
        public event OnChangeTempStateEvent OnChangeTempState;
        
        private void Update()
        {
            // Called before or else it wasn't called for objects
            if (HasReachMaxTemperature)
                OnHeatThreshold?.Invoke(this);
            if (HasReachMinTemperature)
                OnColdThreshold?.Invoke(this);

            var lastState = TemperatureState;
            
            var tempDelta = TempVariationStrength * Time.deltaTime;

            temperature = Mathf.Clamp(temperature + tempDelta, minTemperature, maxTemperature);

            if (lastState != TemperatureState)
                OnChangeTempState?.Invoke(this);
        }

        public void SetTemperatureToMin()
        {
            temperature = minTemperature;
        }

        public void SetTemperatureToMax()
        {
            temperature = maxTemperature;
        }

        public void ResetZone()
        {
            zoneInfluence = 0f;
        }
    }

    public delegate void OnChangeTempStateEvent(TemperatureStats temp);

    public delegate void OnColdThresholdEvent(TemperatureStats temp);

    public delegate void OnHeatThresholdEvent(TemperatureStats temp);
}