using Harmony;
using UnityEngine;

namespace Game
{
    // LouisRD
    [Findable(Tags.GameController)]
    public class OnTemperatureModifiedEventChannel : MonoBehaviour
    {
        public event OnTemperatureModifiedEvent OnTemperatureModified;

        public void Publish(TemperatureStats playerTemperatureStats)
        {
            OnTemperatureModified?.Invoke(playerTemperatureStats);
        }
    }
    public delegate void OnTemperatureModifiedEvent(TemperatureStats playerTemperatureStats);
}