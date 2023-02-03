using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    //Author: Felix.B
    public class Highlightable : MonoBehaviour
    {
        [SerializeField] private List<string> paramEffectStrings; 
        
        private Material mat;
        private ISensor<Player> playerSensor;
        
        private void Awake()
        {
            playerSensor = GetComponent<TriggerSensor2D>().For<Player>();
            mat = GetComponentInParent<Renderer>().material;
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
        
        
        private void Highlight(bool onOff)
        {
            switch (onOff)
            {
                case true:
                    foreach (var param in paramEffectStrings)
                    {
                        mat.EnableKeyword(param);
                    }
                    break;
                case false:
                    foreach (var param in paramEffectStrings)
                    {
                        mat.DisableKeyword(param);
                    }
                    break;
            }
        }
        
        private void OnPlayerSensed(Player player)
        {
            Highlight(true);
        }
        
        private void OnPlayerUnsensed(Player player)
        {
            Highlight(false);
        }
    }
}