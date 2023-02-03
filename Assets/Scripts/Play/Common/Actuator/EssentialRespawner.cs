using UnityEngine;

namespace Game
{
    // Author : Félix B
    public class EssentialRespawner : MonoBehaviour
    {
        [SerializeField] private bool waterVunerable = false;
        [SerializeField] private bool lavaVunerable = false;
        
        private IEssential essential;
        
        private void OnEnable()
        {
            essential = GetComponentInParent<IEssential>();
        }

        public void Respawn(DamageType type)
        {
            if ((type == DamageType.Water && waterVunerable)||(type == DamageType.Lava && lavaVunerable))
                essential.ResetPosition();
        }
    }
}