using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public sealed class TriggerStimuli2D : MonoBehaviour
    {
        [SerializeField] private TriggerShape2D shape = TriggerShape2D.Circle;
        [SerializeField] [Range(1, 100)] private float size = 1f;
        [SerializeField][Tooltip("Spécifie si le collider utilisé doit être existant, en fonction du sprite ou nouveau")]
        private SensorColliderScalingType sensorColliderScalingType = SensorColliderScalingType.WithNewCollider;
        
        private Collider2D collider2D = null;
        
        // Félix B
        private void Awake()
        {
            if (sensorColliderScalingType == SensorColliderScalingType.UseExistingCollider)
            {
                collider2D = gameObject.GetComponentInParent<Collider2D>();
                collider2D.isTrigger = true;
            }
            else
            {
                CreateCollider();
            }
            SetSensorLayer();
        }

        private void OnDestroy()
        {
            NotifyDestroyed();
        }

        public event StimuliEventHandler OnDestroyed;

        // Felix B
        private void CreateCollider()
        {
            switch (shape)
            {
                case TriggerShape2D.Square:
                    var boxCollider = gameObject.AddComponent<BoxCollider2D>();
                    if(sensorColliderScalingType == SensorColliderScalingType.NewColliderScaleWithSprite)
                    { 
                        var sprite = transform.root.GetComponentInChildren<SpriteRenderer>();
                        if (sprite != null)
                        {
                            boxCollider.size = sprite.size;
                            boxCollider.offset = sprite.transform.position - boxCollider.transform.position;
                        }
                        
                    }
                    boxCollider.isTrigger = true;
                    boxCollider.size *= size;
                    break;
                case TriggerShape2D.Circle:
                    var circleCollider = gameObject.AddComponent<CircleCollider2D>();
                    circleCollider.isTrigger = true;
                    circleCollider.radius = size / 2;
                    break;
                default:
                    throw new Exception("Unknown shape named \"" + shape + "\".");
            }
        }

        private void SetSensorLayer()
        {
            gameObject.layer = Layers.Sensor;
        }

        private void NotifyDestroyed()
        {
            if (OnDestroyed != null)
                OnDestroyed((transform.parent != null ? transform.parent : transform).gameObject);
        }
    }

    public delegate void StimuliEventHandler(GameObject otherObject);
}