using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David D
    public class WanderingEnemy : Enemy , IEssential
    {
        [SerializeField] private float respawnDelay = 5f;
        
        private Vector2 lookDirection;
        private Vector2 enemySize;
        private ISensor<IEntity> entitySensor;
        private List<RaycastHit2D> groundHits;
        private List<RaycastHit2D> wallHits;
        private ContactFilter2D terrainFilter;
        private SpriteRenderer spriteRenderer;
        private Vector2 startPosition;

        private new void Awake()
        {
            base.Awake();

            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            lookDirection = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;
            spriteRenderer.flipX = lookDirection != Vector2.right;

            Bounds bounds = GetComponent<CapsuleCollider2D>().bounds;
            
            enemySize = new Vector2
            {
                x = bounds.extents.x + distanceToNotBeInCollider,
                y = bounds.extents.y + distanceToNotBeInCollider
            };
            
            entitySensor = ColliderSensor.For<IEntity>();
            
            terrainFilter.useLayerMask = true;
            terrainFilter.layerMask = ~Layers.Terrain;
            groundHits = new List<RaycastHit2D>();
            wallHits = new List<RaycastHit2D>();
            
            startPosition = transform.position;
        }

        private new void Update()
        {
            base.Update();
            
            if (!IsGrounded) return;

            var offSetPosition = new Vector2(transform.position.x + (lookDirection == Vector2.left ? -enemySize.x : enemySize.x), transform.position.y - enemySize.y);
            Physics2D.Raycast(offSetPosition, Vector2.down, terrainFilter, groundHits, rayCastDistance);
            
            var wallOffSetPosition = new Vector2(offSetPosition.x, transform.position.y);
            Physics2D.Raycast(wallOffSetPosition, lookDirection, terrainFilter, wallHits, rayCastDistance);

            if (groundHits.Count == 0)
            {
                ChangeDirection();
            }
            else if (groundHits.Count > 0 &&
                     groundHits.Any(groundHit => groundHit.transform.gameObject.GetComponent<Obstacle>() != null))
            {
                ChangeDirection();
            }
            else if (entitySensor.SensedObjects.Any(sensedEntity => sensedEntity != null))
            {
                ChangeDirection();
            }
            else if (wallHits.Count > 0)
            {
                var isPlayer = wallHits.Any(hit => hit.transform.gameObject.GetComponent<Player>() != null);
                if (!isPlayer)
                    ChangeDirection();
            }

            Move(lookDirection);
        }

        private void ChangeDirection()
        {
            lookDirection = -lookDirection;
            spriteRenderer.flipX = lookDirection != Vector2.right;
        }
        
        public void ResetPosition()
        {
            StartCoroutine(ResetPositionCoroutine());
        }

        private IEnumerator ResetPositionCoroutine()
        {
            spriteRenderer.enabled = false;
            EnemyIceBlockController.DisableFreeze();
            yield return new WaitForSeconds(respawnDelay);
            transform.position = startPosition;
            spriteRenderer.enabled = true;
        }
    }
}