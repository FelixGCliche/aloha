using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David Dorion
    [Findable(Tags.GameController)]
    public class OnActionActivatedChannel : MonoBehaviour
    {
        public event ArtefactActivatedEvent OnArtefactActivated;

        public void Publish(ActionsWithCooldown action)
        {
            OnArtefactActivated?.Invoke(action);
        }
    }
    public delegate void ArtefactActivatedEvent(ActionsWithCooldown sender);
}