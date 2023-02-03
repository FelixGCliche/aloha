using Cinemachine;
using UnityEngine;

namespace Game
{
    // Author: Louis RD
    public class FullLevelCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera fullLevelCamera;
        [SerializeField] private CinemachineVirtualCamera playerCamera;

        private void Awake()
        {
            if (fullLevelCamera != null)
                fullLevelCamera.enabled = true;
            if (playerCamera != null)
                playerCamera.enabled = false;
        }

        public void StartPlayerCamera()
        {
            if (!fullLevelCamera.isActiveAndEnabled) return;
            
            fullLevelCamera.enabled = false;
            playerCamera.enabled = true;
        }

        public void StartFullLevelCamera()
        {
            if (!playerCamera.isActiveAndEnabled) return;

            playerCamera.enabled = false;
            fullLevelCamera.enabled = true;
        }
    }
}