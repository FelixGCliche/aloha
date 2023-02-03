using System.Collections.Generic;
using System.Linq;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Félix GC
    public class FallingEnemy : Enemy
    {
        [SerializeField] private float horizontalDetectionSize = 5.0f;
        [SerializeField] private float zBoundsValue = 100.0f;

        private new Rigidbody2D rigidbody;
        private List<RaycastHit2D> groundHits;
        private ContactFilter2D groundTerrainFilter2D;
        private float distanceFromGround;
        private Bounds playerDetectionBounds;
        private Player player;

        private new void Awake()
        {
            base.Awake();

            rigidbody = GetComponent<Rigidbody2D>();

            player = Finder.Player;

            groundHits = new List<RaycastHit2D>();
            rigidbody.isKinematic = true;

            groundTerrainFilter2D.useLayerMask = true;
            groundTerrainFilter2D.layerMask = ~Layers.Terrain;
            groundHits = new List<RaycastHit2D>();

            SetDistanceFromGround();
            playerDetectionBounds = SetPlayerDetectionBounds();
        }

        private new void Update()
        {
            base.Update();

            if (PlayerSensed()) rigidbody.isKinematic = false;
        }

        private void SetDistanceFromGround()
        {
            var offSetPosition = new Vector2(transform.position.x, transform.position.y - characterHeight);
            Physics2D.Raycast(offSetPosition, Vector2.down, groundTerrainFilter2D, groundHits);

            distanceFromGround = groundHits.First().distance;
        }

        private Bounds SetPlayerDetectionBounds()
        {
            var boundCenter = new Vector2(transform.position.x, transform.position.y - distanceFromGround/2);
            return new Bounds(boundCenter, new Vector3(horizontalDetectionSize, distanceFromGround, zBoundsValue));
        }

        private bool PlayerSensed()
        {
            return playerDetectionBounds.Contains(player.Position);
        }
    }
}