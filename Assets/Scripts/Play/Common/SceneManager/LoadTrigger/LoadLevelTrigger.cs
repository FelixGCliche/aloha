using Harmony;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    //Author : William Lemelin
    [RequireComponent(typeof(TriggerSensor2D))]
    public class LoadLevelTrigger : MonoBehaviour
    {
        [FormerlySerializedAs("levelToLoad")] [SerializeField] private SceneName sceneToLoad;
        [SerializeField] private Sprite openSprite;
        [SerializeField] private Sprite closedSprite;
        
        private InteractSensor interactSensor;
        private Main main;
        private SpriteRenderer spriteRenderer;
        private GameMemory gameMemory;

        private void Awake()
        {
            main = Finder.Main;
            gameMemory = Finder.GameMemory;
            interactSensor = GetComponent<InteractSensor>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = Finder.Main.CanEnterLevel(sceneToLoad) ? openSprite : closedSprite;
        }

        private void Start()
        {
            interactSensor.OnInteract += EnterLevel;
        }

        private void OnDestroy()
        {
            interactSensor.OnInteract -= EnterLevel;
        }

        private void EnterLevel(InteractSensor interactSensor)
        {
            if (main.CanEnterLevel(sceneToLoad))
            {
                gameMemory.CleanData();
                main.GoToScene(sceneToLoad);
            }
        }
    }
}