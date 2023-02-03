using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    // Félix B
    public class HighlightableGroupController : MonoBehaviour
    {
        [SerializeField] private List<string> paramNames = null;
        [SerializeField] private List<SpriteRenderer> sprites = null;
        
        private ISensor<Player> playerSensor;
        
        private void Awake()
        {
            playerSensor = GetComponent<TriggerSensor2D>().For<Player>();
        }
        
        public void OnEnable()
        {
            Highlight(false);
            
            playerSensor.OnSensedObject += OnPlayerSensed;
            playerSensor.OnUnsensedObject += OnPlayerUnsensed;
        }
        
        private void OnDisable()
        {
            playerSensor.OnSensedObject -= OnPlayerSensed;
            playerSensor.OnUnsensedObject -= OnPlayerUnsensed;
        }
        
        private void OnPlayerSensed(Player player)
        {
            Highlight(true);
        }
        
        private void OnPlayerUnsensed(Player player)
        {
            Highlight(false);
        }
        
        private void Highlight( bool onOff)
        {
            Material mat = null;
            foreach (var sprite in sprites)
            {
                mat = sprite.material;
                switch (onOff)
                {
                    case true:
                        foreach (var param in paramNames)
                        {
                            mat.EnableKeyword(param);
                        }
                        break;
                    case false:
                        foreach (var param in paramNames)
                        {
                            mat.DisableKeyword(param);
                        }
                        break;
                }
            }
        }
    }
}