using Harmony;
using UnityEngine;
using UnityEngine.Audio;

namespace Game
{
    // Author: David Dorion, William Lemelin
    [Findable(Tags.MainController)]
    public class GameMemory : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private string volumeVariableName = "Volume";

        private GameData gameData;
        private bool hasNewData;
        private float startPlayTime;
        private OptionData optionData;
        private SaveSystem saveSystem;
        private Resolution[] resolutions;

        public TempState LastPlayerState { get; set; }
        public SaveSlot SaveSlot { get; set; }
        public bool HasNewData => hasNewData;
        public OptionData OptionData
        {
            get => optionData;
            set
            {
                optionData = value;
                saveSystem.SaveOptionData(optionData);
            }
        }
        public GameData GameData
        {
            get => gameData;

            set => gameData = value;
        }
        public Resolution[] Resolutions => resolutions;
        
        private void Awake()
        {
            saveSystem = GetComponent<SaveSystem>();
            
            NewGameMemory();

            resolutions = Screen.resolutions;

            optionData = saveSystem.LoadOptionData();
        }

        private void Start()
        {
            if(optionData != null)
                ApplyOptions();
            else 
                optionData = new OptionData();
        }

        public void NewGameMemory()
        {
            hasNewData = true;
            gameData = new GameData();
            
            SaveSlot = 0;
            startPlayTime = 0f;
            LastPlayerState = TempState.Hot;
        }

        public void AddLevelToCompletedList(SceneName sceneName)
        {
            if (!HasCompletedLevel(sceneName))
            {
                hasNewData = true;
                gameData.UnlockedLevels.Add(sceneName);
            }
        }

        public bool HasCompletedLevel(SceneName sceneName)
        {
            return gameData.UnlockedLevels.Contains(sceneName);
        }

        public void AddArtefactToCollectedList(ArtefactType artefact)
        {
            if (!HasCollectedArtefact(artefact))
            {
                hasNewData = true;
                gameData.CollectedArtefacts.Add(artefact);
            }
        }

        public bool HasCollectedArtefact(ArtefactType artefact)
        {
            return gameData.CollectedArtefacts.Contains(artefact);
        }

        //Author : William Lemelin
        public bool AddAchievementToUnlockedList(AchievementType achievement)
        {
            if (!HasUnlockedAchievement(achievement))
            {
                hasNewData = true;
                gameData.UnlockedAchievements.Add(achievement);
                return true;
            }

            return false;
        }
        
        //Author : William Lemelin
        public bool HasUnlockedAchievement(AchievementType achievement)
        {
            return gameData.UnlockedAchievements.Contains(achievement);
        }

        private void RemoveArtefact(ArtefactType artefact)
        {
            if (HasCollectedArtefact(artefact))
            {
                hasNewData = true;
                gameData.CollectedArtefacts.Remove(artefact);
            }
        }

        public void CleanData()
        {
            if (!HasCompletedLevel(SceneName.Level1))
                RemoveArtefact(ArtefactType.DoubleJump);
            if (!HasCompletedLevel(SceneName.Level2))
                RemoveArtefact(ArtefactType.Freeze);
            if (!HasCompletedLevel(SceneName.Level3))
                RemoveArtefact(ArtefactType.Grapple);
            if (!HasCompletedLevel(SceneName.LevelFire))
                RemoveArtefact(ArtefactType.Geyser);
            if (!HasCompletedLevel(SceneName.LevelIce))
                RemoveArtefact(ArtefactType.DashBreak);
        }

        public void StartPlayTimeTimer()
        {
            startPlayTime = Time.time;
        }

        public void StopPlayTimeTimer()
        {
            if (startPlayTime == 0f) return;

            gameData.GameTimeInSeconds += Time.time - startPlayTime;
            startPlayTime = 0f;
        }

        // BR : Ben approuves
        private void ApplyOptions()
        {
            Screen.fullScreen = optionData.fullscreen;
            audioMixer.SetFloat(volumeVariableName, optionData.volume);
            Screen.SetResolution(optionData.width,optionData.height,optionData.fullscreen,optionData.refreshRate);
        }
    }
}