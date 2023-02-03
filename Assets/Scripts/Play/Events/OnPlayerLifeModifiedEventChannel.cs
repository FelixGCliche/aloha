using Harmony;
using UnityEngine;

namespace Game
{
    // LouisRD
    [Findable(Tags.GameController)]
    public class OnPlayerLifeModifiedEventChannel : MonoBehaviour
    {
        public event OnPlayerLifeModifiedEvent OnPlayerLifeModified;

        public void Publish(Player player)
        {
            OnPlayerLifeModified?.Invoke(player);
        }
    }
    public delegate void OnPlayerLifeModifiedEvent(Player player);
}