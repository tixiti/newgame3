using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    
    public int currentLevel;
    public List<LevelData> levelData;
    

    public GameData(GameDataController gdc)
    {
        levelData = gdc.levelData;
        currentLevel = gdc.currentLevel;
    }
}
