using UnityEngine;

public class test : MonoBehaviour
{
    public int level;
    
    public void SaveLevel()
    {
        Saver.SaveData(level);
    }

    public void LoadLevel()
    {
        GameData data = Saver.LoadData();
        Debug.Log(data.level);
        GameSceneData.Level = data.level;
    }
}
