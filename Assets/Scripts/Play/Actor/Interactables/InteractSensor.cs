using System.Linq;
using Harmony;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    // Author: David D
    public class InteractSensor : MonoBehaviour
    {
        private InputAction interactAction;
        private ISensor<Player> playerSensor;

        public event OnInteractEvent OnInteract;

        protected void Awake()
        {
            interactAction = Finder.Inputs.Actions.Game.Interact;
            playerSensor = GetComponent<TriggerSensor2D>().For<Player>();
        }
        private void Update()
        {
            if (playerSensor.SensedObjects.Any() && interactAction.triggered)
                OnInteract?.Invoke(this);
        }
    }

    public delegate void OnInteractEvent(InteractSensor sender);
}