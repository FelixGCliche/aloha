using UnityEngine;

namespace Game
{
    //Author: William L
    public class GameObjectActivationTrigger : MonoBehaviour
    {
        [Tooltip("GameObject que l'on veut faire afficher en l'activant")]
        [SerializeField] private GameObject objectToActivate;
        
        private ISensor<Player> playerSensor;

        private void Awake()
        {
            playerSensor = GetComponent<TriggerSensor2D>().For<Player>();
            objectToActivate.SetActive(false);
        }

        private void OnEnable()
        {
            playerSensor.OnSensedObject += OnPlayerSensed;
            playerSensor.OnUnsensedObject += OnPlayerUnsensed;
        }

        private void OnDisable()
        {
            playerSensor.OnSensedObject -= OnPlayerSensed;
            playerSensor.OnUnsensedObject -= OnPlayerUnsensed;
        }

        private void OnPlayerSensed(Player player)
        {
            objectToActivate.SetActive(true);
        }

        private void OnPlayerUnsensed(Player player)
        {
            objectToActivate.SetActive(false);
        }
    }
}