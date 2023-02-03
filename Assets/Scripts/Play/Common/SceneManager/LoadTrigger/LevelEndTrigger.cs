using Harmony;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    //Author : William Lemelin
    [RequireComponent(typeof(TriggerSensor2D))]
    public class LevelEndTrigger : MonoBehaviour
    {
        [FormerlySerializedAs("levelToLoad")] [SerializeField] private SceneName sceneToLoad;
        
        private InteractSensor interactSensor;
        private Main main;
        private OnLevelEndEventChannel onLevelEndEventChannel;

        private void Awake()
        {
            main = Finder.Main;
            interactSensor = GetComponent<InteractSensor>();
            onLevelEndEventChannel = Finder.OnLevelEndEventChannel;
        }

        private void Start()
        {
            interactSensor.OnInteract += ExitLevel;
        }

        private void OnDestroy()
        {
            interactSensor.OnInteract -= ExitLevel;
        }

        private void ExitLevel(InteractSensor interactSensor)
        {
            onLevelEndEventChannel.Publish(Finder.SceneLoadSetting.LastSceneLoaded);
            main.GoToScene(sceneToLoad);
        }
    }
}