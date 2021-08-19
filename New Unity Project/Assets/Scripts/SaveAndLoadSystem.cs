using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveAndLoadSystem
{
    public static void SaveData(GameDataController gameDataController)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/ball.fun";

        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(gameDataController);

        formatter.Serialize(stream, data);

        stream.Close();
    }
    public static GameData LoadData()
    {
        string path = Application.persistentDataPath + "/ball.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }
}
