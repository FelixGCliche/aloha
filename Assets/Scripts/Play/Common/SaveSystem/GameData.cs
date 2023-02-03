using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    // Author: David D
    public class GameData
    {
        private List<SceneName> unlockedLevels;
        private List<ArtefactType> collectedArtefacts;
        private List<AchievementType> unlockedAchievements;
        private float gameTimeInSeconds;
        private DateTime saveDate;

        public List<ArtefactType> CollectedArtefacts => collectedArtefacts;
        public List<SceneName> UnlockedLevels => unlockedLevels;
        // Author: William Lemelin
        public List<AchievementType> UnlockedAchievements => unlockedAchievements;
        public float GameTimeInSeconds { get => gameTimeInSeconds; set => gameTimeInSeconds = value; }
        public DateTime SaveDate { get => saveDate; set => saveDate = value; }
        
        public GameData()
        {
            unlockedLevels = new List<SceneName>();
            collectedArtefacts = new List<ArtefactType>();
            unlockedAchievements = new List<AchievementType>();
            gameTimeInSeconds = 0f;
            saveDate = DateTime.Today;
        }

        public string ToJson()
        {
            GameDataJson data = new GameDataJson
            {
                jCollectedArtefacts = collectedArtefacts,
                jSaveDate = saveDate.ToString(),
                jUnlockedAchievements = unlockedAchievements,
                jUnlockedLevels = unlockedLevels,
                jGameTimeInSeconds = gameTimeInSeconds
            };

            return JsonUtility.ToJson(data);
        }
        
        [Serializable]
        public struct GameDataJson
        {
            public List<SceneName> jUnlockedLevels;
            public List<ArtefactType> jCollectedArtefacts;
            public List<AchievementType> jUnlockedAchievements;
            public float jGameTimeInSeconds;
            // DateTime ne fonctionne pas avec json ???
            public string jSaveDate;

            public GameData FromJson()
            {
                GameData data = new GameData
                {
                    collectedArtefacts = jCollectedArtefacts,
                    saveDate = DateTime.Parse(jSaveDate),
                    unlockedAchievements = jUnlockedAchievements,
                    unlockedLevels = jUnlockedLevels,
                    gameTimeInSeconds = jGameTimeInSeconds
                };
                return data;
            }
        }
    }
}