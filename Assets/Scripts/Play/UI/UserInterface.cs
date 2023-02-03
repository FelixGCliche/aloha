using Harmony;
using UnityEngine;

namespace Game
{
    // LouisRD
    [Findable(Tags.UICanvas)]
    public class UserInterface : MonoBehaviour
    {
        private Canvas canvas;
        private PlayerLifeBar playerLifeBar;
        private TemperatureBar temperatureBar;
        private ActionCooldownIcon[] artefactCooldowns;

        private void Awake()
        {
            canvas = gameObject.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            
            playerLifeBar = GetComponentInChildren<PlayerLifeBar>();
            temperatureBar = GetComponentInChildren<TemperatureBar>();
            artefactCooldowns = GetComponentsInChildren<ActionCooldownIcon>();
        }

        public void OnPlayerLifeModified(Player player)
        {
            playerLifeBar.OnPlayerLifeModified(player);
        }

        public void OnTemperatureModified(TemperatureStats playerTemperatureStats)
        {
            temperatureBar.OnTemperatureModified(playerTemperatureStats);
        }

        public void DisplayArtefactCoolDown()
        {
            foreach (var artefactCooldown in artefactCooldowns)
            {
                artefactCooldown.DisplayArtefactCooldown();
            }
        }
    }
}