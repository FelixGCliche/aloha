namespace Game
{
    // Author : Félix GC
    public class JumpingEnemy : Enemy
    {
        private new void Update()
        {
            base.Update();

            if (IsGrounded) Mover.Jump();
        }
    }
}