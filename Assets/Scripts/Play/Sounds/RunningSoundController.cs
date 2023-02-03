using UnityEngine;

namespace Game
{
    // David D
    public class RunningSoundController : MonoBehaviour
    {
        [SerializeField] private AudioClip runningSound;
        
        private AudioSource audioSource;
        private Player player;
        private Rigidbody2D playerRigidBody;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            player = GetComponentInParent<Player>();
            playerRigidBody = player.GetComponent<Rigidbody2D>();
            audioSource.clip = runningSound;
            audioSource.loop = true;
        }

        private void Update()
        {
            if(playerRigidBody.velocity.x == 0 || !player.IsGrounded)
            {
                audioSource.Stop();
            }
            else if (playerRigidBody.velocity.x != 0 && !audioSource.isPlaying && player.IsGrounded)
            {
                audioSource.Play();
            }
        }
    }
}