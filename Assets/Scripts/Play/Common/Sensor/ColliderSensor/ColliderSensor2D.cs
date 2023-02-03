using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class ColliderSensor2D : MonoBehaviour, ISensor<GameObject>
    {
        private readonly List<GameObject> sensedObjects;
        private new Collider2D collider;
        private Transform parentTransform;

        public ColliderSensor2D()
        {
            sensedObjects = new List<GameObject>();
            DirtyFlag = ulong.MinValue;
        }

        public ulong DirtyFlag { get; private set; }

        private void Awake()
        {
            parentTransform = transform.parent ?? transform;
            collider = GetComponent<Collider2D>();
#if UNITY_EDITOR
            Debug.Assert(!collider.isTrigger, "ColliderSensor2D need a collider, not a trigger.");
#endif
        }

        private void OnEnable()
        {
            collider.enabled = true;
        }

        private void OnDisable()
        {
            collider.enabled = false;
            ClearSensedObjects();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var otherParentTransform = other.transform.parent ?? other.transform;
            if (!IsSelf(otherParentTransform))
            {
                var stimuli = other.collider.GetComponent<ColliderStimuli2D>();
                if (stimuli != null)
                {
                    stimuli.OnDestroyed += RemoveSensedObject;
                    AddSensedObject(otherParentTransform.gameObject);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            var otherParentTransform = other.transform.parent ?? other.transform;
            if (!IsSelf(otherParentTransform))
            {
                var stimuli = other.collider.GetComponent<ColliderStimuli2D>();
                if (stimuli != null)
                {
                    stimuli.OnDestroyed -= RemoveSensedObject;
                    RemoveSensedObject(otherParentTransform.gameObject);
                }
            }
        }

        public event SensorEventHandler<GameObject> OnSensedObject;
        public event SensorEventHandler<GameObject> OnUnsensedObject;

        public IReadOnlyList<GameObject> SensedObjects => sensedObjects;

        public ISensor<T> For<T>()
        {
            return new ColliderSensor2D<T>(this);
        }

        public ISensor<T> ForNothing<T>()
        {
            return new EmptyColliderSensor2D<T>();
        }

        private void AddSensedObject(GameObject otherObject)
        {
            if (!sensedObjects.Contains(otherObject))
            {
                sensedObjects.Add(otherObject);
                DirtyFlag++;
                NotifySensedObject(otherObject);
            }
        }

        private void RemoveSensedObject(GameObject otherObject)
        {
            if (sensedObjects.Contains(otherObject))
            {
                sensedObjects.Remove(otherObject);
                DirtyFlag++;
                NotifyUnsensedObject(otherObject);
            }
        }

        private void ClearSensedObjects()
        {
            sensedObjects.Clear();
            DirtyFlag++;
        }

        private bool IsSelf(Transform otherParentTransform)
        {
            return parentTransform == otherParentTransform;
        }

        private void NotifySensedObject(GameObject otherObject)
        {
            if (OnSensedObject != null) OnSensedObject(otherObject);
        }

        private void NotifyUnsensedObject(GameObject otherObject)
        {
            if (OnUnsensedObject != null) OnUnsensedObject(otherObject);
        }
    }

    [SuppressMessage("ReSharper", "DelegateSubtraction")]
    public sealed class ColliderSensor2D<T> : ISensor<T>
    {
        private readonly List<T> sensedObjects;
        private readonly ColliderSensor2D sensor;
        private ulong dirtyFlag;
        private SensorEventHandler<T> onSensedObject;
        private SensorEventHandler<T> onUnsensedObject;

        public ColliderSensor2D(ColliderSensor2D sensor)
        {
            this.sensor = sensor;
            sensedObjects = new List<T>();
            dirtyFlag = sensor.DirtyFlag;

            UpdateSensor();
        }

        public IReadOnlyList<T> SensedObjects
        {
            get
            {
                if (IsDirty()) UpdateSensor();

                return sensedObjects;
            }
        }

        public event SensorEventHandler<T> OnSensedObject
        {
            add
            {
                if (onSensedObject == null || onSensedObject.GetInvocationList().Length == 0)
                    sensor.OnSensedObject += OnSensedObjectInternal;
                onSensedObject += value;
            }
            remove
            {
                if (onSensedObject != null && onSensedObject.GetInvocationList().Length == 1)
                    sensor.OnSensedObject -= OnSensedObjectInternal;
                onSensedObject -= value;
            }
        }

        public event SensorEventHandler<T> OnUnsensedObject
        {
            add
            {
                if (onUnsensedObject == null || onUnsensedObject.GetInvocationList().Length == 0)
                    sensor.OnUnsensedObject += OnUnsensedObjectInternal;
                onUnsensedObject += value;
            }
            remove
            {
                if (onUnsensedObject != null && onUnsensedObject.GetInvocationList().Length == 1)
                    sensor.OnUnsensedObject -= OnUnsensedObjectInternal;
                onUnsensedObject -= value;
            }
        }

        private bool IsDirty()
        {
            return sensor.DirtyFlag != dirtyFlag;
        }

        private void UpdateSensor()
        {
            sensedObjects.Clear();

            foreach (var otherObject in sensor.SensedObjects)
            {
                var otherComponent = otherObject.GetComponentInChildren<T>();
                if (otherComponent != null) sensedObjects.Add(otherComponent);
            }

            dirtyFlag = sensor.DirtyFlag;
        }

        private void OnSensedObjectInternal(GameObject otherObject)
        {
            var otherComponent = otherObject.GetComponentInChildren<T>();
            if (otherComponent != null && !sensedObjects.Contains(otherComponent))
            {
                sensedObjects.Add(otherComponent);
                NotifySensedObject(otherComponent);
            }

            dirtyFlag = sensor.DirtyFlag;
        }

        private void OnUnsensedObjectInternal(GameObject otherObject)
        {
            var otherComponent = otherObject.GetComponentInChildren<T>();
            if (otherComponent != null && sensedObjects.Contains(otherComponent))
            {
                sensedObjects.Remove(otherComponent);
                NotifyUnsensedObject(otherComponent);
            }

            dirtyFlag = sensor.DirtyFlag;
        }

        private void NotifySensedObject(T otherObject)
        {
            if (onSensedObject != null) onSensedObject(otherObject);
        }

        private void NotifyUnsensedObject(T otherObject)
        {
            if (onUnsensedObject != null) onUnsensedObject(otherObject);
        }
    }

    public sealed class EmptyColliderSensor2D<T> : ISensor<T>
    {
        private readonly List<T> sensedObjects;

        public EmptyColliderSensor2D()
        {
            sensedObjects = new List<T>();
        }

        public IReadOnlyList<T> SensedObjects => sensedObjects;

        public event SensorEventHandler<T> OnSensedObject;
        public event SensorEventHandler<T> OnUnsensedObject;
    }
}