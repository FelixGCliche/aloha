namespace Game
{
    //Author: William Lemelin
    public static class AchievementTypeExtensions
    {
        public static AchievementType ConvertArtefactTypeToAchievementType(this ArtefactType artefactType)
        {
            switch (artefactType)
            {
                case ArtefactType.Geyser:
                {
                    return AchievementType.Geyser;
                }
                case ArtefactType.Grapple:
                {
                    return AchievementType.Grapple;
                }
                case ArtefactType.Freeze:
                {
                    return AchievementType.Freeze;
                }
                case ArtefactType.DashBreak:
                {
                    return AchievementType.BreakDoor;
                }
                case ArtefactType.DoubleJump:
                {
                    return AchievementType.DoubleJump;
                }
            }

            return AchievementType.Null;
        }

        public static AchievementType ConvertSceneNameToAchievementType(this SceneName sceneName)
        {
            switch (sceneName)
            {
                case SceneName.Tutorial:
                {
                    return AchievementType.Tutorial;
                }
                case SceneName.Level1:
                {
                    return AchievementType.Level1;
                }
                case SceneName.Level2:
                {
                    return AchievementType.Level2;
                }
                case SceneName.Level3:
                {
                    return AchievementType.Level3;
                }
                case SceneName.LevelFire:
                {
                    return AchievementType.LevelFire;
                }
                case SceneName.LevelIce:
                {
                    return AchievementType.LevelIce;
                }
            }

            return AchievementType.Null;
        }
    }
}