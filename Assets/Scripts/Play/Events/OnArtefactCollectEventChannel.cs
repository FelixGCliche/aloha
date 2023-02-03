using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David Dorion
    [Findable(Tags.GameController)]
    public class OnArtefactCollectEventChannel : MonoBehaviour
    {
        public event ArtefactCollectEvent OnArtefactCollect;

        public void Publish(ArtefactType artefact)
        {
            if (OnArtefactCollect != null) OnArtefactCollect(artefact);
        }
    }
    public delegate void ArtefactCollectEvent(ArtefactType sender);
}