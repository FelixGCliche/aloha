using Harmony;
using UnityEngine;

namespace Game
{
    // LouisRD
    public class HubController : MonoBehaviour
    {
        private Player player;
        private UserInterface userInterface;

        private void Awake()
        {
            player = Finder.Player;

            userInterface = Finder.UserInterface;

            player.IsInHub = true;
            
            userInterface.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            userInterface.gameObject.SetActive(true);
        }
    }
}