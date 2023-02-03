using Harmony;
using UnityEngine;

namespace Game
{
    //Author: William Lemelin
    public class AchievementController : MonoBehaviour
    {
        private OnArtefactCollectEventChannel onArtefactCollectEventChannel;
        private OnLevelEndEventChannel onLevelEndEventChannel;
        private OnPlayerDeathEventChannel onPlayerDeathEventChannel;
        private OnAchievementUnlockedEventChannel onAchievementUnlockedEventChannel;

        private GameMemory gameMemory;

        private void Awake()
        {
            onArtefactCollectEventChannel = Finder.OnArtefactCollectEventChannel;
            onLevelEndEventChannel = Finder.OnLevelEndEventChannel;
            onPlayerDeathEventChannel = Finder.OnPlayerDeathEventChannel;
            onAchievementUnlockedEventChannel = Finder.OnAchievementUnlockedEventChannel;
            gameMemory = Finder.GameMemory;
        }

        private void OnEnable()
        {
            onArtefactCollectEventChannel.OnArtefactCollect += OnArtefactCollect;
            onLevelEndEventChannel.OnLevelEnd += OnLevelEnd;
            onPlayerDeathEventChannel.OnPlayerDeath += OnPlayerDeath;
        }

        private void OnDisable()
        {
            onArtefactCollectEventChannel.OnArtefactCollect -= OnArtefactCollect;
            onLevelEndEventChannel.OnLevelEnd -= OnLevelEnd;
            onPlayerDeathEventChannel.OnPlayerDeath -= OnPlayerDeath;
        }

        private void OnArtefactCollect(ArtefactType artefact)
        {
            AchievementType artefactCollected = artefact.ConvertArtefactTypeToAchievementType();
            if(gameMemory.AddAchievementToUnlockedList(artefactCollected))
                onAchievementUnlockedEventChannel.Publish(artefactCollected);
        }

        private void OnLevelEnd(SceneName scene)
        {
            AchievementType levelEnded = scene.ConvertSceneNameToAchievementType();
            if(gameMemory.AddAchievementToUnlockedList(levelEnded))
                onAchievementUnlockedEventChannel.Publish(levelEnded);
        }

        private void OnPlayerDeath()
        {
            if (gameMemory.AddAchievementToUnlockedList(AchievementType.Dying))
                onAchievementUnlockedEventChannel.Publish(AchievementType.Dying);
        }
    }
}