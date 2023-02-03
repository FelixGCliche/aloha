using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    //Author: Felix.B
    public class DeepWater : MonoBehaviour
    {
        [SerializeField] private FreezableWaterSource relatedWaterSource = null;

        private new TilemapRenderer renderer;

        private void Awake()
        {
            renderer = GetComponent<TilemapRenderer>();
        }

        public void Update()
        {
            renderer.material = relatedWaterSource.InUseMaterial;
        }
    }
}