using UnityEngine;

namespace Game
{
    //Author : William Lemelin
    public class LevelSpawnpoint : MonoBehaviour
    {
        [SerializeField] private SceneName name;

        public Vector3 Position => transform.position;
        public SceneName Name => name;
    }
}