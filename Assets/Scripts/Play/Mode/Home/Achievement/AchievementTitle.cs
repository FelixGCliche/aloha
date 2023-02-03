using Harmony;
using UnityEngine;

namespace Game
{
    //Author: William Lemelin
    [Findable(Tags.GameController)]
    public class AchievementTitle : MonoBehaviour
    {
        [SerializeField] private string tutorial = "Tutoriel Complété";
        [SerializeField] private string level1 = "Niveau 1 Complété";
        [SerializeField] private string level2 = "Niveau 2 Complété";
        [SerializeField] private string level3 = "Niveau 3 Complété";
        [SerializeField] private string levelFire = "Niveau de feu Complété";
        [SerializeField] private string levelIce = "Niveau de glace Complété";
        [SerializeField] private string doubleJump = "Icarus";
        [SerializeField] private string freeze = "Do you want to build an Ice Cube?";
        [SerializeField] private string grapple = "GET OVER HERE!";
        [SerializeField] private string geyser = "Surfer Boy";
        [SerializeField] private string breakDoor = "DOOOOOOMFIST";
        [SerializeField] private string dying = "Mr. Stark, I don't feel so good";
        
        public string Title(AchievementType type)
        {
            switch (type)
            {
                case AchievementType.Tutorial:
                    return tutorial;
                case AchievementType.Level1:
                    return level1;
                case AchievementType.Level2:
                    return level2;
                case AchievementType.Level3:
                    return level3;
                case AchievementType.LevelFire:
                    return levelFire;
                case AchievementType.LevelIce:
                    return levelIce;
                case AchievementType.DoubleJump:
                    return doubleJump;
                case AchievementType.Freeze:
                    return freeze;
                case AchievementType.Grapple:
                    return grapple;
                case AchievementType.Geyser:
                    return geyser;
                case AchievementType.BreakDoor:
                    return breakDoor;
                case AchievementType.Dying:
                    return dying;
            }

            return null;
        }
    }
}