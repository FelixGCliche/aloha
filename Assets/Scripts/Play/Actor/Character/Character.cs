using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Félix B
    public enum Direction
    {
        Left,
        Rigth
    }
    
    public abstract class Character : MonoBehaviour, IEntity
    {
        [SerializeField] private Direction initalDirection = Direction.Rigth;
        [SerializeField] [Min(0.1f)] protected float distanceToNotBeInCollider = 0.1f;
        [SerializeField] [Min(0.1f)] protected float rayCastDistance = 0.1f;
        
        protected float characterHeight;
        private Vector2 facingDirection;
        private List<RaycastHit2D> groundHits;
        private ContactFilter2D groundTerrainFilter;
        protected ISensor<Terrain> terrainSensor;
        private new CapsuleCollider2D collider2D;

        protected Mover Mover { get; private set; }
        protected VitalStats Vitals { get; private set; }
        protected ColliderSensor2D ColliderSensor { get; private set; }
        public Vector3 Position => transform.position;
        public bool IsGrounded
        {
            get
            {
                var offSetPosition = new Vector2(Position.x, Position.y - characterHeight);
            
                Physics2D.Raycast(offSetPosition, Vector2.down, groundTerrainFilter, groundHits, rayCastDistance);

                return groundHits.Count > 0;
            }
        }
        protected Vector2 FacingDirection
        {
            get
            {
                if(!Mathf.Approximately(facingDirection.x, 0) && facingDirection.x < 0)
                    return Vector2.left;
                if(!Mathf.Approximately(facingDirection.x, 0) && facingDirection.x > 0)
                    return Vector2.right;
                return Vector2.zero;
            }
            set
            {
                if (value == Vector2.left || value == Vector2.right)
                    facingDirection = value;
            } 
        }

        protected void Awake()
        {
            collider2D = GetComponent<CapsuleCollider2D>();
            characterHeight = collider2D.bounds.extents.y + distanceToNotBeInCollider;
            
            Mover = GetComponentInChildren<Mover>();
            Vitals = GetComponentInChildren<VitalStats>();

            ColliderSensor = GetComponent<ColliderSensor2D>();
            terrainSensor = ColliderSensor.For<Terrain>();
            groundTerrainFilter.useLayerMask = true;
            groundTerrainFilter.layerMask = ~Layers.Terrain;
            groundHits = new List<RaycastHit2D>();
        }

        protected void OnEnable()
        {
            switch (initalDirection)
            {
                case Direction.Left:
                    facingDirection = Vector2.left;
                    break;
                case Direction.Rigth:
                    facingDirection = Vector2.right;
                    break;
            }
        }

        public void ResetVitals()
        {
            Vitals.ResetVitals();
        }
    }
}