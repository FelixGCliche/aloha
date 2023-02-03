using UnityEngine;

namespace Game
{
    // Author : Félix B
    public enum PlayerMotionState
    {
        Idle,
        Running,
        Jumping,
        Falling,
        Dashing,
        Dying
    }
    public class PlayerMotionAnimator : MonoBehaviour
    {
        public const float COLD_COLOR_BLENDING_VALUE = 0; 
        public const float HOT_COLOR_BLENDING_VALUE = 1; 
            
        [SerializeField] private PlayerMotionState motionState = PlayerMotionState.Idle;
        [SerializeField]  private string colorBlendParameterName = "ColorBlend";
        
        [Header("Les nom des paramètres à déclancher pour chaque états")]
        [SerializeField]  private string idleBoolName = "IsIdle";
        [SerializeField]  private string runningBoolName = "IsRunning";
        [SerializeField]  private string fallingBoolName = "IsFalling";
        [SerializeField]  private string jumpingBoolName = "IsJumping";
        [SerializeField]  private string dashingTriggerName = "StartDash";
        [SerializeField]  private string dyingTriggerName = "StartDie";

        private Animator animator;

        public PlayerMotionState MotionState
        {
            set
            {
                if(motionState!=value)
                    SetMotionState(value);
                motionState = value;
            }
        }
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void SetMotionState(PlayerMotionState value)
        {
            SetBoolParametersToFalse();
            
            switch (value)
            {
                case PlayerMotionState.Running:
                    animator.SetBool(runningBoolName,true);
                    break;
                case PlayerMotionState.Falling:
                    animator.SetBool(fallingBoolName,true);
                    break;
                case PlayerMotionState.Jumping:
                    animator.SetBool(jumpingBoolName,true);
                    break;
                case PlayerMotionState.Dashing:
                    animator.SetTrigger(dashingTriggerName);
                    break;
                case PlayerMotionState.Dying:
                    animator.SetTrigger(dyingTriggerName);
                    break;
            }
        }

        private void SetBoolParametersToFalse()
        {
            animator.SetBool(runningBoolName,false);
            animator.SetBool(idleBoolName,false);
            animator.SetBool(fallingBoolName,false);
            animator.SetBool(jumpingBoolName,false);
        }

        public void SwitchColor(TempState state)
        {
            switch (state)
            {
                case TempState.Frozen:
                    animator.SetFloat(colorBlendParameterName,COLD_COLOR_BLENDING_VALUE);
                    break;
                case TempState.Hot:
                    animator.SetFloat(colorBlendParameterName,HOT_COLOR_BLENDING_VALUE);
                    break;
            }
        }
    }
}