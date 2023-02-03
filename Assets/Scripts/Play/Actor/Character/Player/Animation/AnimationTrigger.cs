using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Félix B
    public class AnimationTrigger : MonoBehaviour
    {
        [SerializeField] [Tooltip("Nom du Trigger de L'animation")] private string animationTriggerName = "Default";
        [SerializeField] private bool shouldLeaveACopy = false;
        [SerializeField] private string baseLayerName = "Base Layer";
        
        private Animator animator;
        private Player player;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            player = Finder.Player;
        }

        public void Play()
        {
            if (shouldLeaveACopy)
            {
                GameObject clone = Instantiate(gameObject);
                // Set la position dans le monde en fonction du joueur.
                clone.transform.position += player.transform.position;
                // Lancer L'animation (L'objet s'auto détruit)
                AnimationTrigger cloneTrigger = clone.GetComponent<AnimationTrigger>();
                
                cloneTrigger.PlayAndDestroyRoutine();
            }
            else
            {
                animator.SetTrigger(animationTriggerName);
            }
        }

        private void PlayAndDestroyRoutine()
        {
             IEnumerator Routine()
            {
                animator.SetTrigger(animationTriggerName);
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex(baseLayerName)).length);
                Destroy(gameObject);
            }

             StartCoroutine(Routine());
        }
    }
}