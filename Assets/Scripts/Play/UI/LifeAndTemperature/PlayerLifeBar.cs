using UnityEngine;

namespace Game
{
    // Author : Felix.B
    public class PlayerLifeBar : MonoBehaviour
    {
        [Header("Colors")]
        [SerializeField] private Color criticalStateBarColor;
        [SerializeField] private Color fullLifeBarColor;
        [SerializeField] private float upperColorTransitionValue = 0.55f;
        [SerializeField] private float lowerColorTransitionValue = 0.45f;
        
        private LiquidBar bar;
        
        private void Awake()
        {
            bar = GetComponent<LiquidBar>();
        }
        public void OnPlayerLifeModified(Player player)
        {
            bar.targetFillAmount = player.GetLifePoints();

            if (player.GetLifePoints() > upperColorTransitionValue) 
                bar.barColor = fullLifeBarColor;
            else if (player.GetLifePoints() < lowerColorTransitionValue)
                bar.barColor = criticalStateBarColor;
            else
            {
                float unitValue = player.GetLifePoints() / player.GetInitialLifePoint();
                //Règle de trois
                float a = unitValue * (1 - lowerColorTransitionValue);
                float b = unitValue * upperColorTransitionValue;
                //Moyenne
                float c = (a + b) / 2;
                float blendingValue = Mathf.Clamp(c,0 , 1 );
                
                bar.barColor = criticalStateBarColor.Blend(fullLifeBarColor, blendingValue);   
            }
        }
    }
}