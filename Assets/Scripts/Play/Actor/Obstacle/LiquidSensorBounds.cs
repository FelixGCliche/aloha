using UnityEngine;

namespace Game
{
    // Louis RD
    public class LiquidSensorBounds : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private BoxCollider2D boxCollider2D;
        private void Awake()
        {
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();

            var vector2 = new Vector2(spriteRenderer.size.x, spriteRenderer.size.y - (2 * spriteRenderer.size.y/3));
            
            if (boxCollider2D != null)
                boxCollider2D.size = vector2;
        }
    }
}