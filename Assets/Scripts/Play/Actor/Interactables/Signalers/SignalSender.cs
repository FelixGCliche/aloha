using UnityEngine;

namespace Game
{
    // Author: David D
    public class SignalSender : MonoBehaviour
    {
        [Tooltip("Inverse le signal envoyé")]
        [SerializeField] private bool inverted;

        public bool RawActivated { get; private set; }
        public bool IsActivated
        {
            get
            {
                if (inverted)
                    return !RawActivated;
                return RawActivated;
            }
            set => RawActivated = value;
        }

        public void ToggleActivation()
        {
            RawActivated = !RawActivated;
        }
    }
}