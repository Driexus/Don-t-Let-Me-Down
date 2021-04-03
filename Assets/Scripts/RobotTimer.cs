using UnityEngine;

public class RobotTimer : MonoBehaviour
{
    // Array of robotFaces (aka Timers)
    RobotFace[] robotFaces;
    // Current timer
    int robotIndex;

    LevelManager LM;

    private void Awake()
    {       
        // Find Level Manager
        LM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

        // Subscribe LM events
        LM.OnLevelFailed += () => robotFaces[robotIndex].emptying = false;
        LM.OnLevelCompleted += () => robotFaces[robotIndex].emptying = false;

        // Initiate array
        robotFaces = new RobotFace[transform.childCount];
        
        // Initiate robot faces and subscribe events
        for (int i = 0; i < transform.childCount; i++)
        {
            robotFaces[i] = transform.GetChild(i).GetComponent<RobotFace>();
            robotFaces[i].OnEmpty += TimeEnded;
            robotFaces[i].OnEmpty += LM.LevelFailed;
        }
    }
    
    // Sets timer's total time and extra time (used when changing platform)
    public void SetTimer(float totalTime, float extraTime)
    {
        foreach (RobotFace robotFace in robotFaces)
        {
            robotFace.TotalTime = totalTime;
            robotFace.extraTime = extraTime;
        }
    }

    // Called to start the timer
    public void StartTimer()
    {
        robotIndex = 0;
        robotFaces[0].emptying = true;
    }

    // Jumps to the next timer
    public void NextTimer()
    {
        robotFaces[robotIndex].emptying = false;
        robotFaces[robotIndex].AddExtraTime();
        
        robotIndex++;
        if (robotIndex >= robotFaces.Length)
            robotIndex = 0;

        robotFaces[robotIndex].emptying = true;
    }

    // OnTimeEnded event
    public delegate void timerHandler();
    public event timerHandler OnTimeEnded;

    public void TimeEnded()
    {
        Debug.Log("Out of Time");
        OnTimeEnded?.Invoke();
    }
}
