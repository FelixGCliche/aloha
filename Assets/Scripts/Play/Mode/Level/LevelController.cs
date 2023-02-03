using Cinemachine;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Tout le monde
    [Findable(Tags.LevelController)]
    public class LevelController : MonoBehaviour
    {
        [SerializeField] [Range(0, 140)] private float playerCameraZoom = 15f;

        private CinemachineVirtualCamera cVCamera;
        private OnLevelEndEventChannel onLevelEndEventChannel;
        private GameMemory gameMemory;

        private void Awake()
        {
            cVCamera = gameObject.Parent().GetComponentInChildren<CinemachineVirtualCamera>();
            onLevelEndEventChannel = Finder.OnLevelEndEventChannel;
            gameMemory = Finder.GameMemory;
        }

        private void Start()
        {
            GameObject artefactObject = GameObject.Find(GameObjects.Artefact);
            if(artefactObject != null)
            {
                Artefact artefact = artefactObject.GetComponent<Artefact>();
                if (gameMemory.HasCollectedArtefact(artefact.ArtefactType))
                {
                    artefact.SetToAlreadyCollected();
                }
            }

            cVCamera.m_Lens.OrthographicSize = playerCameraZoom;
        }

        private void OnEnable()
        {
            onLevelEndEventChannel.OnLevelEnd += OnLevelEnd;
        }
        
        private void OnDisable()
        {
            onLevelEndEventChannel.OnLevelEnd -= OnLevelEnd;
        }
        
        private void OnLevelEnd(SceneName scene)
        {
            gameMemory.AddLevelToCompletedList(scene);
        }
    }
}