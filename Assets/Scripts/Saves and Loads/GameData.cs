[System.Serializable]
public class GameData
{
    public int level = 1;
    public int highscoreLevel = 1;
    public int letdowns;
    public int totalLetdowns;
    public int highscore = -1;

    public enum DataType { level, highscoreLevel, letdowns, totalLetdowns, highscore}

    public GameData(int _level, int _highscoreLevel, int _letdowns, int _totalLetdowns, int _highscore)
    {
        level = _level;
        highscoreLevel = _highscoreLevel;
        letdowns = _letdowns;
        totalLetdowns = _totalLetdowns;
        highscore = _highscore;
    }

    public GameData()
    {
    }

    public void SetDataValue(DataType type, int value)
    {
        if (type == DataType.level)
            level = value;
        else if (type == DataType.highscoreLevel)
            highscoreLevel = value;
        else if (type == DataType.letdowns)
            letdowns = value;
        else if (type == DataType.totalLetdowns)
            totalLetdowns = value;
        else if (type == DataType.highscore)
            highscore = value;
    }

    public int GetDataValue(DataType type)
    {
        if (type == DataType.level)
            return level;
        else if (type == DataType.highscoreLevel)
            return highscoreLevel;
        else if (type == DataType.letdowns)
            return letdowns;
        else if (type == DataType.totalLetdowns)
            return totalLetdowns;
        else if (type == DataType.highscore)
            return highscore;

        return -1;
    }
}
