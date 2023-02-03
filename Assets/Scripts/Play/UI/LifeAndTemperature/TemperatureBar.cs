using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    //Author : Felix B
    public class TemperatureBar : MonoBehaviour
    {
        [Header("Colors")]
        [SerializeField] private Color colorHot;
        [SerializeField] private Color colorCold;
        
        [Header("Materials")] 
        [SerializeField] private Material hotMaterial = null;
        [SerializeField] private Material coldMaterial = null;
        [SerializeField] private string fadeParamName = "_FadeAmount";
        
        [Header("Others")]
        [SerializeField] [Range(0, 0.4f)] private float matFadingRange = 0.2f;
        [SerializeField] private string coldBarFillerGoName = "TempBarFillerCold";
        [SerializeField] private string hotBarFillerGoName = "TempBarFillerHot";

        private LiquidBar barFillerHot;
        private LiquidBar barFillerCold;
        private Image outLineImage;
        private TemperatureStats playerTemperatureStats;

        private void Awake()
        {
            barFillerCold = transform.Find(coldBarFillerGoName).GetComponentInChildren<LiquidBar>();
            barFillerHot = transform.Find(hotBarFillerGoName).GetComponentInChildren<LiquidBar>();
            outLineImage = GetComponent<Image>();
        }

        private void OnEnable()
        {
            StartCoroutine(UpdateMatCoRoutine());
        }

        private void Start()
        {
            barFillerCold.barColor = colorCold;
            barFillerHot.barColor = colorHot;
            
            if(coldMaterial != null)
                outLineImage.material = coldMaterial;
        }

        private void OnDisable()
        {
            if(hotMaterial != null)
                hotMaterial.SetFloat(fadeParamName, 0);
            if(coldMaterial != null)
                coldMaterial.SetFloat(fadeParamName, 0);
            
            StopCoroutine(UpdateMatCoRoutine());
        }

        public void OnTemperatureModified(TemperatureStats newPlayerTemperatureStats)
        {
            playerTemperatureStats = newPlayerTemperatureStats;

            barFillerCold.targetFillAmount = 1 - playerTemperatureStats.Temperature;
            barFillerHot.targetFillAmount = playerTemperatureStats.Temperature;
        }

        private IEnumerator UpdateMatCoRoutine()
        {
            while (isActiveAndEnabled)
            {
                if (playerTemperatureStats != null)
                {
                    if (playerTemperatureStats.Temperature < playerTemperatureStats.TemperatureTresholdsRange / 2)
                        outLineImage.material = coldMaterial;
                    else
                    {
                        outLineImage.material = hotMaterial;
                    }
                    outLineImage.material.SetFloat(fadeParamName, GetBlendingValue());
                }
                yield return null;
            }
        }

        private float GetBlendingValue()
        {
            float blendingValue = 0;
            float unitValue = playerTemperatureStats.Temperature / playerTemperatureStats.TemperatureTresholdsRange;
            
            if(playerTemperatureStats.Temperature < playerTemperatureStats.TemperatureTresholdsRange/2)
            { 
                if(unitValue > playerTemperatureStats.MinTemperature+ matFadingRange)
                { 
                    blendingValue = 0;
                }
                else
                {
                    //Règle de trois
                    float b = (matFadingRange - unitValue) / matFadingRange;
                    blendingValue = Mathf.Clamp(b,0 , 1 );
                }
            }
            else if (playerTemperatureStats.Temperature > playerTemperatureStats.TemperatureTresholdsRange/2)
            {
                if(unitValue < playerTemperatureStats.MaxTemperature-matFadingRange)
                { 
                    blendingValue = 0;
                }
                else
                {
                    //Règle de trois
                    float b = (unitValue-(playerTemperatureStats.MaxTemperature-matFadingRange))/matFadingRange;
                    blendingValue = Mathf.Clamp(b, 0, 1);
                }
            }
            return blendingValue;
        }
    }
}