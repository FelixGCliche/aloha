using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Félix GC
    [Findable(Tags.GameController)]
    public class OnPlayerDeathEventChannel : MonoBehaviour
    {
        public event OnPlayerDeathEvent OnPlayerDeath;

        public void Publish()
        {
            OnPlayerDeath?.Invoke();
        }
    }
    public delegate void OnPlayerDeathEvent();
}