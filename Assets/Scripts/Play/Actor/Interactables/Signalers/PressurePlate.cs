using System.Linq;
using UnityEngine;

namespace Game
{
    // Author: David D
    public class PressurePlate : MonoBehaviour
    {
        [SerializeField] private Sprite inactiveSprite;
        [SerializeField] private Sprite activeSprite;
        
        private ISensor<IEntity> entitySensor;
        private SignalSender signalSender;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            entitySensor = GetComponent<TriggerSensor2D>().For<IEntity>();
            signalSender = GetComponent<SignalSender>();
        }

        private void OnEnable()
        {
            entitySensor.OnSensedObject += UpdateState;
            entitySensor.OnUnsensedObject += UpdateState;
        }

        private void OnDisable()
        {
            entitySensor.OnSensedObject -= UpdateState;
            entitySensor.OnUnsensedObject -= UpdateState;
        }

        private void UpdateState(IEntity entity)
        {
            signalSender.IsActivated = entitySensor.SensedObjects.Any();
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            spriteRenderer.sprite = signalSender.RawActivated ? activeSprite : inactiveSprite;
        }
    }
}