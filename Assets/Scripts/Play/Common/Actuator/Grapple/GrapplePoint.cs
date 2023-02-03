using UnityEngine;

//Author: FGCliche
namespace Game
{
    public class GrapplePoint : MonoBehaviour
    {
        [SerializeField] private Color selectedColor = Color.green;
        [SerializeField] private Color unselectedColor = new Color(0, 0, 0, 0.2f);
        
        private SpriteRenderer sprite;
        public Vector2 Position => transform.position;

        private void Awake()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            sprite.color = unselectedColor;
        }

        public void Select()
        {
            sprite.color = selectedColor;
        }

        public void Unselect()
        {
            sprite.color = unselectedColor;
        }
    }
}