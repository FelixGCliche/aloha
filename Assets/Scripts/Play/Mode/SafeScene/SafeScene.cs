using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Louis RD
    public class SafeScene : MonoBehaviour
    {
        private Player player;

        private void Awake()
        {
            player = Finder.Player;
        }

        private void OnEnable()
        {
            if (player != null)
                player.SetPlayerIsInvincible(true);
        }

        private void OnDisable()
        {
            if (player != null)
                player.SetPlayerIsInvincible(false);
        }
    }
}