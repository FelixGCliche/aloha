using UnityEngine;

namespace Game
{
    // Author : Félix B
    public class LiquidStateStructure : MonoBehaviour
    {
        [SerializeField] [Range(0f, 1f)] private float damageAmount = 1f;
        [SerializeField] private DamageType damageType = DamageType.Water;

        private ISensor<Player> playerSensor;
        private ISensor<EssentialRespawner> essentialRespawnerSensor;

        private void Awake()
        {
            playerSensor = GetComponentInChildren<TriggerSensor2D>().For<Player>();
            essentialRespawnerSensor = GetComponentInChildren<TriggerSensor2D>().For<EssentialRespawner>();
        }

        private void OnEnable()
        {
            playerSensor.OnSensedObject += OnPlayerSensed;
            essentialRespawnerSensor.OnSensedObject += OnEssentialRespawnerSensed;
            essentialRespawnerSensor.OnUnsensedObject += OnUnsensed;
        }
        
        private void OnUnsensed(EssentialRespawner essentialRespawner) {/* Needed because of a bug */}

        private void OnDisable()
        {
            playerSensor.OnSensedObject -= OnPlayerSensed;
            essentialRespawnerSensor.OnSensedObject -= OnEssentialRespawnerSensed;
            essentialRespawnerSensor.OnUnsensedObject -= OnUnsensed;
        }

        private void OnPlayerSensed(Player player)
        {
            player.Hurt(damageAmount);
        }
        
        private void OnEssentialRespawnerSensed(EssentialRespawner essentialRespawner)
        {
            essentialRespawner.Respawn(damageType);
        }
    }
}