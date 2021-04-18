using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Saver
{
    static string path = Application.persistentDataPath + "/save.dlmd";

    public static void SaveData(GameData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        GameData data;

        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            data = formatter.Deserialize(stream) as GameData;
            stream.Close();
        }
        else
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            data = new GameData();
            formatter.Serialize(stream, data);
            stream.Close();
        }

        return data;
    }
}
