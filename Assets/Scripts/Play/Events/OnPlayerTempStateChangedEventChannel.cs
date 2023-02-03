using Harmony;
using UnityEngine;

namespace Game
{
    // LouisRD
    [Findable(Tags.GameController)]
    public class OnPlayerTempStateChangedEventChannel : MonoBehaviour
    {
        public event OnPlayerTempStateChangedEvent OnPlayerTempStateChanged;

        public void Publish(TempState tempState)
        {
            OnPlayerTempStateChanged?.Invoke(tempState);
        }

        public delegate void OnPlayerTempStateChangedEvent(TempState tempState);
    }
}