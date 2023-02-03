using Harmony;
using UnityEngine;

namespace Game
{
    //Author : William Lemelin
    [Findable(Tags.GameController)]
    public class OnAchievementUnlockedEventChannel : MonoBehaviour
    {
        public event OnAchievementUnlockedEvent OnAchivementUnlocked;

        public void Publish(AchievementType achievement)
        {
            OnAchivementUnlocked?.Invoke(achievement);
        }
    }

    public delegate void OnAchievementUnlockedEvent(AchievementType achievement);
}