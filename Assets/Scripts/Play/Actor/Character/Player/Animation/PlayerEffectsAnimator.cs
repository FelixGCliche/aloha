using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Félix B
    public class PlayerEffectsAnimator : MonoBehaviour
    {
        private AnimationTrigger jumpTrigger;
        private AnimationTrigger dashTrigger;
        private AnimationTrigger freezeTrigger;

        private void OnEnable()
        {
            var animationTriggers = GetComponentsInChildren<AnimationTrigger>();
            if (animationTriggers != null)
            {
                jumpTrigger = animationTriggers.WithName(GameObjects.JumpEffectAnimator);
                dashTrigger = animationTriggers.WithName(GameObjects.DashEffectAnimator);
                freezeTrigger = animationTriggers.WithName(GameObjects.FreezeEffectAnimator);
            }
        }

        public void FireDashEffect()
        {
            dashTrigger.Play();
        }
        
        public void FireJumpEffect()
        {
            jumpTrigger.Play();
        }
        
        public void FireFreezeEffect()
        {
            freezeTrigger.Play();
        }
    }
}