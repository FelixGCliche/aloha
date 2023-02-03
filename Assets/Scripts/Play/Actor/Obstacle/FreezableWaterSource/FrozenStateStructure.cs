using UnityEngine;

namespace Game
{
    // Author : Félix B
    public class FrozenStateStructure : MonoBehaviour
    {
        private SpriteRenderer sprite;
        private new BoxCollider2D collider;

        private void Awake()
        {
            sprite = GetComponentInParent<SpriteRenderer>();
            collider = GetComponent<BoxCollider2D>();
        }

        private void OnEnable()
        {
            collider.size = sprite.size;
        }
    }
}