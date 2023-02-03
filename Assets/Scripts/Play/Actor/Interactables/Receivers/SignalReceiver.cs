using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    // Author: David D
    public class SignalReceiver : MonoBehaviour
    {
        [SerializeField] protected List<SignalSender> signalSenders;
        [SerializeField][Tooltip("Inverse le signal recu")] protected bool invert;

        public bool IsActivated { get; private set; }
        
        public event OnChangeActivation OnChange;

        private void Update()
        {
            var recievedSignal = signalSenders.Any(signalSender => signalSender.IsActivated);

            if (invert)
                recievedSignal = !recievedSignal;

            if (recievedSignal == IsActivated) return;

            IsActivated = recievedSignal;
            
            Notify();
        }

        private void Notify()
        {
            OnChange?.Invoke(this);
        }
    }
    public delegate void OnChangeActivation(SignalReceiver signalReceiver);
}