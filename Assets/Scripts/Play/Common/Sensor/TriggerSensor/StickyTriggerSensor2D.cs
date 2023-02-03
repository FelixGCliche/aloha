using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Harmony;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Game
{
    // Author: David Dorion, Félix Bergeron
    public sealed class StickyTriggerSensor2D : MonoBehaviour, ISensor<GameObject>
    {
        private readonly List<GameObject> sensedObjects;
        private new Collider2D collider;
        private Transform parentTransform;

        public StickyTriggerSensor2D()
        {
            sensedObjects = new List<GameObject>();
            DirtyFlag = ulong.MinValue;
        }

        public ulong DirtyFlag { get; private set; }

        private void Awake()
        {
            parentTransform = transform.parent ?? transform;
            
            collider = GetComponentInParent<Collider2D>();
            
            SetSensorLayer();
        }
        

        private void OnEnable()
        {
            collider.enabled = true;
            collider.isTrigger = true;
        }

        private void OnDisable()
        {
            collider.enabled = false;
            collider.isTrigger = false;
            ClearSensedObjects();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var otherParentTransform = other.transform.parent ?? other.transform;
            if (!IsSelf(otherParentTransform))
            {
                var stimuli = other.GetComponent<TriggerStimuli2D>();
                if (stimuli != null)
                {
                    stimuli.OnDestroyed += RemoveSensedObject;
                    AddSensedObject(otherParentTransform.gameObject);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var otherParentTransform = other.transform.parent ?? other.transform;
            if (!IsSelf(otherParentTransform))
            {
                var stimuli = other.GetComponent<TriggerStimuli2D>();
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

        public ISensor<T> For<T>()
        {
            return new StickyTriggerSensor2D<T>(this);
        }

        public ISensor<T> ForNothing<T>()
        {
            return new EmptySensor<T>();
        }

        private void SetSensorLayer()
        {
            gameObject.layer = Layers.Sensor;
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
    public sealed class StickyTriggerSensor2D<T> : ISensor<T>
    {
        private readonly List<T> sensedObjects;
        private readonly StickyTriggerSensor2D triggerSensor;
        private ulong dirtyFlag;
        private SensorEventHandler<T> onSensedObject;
        private SensorEventHandler<T> onUnsensedObject;

        public StickyTriggerSensor2D(StickyTriggerSensor2D triggerSensor)
        {
            this.triggerSensor = triggerSensor;
            sensedObjects = new List<T>();
            dirtyFlag = triggerSensor.DirtyFlag;

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
                    triggerSensor.OnSensedObject += OnSensedObjectInternal;
                onSensedObject += value;
            }
            remove
            {
                if (onSensedObject != null && onSensedObject.GetInvocationList().Length == 1)
                    triggerSensor.OnSensedObject -= OnSensedObjectInternal;
                onSensedObject -= value;
            }
        }

        public event SensorEventHandler<T> OnUnsensedObject
        {
            add
            {
                if (onUnsensedObject == null || onUnsensedObject.GetInvocationList().Length == 0)
                    triggerSensor.OnUnsensedObject += OnUnsensedObjectInternal;
                onUnsensedObject += value;
            }
            remove
            {
                if (onUnsensedObject != null && onUnsensedObject.GetInvocationList().Length == 1)
                    triggerSensor.OnUnsensedObject -= OnUnsensedObjectInternal;
                onUnsensedObject -= value;
            }
        }

        private bool IsDirty()
        {
            return triggerSensor.DirtyFlag != dirtyFlag;
        }

        private void UpdateSensor()
        {
            sensedObjects.Clear();

            foreach (var otherObject in triggerSensor.SensedObjects)
            {
                var otherComponent = otherObject.GetComponentInChildren<T>();
                if (otherComponent != null) sensedObjects.Add(otherComponent);
            }

            dirtyFlag = triggerSensor.DirtyFlag;
        }

        private void OnSensedObjectInternal(GameObject otherObject)
        {
            var otherComponent = otherObject.GetComponentInChildren<T>();
            if (otherComponent != null && !sensedObjects.Contains(otherComponent))
            {
                sensedObjects.Add(otherComponent);
                NotifySensedObject(otherComponent);
            }

            dirtyFlag = triggerSensor.DirtyFlag;
        }

        private void OnUnsensedObjectInternal(GameObject otherObject)
        {
            var otherComponent = otherObject.GetComponentInChildren<T>();
            if (otherComponent != null && sensedObjects.Contains(otherComponent))
            {
                sensedObjects.Remove(otherComponent);
                NotifyUnsensedObject(otherComponent);
            }

            dirtyFlag = triggerSensor.DirtyFlag;
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

    public sealed class EmptyStickyTriggerSensor2D<T> : ISensor<T>
    {
        private readonly List<T> sensedObjects;

        public EmptyStickyTriggerSensor2D()
        {
            sensedObjects = new List<T>();
        }

        public IReadOnlyList<T> SensedObjects => sensedObjects;

        public event SensorEventHandler<T> OnSensedObject;
        public event SensorEventHandler<T> OnUnsensedObject;
    }
}