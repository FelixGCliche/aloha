using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class ColliderStimuli2D : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Awake()
        {
            Debug.Assert(!GetComponent<Collider2D>().isTrigger, "ColliderStimuli2D need a collider, not a trigger.");
        }
#endif

        private void OnDestroy()
        {
            NotifyDestroyed();
        }

        public event StimuliEventHandler OnDestroyed;

        private void NotifyDestroyed()
        {
            if (OnDestroyed != null)
                OnDestroyed((transform.parent != null ? transform.parent : transform).gameObject);
        }
    }
}