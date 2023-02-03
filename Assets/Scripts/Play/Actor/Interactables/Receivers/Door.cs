using UnityEngine;

namespace Game
{
    // Author: David D
    public class Door : MonoBehaviour
    {
        [SerializeField] private Sprite openSprite;
        [SerializeField] private Sprite closedSprite;
        
        private BoxCollider2D boxCollider2D;
        private SignalReceiver signalReceiver;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();
            signalReceiver = GetComponent<SignalReceiver>();
        }

        private void OnEnable()
        {
            signalReceiver.OnChange += ChangeState;
        }

        private void OnDisable()
        {
            signalReceiver.OnChange -= ChangeState;
        }

        private void ChangeState(SignalReceiver receiver)
        {
            if (receiver.IsActivated)
            {
                boxCollider2D.enabled = false;
                spriteRenderer.sprite = openSprite;
            }
            else
            {
                boxCollider2D.enabled = true;
                spriteRenderer.sprite = closedSprite;
            }
        }
    }
}