using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    //Author : William Lemelin
    public class AchievementView : MonoBehaviour
    {
        [SerializeField] private Text title;
        [SerializeField] private Text description;
        [SerializeField] private Image image;
        [SerializeField] private Image background;
        
        private Sprite images;
        private bool isUnlocked;

        public string Title
        {
            set => title.text = value;
        }
        public string Description
        {
            set => description.text = value;
        }
        public Sprite Image
        {
            set => image.sprite = value;
        }
        public bool IsUnlocked
        {
            get => isUnlocked;
            set => isUnlocked = value;
        }

        private void Start()
        {
            if (IsUnlocked)
            {
                background.color = Color.green;
            }
        }
    }
}