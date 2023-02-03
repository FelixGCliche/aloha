using Harmony;
using UnityEngine;

namespace Game
{
    //Author : William L
    public class DialogueActivationTrigger : MonoBehaviour
    {
        [SerializeField] private string dialogueText;

        private OnDialogueToShowEventChannel onDialogueToShowEventChannel;
        private ISensor<Player> playerSensor;

        private void Awake()
        {
            onDialogueToShowEventChannel = Finder.OnDialogueToShowEventChannel;
            playerSensor = GetComponent<TriggerSensor2D>().For<Player>();
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
            onDialogueToShowEventChannel.Publish(dialogueText);
        }

        private void OnPlayerUnsensed(Player player)
        {
            //Permets de rappeler le OnPlayerSensed si le joueur revient dans la zone
        }
    }
}