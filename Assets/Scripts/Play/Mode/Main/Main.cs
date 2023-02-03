using System;
using System.Collections;
using DG.Tweening;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author : William L
    [Findable(Tags.MainController)]
    public class Main : MonoBehaviour
    {
        [Header("Scenes")]
        [SerializeField] private SceneBundle homeScenes;
        [SerializeField] private SceneBundle loadingScreenScenes;
        [SerializeField] private SceneBundle gameScenes;
        [SerializeField] private SceneBundle tutorialScenes;
        [SerializeField] private SceneBundle hubScenes;
        [SerializeField] private SceneBundle level1Scenes;
        [SerializeField] private SceneBundle level2Scenes;
        [SerializeField] private SceneBundle level3Scenes;
        [SerializeField] private SceneBundle levelFireScenes;
        [SerializeField] private SceneBundle levelIceScenes;
        [SerializeField] private SceneBundle endScenes;

        private SceneBundleLoader loader;
        private SaveSystem saveSystem;
        private SceneLoadSetting sceneLoadSetting;
        private GameMemory gameMemory;

        private void Awake()
        {
            loader = GetComponent<SceneBundleLoader>();
            saveSystem = GetComponent<SaveSystem>();

            DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
            DOTween.SetTweensCapacity(200, 125);

            sceneLoadSetting = GetComponent<SceneLoadSetting>();
            gameMemory = GetComponent<GameMemory>();
        }

        private IEnumerator Start()
        {
#if UNITY_EDITOR
            if (gameScenes.IsLoaded)
                yield return loader.Load(gameScenes);
            else
#endif
                yield return loader.Load(homeScenes);
        }

        public Coroutine LoadGameScenes()
        {
            gameMemory.StartPlayTimeTimer();
            return loader.Load(gameScenes);
        }

        private Coroutine UnloadGameScenes()
        {
            gameMemory.StopPlayTimeTimer();
            return loader.Unload(gameScenes);
        }
        
        public bool CanEnterLevel(SceneName sceneToEnter)
        {
            switch (sceneToEnter)
            {
                case SceneName.Level1:
                    return true;
                case SceneName.Level2:
                    return gameMemory.HasCompletedLevel(SceneName.Level1);
                case SceneName.Level3:
                    return gameMemory.HasCompletedLevel(SceneName.Level2);
                case SceneName.LevelFire:
                    return gameMemory.HasCompletedLevel(SceneName.Level3);
                case SceneName.LevelIce:
                    return gameMemory.HasCompletedLevel(SceneName.Level3);
                case SceneName.End:
                    return gameMemory.HasCompletedLevel(SceneName.LevelFire) &&
                           gameMemory.HasCompletedLevel(SceneName.LevelIce);
            }
            return true;
        }

        public Coroutine GoToScene(SceneName sceneToLoad)
        {
            IEnumerator Routine()
            {
                yield return loader.Load(loadingScreenScenes);

                if (homeScenes.IsLoaded) yield return loader.Unload(homeScenes);
                if (tutorialScenes.IsLoaded) yield return loader.Unload(tutorialScenes);
                if (hubScenes.IsLoaded) yield return loader.Unload(hubScenes);
                if (level1Scenes.IsLoaded) yield return loader.Unload(level1Scenes);
                if (level2Scenes.IsLoaded) yield return loader.Unload(level2Scenes);
                if (level3Scenes.IsLoaded) yield return loader.Unload(level3Scenes);
                if (levelFireScenes.IsLoaded) yield return loader.Unload(levelFireScenes);
                if (levelIceScenes.IsLoaded) yield return loader.Unload(levelIceScenes);
                if (endScenes.IsLoaded) yield return loader.Unload(endScenes);

                switch (sceneToLoad)
                {
                    case SceneName.Home :
                        yield return loader.Load(homeScenes);
                        break;
                    case SceneName.Tutorial:
                        yield return loader.Load(tutorialScenes);
                        break;
                    case SceneName.Hub:
                        yield return loader.Load(hubScenes);
                        break;
                    case SceneName.Level1:
                        yield return loader.Load(level1Scenes);
                        break;
                    case SceneName.Level2:
                        yield return loader.Load(level2Scenes);
                        break;
                    case SceneName.Level3:
                        yield return loader.Load(level3Scenes);
                        break;
                    case SceneName.LevelFire:
                        yield return loader.Load(levelFireScenes);
                        break;
                    case SceneName.LevelIce:
                        yield return loader.Load(levelIceScenes);
                        break;
                    case SceneName.End:
                        sceneLoadSetting.LastSceneLoaded = SceneName.End;
                        yield return loader.Load(endScenes);
                        break;
                }

                yield return loader.Unload(loadingScreenScenes);
            }

            return StartCoroutine(Routine());
        }
        
        public void GoToHomeMenu()
        {
            IEnumerator Routine()
            {
                yield return GoToScene(SceneName.Home);
                yield return UnloadGameScenes();
                sceneLoadSetting.LastSceneLoaded = SceneName.Home;
                
                SaveData();
            }

            StartCoroutine(Routine());
        }

        // David Dorion
        private void SaveData()
        {
            gameMemory.CleanData();
            
            if (gameMemory.HasNewData && gameMemory.SaveSlot !=0)
            {
                GameData data = gameMemory.GameData;
                data.SaveDate = DateTime.Today;
                saveSystem.SaveGameData(data,gameMemory.SaveSlot);
            }
        }
    }
}