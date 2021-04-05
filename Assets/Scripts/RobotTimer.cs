using UnityEngine;

public class RobotTimer : MonoBehaviour
{
    // Array of robotFaces (aka Timers)
    RobotFace[] robotFaces;
    // Current timer
    int robotIndex;

    GameManager GM;
    LevelManager LM;

    private void Awake()
    {       
        // Find Managers
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        LM = GM.lm;

        // Subscribe GM events
        GM.OnMemorizationPhaseStarted += StartFilling;
        GM.OnMemorizationPhaseEnded += StopFilling;

        // Subscribe LM events
        LM.OnLevelFailed += () => robotFaces[robotIndex].emptying = false;
        LM.OnLevelCompleted += () => robotFaces[robotIndex].emptying = false;

        // Initiate array
        robotFaces = new RobotFace[transform.childCount];
        
        // Initiate robot faces and subscribe events
        for (int i = 0; i < transform.childCount; i++)
        {
            robotFaces[i] = transform.GetChild(i).GetComponent<RobotFace>();
            robotFaces[i].OnEmpty += LM.LevelFailed;
        }
    }
    
    // Sets timer's total time, extra time (used when changing platform) and memorization time
    public void SetTimer(float totalTime, float extraTime, float memorizationTime)
    {
        foreach (RobotFace robotFace in robotFaces)
        {
            robotFace.EmptyTime = totalTime;
            robotFace.extraTime = extraTime;
            robotFace.FillTime = memorizationTime;
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

    // Start filling all robot faces
    public void StartFilling()
    {
        foreach (RobotFace rf in robotFaces)
        {
            rf.ResetToMin();
            rf.filling = true;
        }
    }

    // Stop filling all robot faces and reset them to max
    public void StopFilling()
    {
        foreach(RobotFace rf in robotFaces)
        {
            rf.filling = false;
            rf.ResetToMax();
        }
    }
}
