using UnityEngine;

namespace Game
{
    // Author: David D, Félix B
    [RequireComponent(typeof(TemperatureStats))]
    public abstract class ObjectTemperature : MonoBehaviour, ITemperature
    {
        [SerializeField] [Range(0f, 1f)] private float naturalTemperature = 0.5f;
        [SerializeField] [Tooltip("Puissance/Vitesse avec laquelle l'entité tend à retourner à sa température de base.")]
        [Range(0f, 1f)] private float normalizingFactor = 0.25f;
        
        [Header("RetroAction")]
        [SerializeField] private Material coldMaterial = null;
        [SerializeField] private Material hotMaterial = null;
        [SerializeField] [Tooltip("Spécifie à quel % de ca température maximum l'objet devrait afficher le matériel chaud à 100%")]
        [Range(0,1)] private float hotMaterialBlendingTresHold = 1f;
        [SerializeField] [Tooltip("Spécifie à quel % de ca température maximum l'objet devrait afficher le matériel froid à 100%")]
        [Range(0,1)] private float coldMaterialBlendingTresHold = 0f;

        protected TemperatureStats temperatureStats;
        private bool shouldBlendMaterials = false;

        protected abstract Renderer Sprite { get; }
        public TemperatureStats TemperatureStats => temperatureStats;
       
        private float ObjectTemperatureVariationFactor
        {
            get
            {
                if (Mathf.Approximately(temperatureStats.Temperature, naturalTemperature))
                    return 0f;
                if (temperatureStats.Temperature < naturalTemperature)
                    return normalizingFactor;
                if (temperatureStats.Temperature > naturalTemperature)
                    return -normalizingFactor;

                return 0f;
            }
        }

        protected virtual void Awake()
        {
            temperatureStats = GetComponent<TemperatureStats>();
        }

        private void Start()
        {
            if (coldMaterial != null && hotMaterial != null)
            {
                Sprite.material = coldMaterial;
                shouldBlendMaterials = true;
            }
        }

        protected virtual void Update()
        {
            temperatureStats.ObjectInfluence = ObjectTemperatureVariationFactor;
            
            if(shouldBlendMaterials)
                Sprite.material.Lerp(coldMaterial,hotMaterial,GetBlendingValue());   
        }

        private float GetBlendingValue()
        {
            float blendingValue;
            float unitValue = temperatureStats.Temperature / temperatureStats.TemperatureTresholdsRange;
            
            if(unitValue < coldMaterialBlendingTresHold)
            { 
                blendingValue = 0;
            }
            else if(unitValue > hotMaterialBlendingTresHold)
            {
                 blendingValue = 1;
            }
            else
            {
                //Règles de trois
                float a = unitValue * (1 - coldMaterialBlendingTresHold);
                float b = unitValue * hotMaterialBlendingTresHold;
                //Moyenne
                float c = (a + b) / 2;
                blendingValue = Mathf.Clamp(c,0 , 1 );
            }

            return blendingValue;
        }
    }
}