using System.Linq;
using Harmony;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    //Author : William Lemelin
    public class LevelSpawnpointTeleporter : MonoBehaviour
    {
        [FormerlySerializedAs("levelLoaded")] [SerializeField] private SceneName sceneLoaded;
        
        private LevelSpawnpoint[] spawnPointList;

        private void Awake()
        {
            spawnPointList = GameObject.FindGameObjectsWithTag(Tags.Respawn)
                .Select(spawnPoint => spawnPoint.GetComponent<LevelSpawnpoint>()).ToArray();
        }

        private void Start()
        {
            TeleportPlayerToCheckpoint();
        }

        private void TeleportPlayerToCheckpoint()
        {
            var player = Finder.Player;
            var levelName = Finder.SceneLoadSetting.LastSceneLoaded;

            if (spawnPointList.Length > 0)
            {
                var spawnPoint = spawnPointList.FirstOrDefault(it => it.Name == levelName);
                if (spawnPoint != null) player.transform.position = spawnPoint.Position;
            }

            Finder.SceneLoadSetting.LastSceneLoaded = sceneLoaded;
        }
    }
}