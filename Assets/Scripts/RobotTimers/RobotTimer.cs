﻿using UnityEngine;

public class RobotTimer : MonoBehaviour
{
    // Array of robotFaces (aka Timers)
    RobotFace[] robotFaces;
    // Current timer
    int robotIndex;

    // True when StartTimer is called, false when StopTimer is Called
    bool isTimerActive;

    GameManager GM;
    LevelManager LM;

    private void Awake()
    {       
        // Find Managers
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        LM = GM.lm;
    }
    
    // Sets timer's total time, extra time (used when changing platform) and memorization time
    public void SetTimer(float totalTime, float extraTime, float memorizationTime)
    {
        // Subscribe GM events
        GM.OnMemorizationPhaseStarted += StartFilling;
        GM.OnMemorizationPhaseEnded += StopFilling;

        // Subscribe LM events
        LM.OnLevelFailed += StopTimer;
        LM.OnLevelCompleted += StopTimer;

        // Initiate array
        robotFaces = new RobotFace[transform.childCount];

        // Initiate robot faces and subscribe events
        for (int i = 0; i < transform.childCount; i++)
        {
            robotFaces[i] = transform.GetChild(i).GetComponent<RobotFace>();
            robotFaces[i].OnEmpty += LM.LevelFailed;
        }

        foreach (RobotFace robotFace in robotFaces)
        {
            robotFace.emptyTime = totalTime;
            robotFace.extraTime = extraTime;
            robotFace.fillTime = memorizationTime;
        }
    }

    // Called to start the timer
    public void StartTimer()
    {
        isTimerActive = true;
        robotIndex = 0;
        robotFaces[0].emptying = true;
    }

    // Called to stop the timer
    public void StopTimer()
    {
        robotFaces[robotIndex].emptying = false;
        isTimerActive = false;
    }

    // Jumps to the next timer
    public void NextTimer()
    {
        // If timer is not active return
        if (!isTimerActive)
            return;

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

    // Pauses the current timer
    public void PauseTimer()
    {
        if (isTimerActive)
            robotFaces[robotIndex].emptying = false;
    }

    // Resumes the current timer
    public void ResumeTimer()
    {
        if (isTimerActive)
            robotFaces[robotIndex].emptying = true;
    }
}