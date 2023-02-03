using Harmony;
using UnityEngine;

namespace Game
{
    //Author : William Lemelin
    [Findable(Tags.MainController)]
    public class SceneLoadSetting : MonoBehaviour
    {
        public SceneName LastSceneLoaded { get; set; }

        private void Awake()
        {
            LastSceneLoaded = SceneName.Home;
        }
    }
}