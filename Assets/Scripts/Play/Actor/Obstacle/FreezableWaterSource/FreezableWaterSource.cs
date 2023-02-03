using Harmony;
using UnityEngine;

namespace Game
{
    // Author: Félix B, David D
    public class FreezableWaterSource : ObjectTemperature
    {
        [Header("Audio")]
        [SerializeField] private AudioClip freezeSound;
        
        private GameObject frozenStructureGameObject;
        private GameObject liquidStructureGameObject;
        private SpriteRenderer sprite;

        protected override Renderer Sprite => sprite;
        private AudioSource audioSource;

        public Material InUseMaterial => sprite.material;
        private new void Awake()
        {
            base.Awake();
            
            frozenStructureGameObject = transform.Find(GameObjects.FrozenStateStructure).gameObject;
            liquidStructureGameObject = GetComponentInChildren<LiquidStateStructure>().gameObject;
            audioSource = GetComponent<AudioSource>();
            sprite = GetComponent<SpriteRenderer>();
            
            ChangeStates(temperatureStats);
        }

        private void OnEnable()
        {
            temperatureStats.OnChangeTempState += ChangeStates;
        }

        private void OnDisable()
        {
            temperatureStats.OnChangeTempState -= ChangeStates;
        }
        
        private void ChangeStates(TemperatureStats temperatureStats)
        {
            switch (temperatureStats.TemperatureState)
            {
                case TempState.Frozen:
                    audioSource.PlayOneShot(freezeSound);
                    liquidStructureGameObject.SetActive(false);
                    frozenStructureGameObject.SetActive(true);
                    break;
                case TempState.Hot:
                    frozenStructureGameObject.SetActive(false);
                    liquidStructureGameObject.SetActive(true);
                    break;
            }
        }
    }
}