using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Saver
{
    public static void SaveData(int level)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.dlmd";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(level);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadData()
    {
        string path = Application.persistentDataPath + "/save.dlmd";
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
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
