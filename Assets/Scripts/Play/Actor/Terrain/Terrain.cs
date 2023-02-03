using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    //Author : Felix.B
    public class Terrain : ObjectTemperature
    {
        private new TilemapRenderer renderer;
        
        protected override Renderer Sprite => renderer;
        
        protected override void Awake()
        {
            base.Awake();
            renderer = GetComponent<TilemapRenderer>();
        }
    }
}