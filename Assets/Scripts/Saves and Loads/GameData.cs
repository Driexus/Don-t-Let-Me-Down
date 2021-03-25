using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int level = 0;

    public GameData(int currentLevel)
    {
        level = currentLevel;
    }
}
