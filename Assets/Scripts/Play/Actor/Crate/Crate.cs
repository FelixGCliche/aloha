using UnityEngine;

namespace Game
{
    // Author: Félix B
    public class Crate : MonoBehaviour, IEntity, IEssential
    {
        public Vector3 Position { get; }

        private Rigidbody2D body;

        private Vector2 startPosition;
        
        private void Start()
        {
            startPosition = transform.position;
        }

        public void ResetPosition()
        {
            transform.position = startPosition;
        }
    }
}