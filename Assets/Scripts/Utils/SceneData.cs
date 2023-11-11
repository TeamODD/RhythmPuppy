namespace SceneData
{
    public enum SceneName
    {
        MAINMENU,
        STAGEMENU,
        TUTORIAL,
        STAGE1_1,
        STAGE1_2,
        STAGE2_1,
        GAMEOVER,
        CREDIT,
        OPTION,
    }

    public static class SceneInfo
    {
        public static string getSceneName(SceneName name)
        {
            switch (name)
            {
                case SceneName.MAINMENU:
                    return "";
                case SceneName.STAGEMENU:
                    return "SceneMenu_01";
                case SceneName.TUTORIAL:
                    return "";
                case SceneName.STAGE1_1:
                    return "SceneStage1_1";
                case SceneName.STAGE1_2:
                    return "SceneStage1_2";
                case SceneName.STAGE2_1:
                    return "SceneStage2_1";
                case SceneName.GAMEOVER:
                    return "GameOver";
                case SceneName.CREDIT:
                    return "Credit";
                case SceneName.OPTION:
                    return "Option_Stage";
            }
            return "";
        }
    }
}