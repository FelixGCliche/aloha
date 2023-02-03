using System.Collections;
using Harmony;
using TMPro;
using UnityEngine;

namespace Game
{
    // Author : William L
    [Findable(Tags.GameController)]
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameObject achievementMessageBox;
        [SerializeField] private string baseAchievementMessage = "Vous avez débloqué le succès: ";
        
        private OnAchievementUnlockedEventChannel onAchievementUnlockedEventChannel;
        private OnPlayerLifeModifiedEventChannel onPlayerLifeModifiedEventChannel;
        private OnTemperatureModifiedEventChannel onTemperatureModifiedEventChannel;

        private AchievementTitle achievementTitle;
        private UserInterface userInterface;
        
        private void Awake()
        {
            onAchievementUnlockedEventChannel = Finder.OnAchievementUnlockedEventChannel;
            onTemperatureModifiedEventChannel = Finder.OnTemperatureModifiedEventChannel;
            onPlayerLifeModifiedEventChannel = Finder.OnPlayerLifeModifiedEventChannel;

            achievementTitle = Finder.AchievementTitle;
            userInterface = Finder.UserInterface;

            userInterface.gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            onAchievementUnlockedEventChannel.OnAchivementUnlocked += OnAchievementUnlocked;
            onTemperatureModifiedEventChannel.OnTemperatureModified += OnTemperatureModified;
            onPlayerLifeModifiedEventChannel.OnPlayerLifeModified += OnPlayerLifeModified;
        }
        private void OnDisable()
        {
            onAchievementUnlockedEventChannel.OnAchivementUnlocked -= OnAchievementUnlocked;
            onTemperatureModifiedEventChannel.OnTemperatureModified -= OnTemperatureModified;
            onPlayerLifeModifiedEventChannel.OnPlayerLifeModified -= OnPlayerLifeModified;
        }
        
        private void OnAchievementUnlocked(AchievementType achievement)
        {
            StartCoroutine(NoticeUnlockedAchievement());

            IEnumerator NoticeUnlockedAchievement()
            {
                achievementMessageBox.SetActive(true);
                var achievementTextMesh = achievementMessageBox.GetComponentInChildren<TextMeshProUGUI>();
                achievementTextMesh.text = baseAchievementMessage + achievementTitle.Title(achievement);
                achievementTextMesh.enabled = true;
                yield return new WaitForSeconds(5);
                achievementTextMesh.enabled = false;
                achievementMessageBox.SetActive(false);
            }
        }

        private void OnTemperatureModified(TemperatureStats playerTemperatureStats)
        {
            userInterface.OnTemperatureModified(playerTemperatureStats);
        }

        private void OnPlayerLifeModified(Player player)
        {
            userInterface.OnPlayerLifeModified(player);
        }
    }
}