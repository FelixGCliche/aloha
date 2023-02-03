using System.Collections;
using UnityEngine;

namespace Game
{
    // Mover : LouisRD 
    // Dash : Félix B
    public class Mover : MonoBehaviour
    {
        [Header("Mover")]
        [SerializeField] [Min(0.0f)] private float speed = 12f;
        [SerializeField] [Min(0.0f)] private float iceSpeed = 15f;
        [SerializeField] [Min(0.0f)] private float jumpHeight = 28f;
        [SerializeField] [Min(0.0f)] private float doubleJumpHeight = 20f;
        [SerializeField] [Tooltip("Plus le controle est élevé, plus l'entité à de controle dans les airs")]
        [Range(0f, 12f)] private float airControl = 5f;
        [SerializeField] private bool touchIce = false;
        [SerializeField] private bool slideToggle = false;
        [SerializeField] private bool isGrounded = true;

        [Header("Dash")] 
        [SerializeField] [Min(0.0f)] private float dashSpeed = 0f;
        [SerializeField] [Min(0.0f)] private float dashDurationTime = 0.2f;
        [SerializeField] private bool isDashing = false;
        [SerializeField] private float dashCooldownDuration = 2f;
        
        [Header("Audio")]
        [SerializeField] private AudioClip dashSound;

        private Rigidbody2D rigidBody;
        private bool dashIsOnCooldown = false;
        private AudioSource audioSource;

        public float DashCooldownDuration => dashCooldownDuration;
        public bool TouchIce
        {
            set => touchIce = value;
        }
        public bool SlideToggle
        {
            set => slideToggle = value;
        }
        public bool IsGrounded
        {
            get => isGrounded;
            set => isGrounded = value;
        }
        public bool IsDashing => isDashing;

        private void Awake()
        {
            rigidBody = GetComponentInParent<Rigidbody2D>();
            audioSource = GetComponentInParent<AudioSource>();
        }
        
        //Louis
        public void Move(Vector2 direction, bool isGrappling = false)
        {
            if (isDashing||isGrappling) return;
            
            if (direction.x > 0 || direction.x < 0)
            {
                if (isGrounded && touchIce && slideToggle)
                    Accelerate(direction);
                else if (isGrounded)
                    rigidBody.velocity = new Vector2(speed * direction.x, rigidBody.velocity.y);
                else
                {
                    AirMove(direction);
                }
            }
            else if (direction.x == 0 && !touchIce)
                Stop();
            else
            {
                if (!slideToggle)
                    Stop();
                else
                {
                    Decelerate();
                }
            }
        }

        //Louis
        private void Stop()
        {
            if (isGrounded)
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }

        //Louis
        private void Accelerate(Vector2 direction)
        {
            if (!isGrounded) return;

            rigidBody.velocity +=
                new Vector2(Mathf.Clamp(iceSpeed * direction.x * Time.deltaTime, -iceSpeed, iceSpeed), 0);
        }

        //Louis
        private void Decelerate()
        {
            if (!isGrounded) return;
            
            if (rigidBody.velocity.x < 0)
            {
                rigidBody.velocity -= new Vector2(-speed*Time.deltaTime,0);
                rigidBody.velocity = new Vector2(Mathf.Clamp(rigidBody.velocity.x, -speed, 0), rigidBody.velocity.y);
            }
            else if (rigidBody.velocity.x > 0)
            {
                rigidBody.velocity -= new Vector2(speed * Time.deltaTime, 0);
                rigidBody.velocity = new Vector2(Mathf.Clamp(rigidBody.velocity.x, 0, speed), rigidBody.velocity.y);
            }
        }

        //Louis
        private void AirMove(Vector2 direction)
        {
            rigidBody.velocity += new Vector2(speed * direction.x * airControl * Time.deltaTime, 0);
            rigidBody.velocity = !slideToggle ? new Vector2(Mathf.Clamp(rigidBody.velocity.x, -speed, speed), rigidBody.velocity.y) : new Vector2(Mathf.Clamp(rigidBody.velocity.x, -iceSpeed, iceSpeed), rigidBody.velocity.y);
        }

        //Louis
        public void Jump()
        {
            if (!isDashing)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x,jumpHeight);
        }

        //Louis
        public void DoubleJump()
        {
            if (!isDashing)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x,doubleJumpHeight);
        }

        //Felix.B
        public bool Dash(Vector2 direction)
        {
            if (!isDashing && !dashIsOnCooldown)
            {
                StartCoroutine(DashRoutine(direction));
                return true;
            }
            return false;
        }

        //Felix.B
        private IEnumerator DashRoutine(Vector2 direction)
        {
            audioSource.PlayOneShot(dashSound);
            isDashing = true;
            rigidBody.velocity = direction * dashSpeed;

            yield return new WaitForSeconds(dashDurationTime);
            
            rigidBody.velocity = Vector2.zero;
            isDashing = false;
            dashIsOnCooldown = true;
            yield return  new WaitForSeconds(dashCooldownDuration);
            dashIsOnCooldown = false;
        }
    }
}