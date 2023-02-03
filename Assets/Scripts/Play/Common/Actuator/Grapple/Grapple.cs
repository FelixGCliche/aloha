using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Félix GC
    [RequireComponent(typeof(TriggerSensor2D))]
    public class Grapple : MonoBehaviour
    {
        [SerializeField] private float grappleSpeed = 100.0f;
        [SerializeField] [Min(0.01f)] private float grappleTolerance = 0.5f;

        private GrapplePoint grappleTarget;
        private bool isGrappling;
        private Vector2 grappleDirection;
        private float grappleDistance;
        private ISensor<GrapplePoint> grappleSensor;
        private List<RaycastHit2D> groundHits;
        private ContactFilter2D groundTerrainFilter;

        private Transform ParentTransform => transform.parent.transform;
        public bool IsGrappling => isGrappling;

        private void Awake()
        {
            grappleSensor = GetComponent<TriggerSensor2D>().For<GrapplePoint>();
            isGrappling = false;

            groundTerrainFilter.useLayerMask = true;
            groundTerrainFilter.layerMask = 1 << Layers.Terrain;
            groundHits = new List<RaycastHit2D>();
        }

        private void OnEnable()
        {
            grappleSensor.OnSensedObject += OnGrapplePointSensed;
            grappleSensor.OnUnsensedObject += OnGrapplePointUnsensed;
        }

        private void OnDisable()
        {
            grappleSensor.OnSensedObject -= OnGrapplePointSensed;
            grappleSensor.OnUnsensedObject -= OnGrapplePointUnsensed;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (isGrappling) Gizmos.color = Color.green;
            else Gizmos.color = Color.yellow;

            if (grappleTarget != null)
            {
                grappleDirection = ParentTransform.position.DirectionTo(grappleTarget.Position);
                grappleDistance = grappleDirection.magnitude;
                if (IsGrapplePathObstructed(grappleDirection, grappleDistance))
                    Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, grappleDirection);
            }
        }
#endif

        private void Update()
        {
            GetNearestGrapplePoint();
            
            if (grappleTarget != null && isGrappling)
            {
                grappleDirection = ParentTransform.position.DirectionTo(grappleTarget.Position);
                grappleDistance = grappleDirection.magnitude;

                if (!grappleTarget.Position.AreClose(ParentTransform.position, grappleTolerance))
                    ParentTransform.Translate(grappleDirection.normalized * (grappleSpeed * Time.deltaTime));
                else
                    isGrappling = false;
            }
        }

        public void GrappleTo()
        {
            if (grappleTarget != null)
            {
                grappleDirection = ParentTransform.position.DirectionTo(grappleTarget.Position);
                grappleDistance = grappleDirection.magnitude;

                if (!grappleTarget.Position.AreClose(ParentTransform.position, grappleTolerance) &&
                    !IsGrapplePathObstructed(grappleDirection, grappleDistance))
                    isGrappling = true;
            }
        }

        private void OnGrapplePointSensed(GrapplePoint grapplePoint)
        {
            if (!isGrappling)
            {
                grappleTarget = grapplePoint;
                grappleTarget.Select();
            }
        }

        private void OnGrapplePointUnsensed(GrapplePoint grapplePoint)
        {
            if (grappleSensor.SensedObjects.Count == 0)
                grappleTarget = null;

            grapplePoint.Unselect();
        }

        private bool IsGrapplePathObstructed(Vector2 direction, float distance)
        {
            Physics2D.Raycast(transform.position, direction, groundTerrainFilter, groundHits, distance);

            return groundHits.Count > 0;
        }

        private void GetNearestGrapplePoint()
        {
            if (grappleSensor.SensedObjects.Count > 1)
            {
                foreach (GrapplePoint grapplePoint in grappleSensor.SensedObjects)
                {
                    if (GetGrappleDistance(grappleTarget) > GetGrappleDistance(grapplePoint))
                        grappleTarget = grapplePoint;
                }
                grappleTarget.Select();
            }
        }

        private float GetGrappleDistance(GrapplePoint grapplePoint)
        {
            return ParentTransform.position.DistanceTo(grapplePoint.Position);
        }
    }
}