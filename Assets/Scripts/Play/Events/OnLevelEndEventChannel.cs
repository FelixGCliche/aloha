using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Félix GC
    [Findable(Tags.GameController)]
    public class OnLevelEndEventChannel : MonoBehaviour
    {
        public event OnLevelEndEvent OnLevelEnd;

        public void Publish(SceneName scene)
        {
            OnLevelEnd?.Invoke(scene);
        }
    }
    public delegate void OnLevelEndEvent(SceneName scene);
}