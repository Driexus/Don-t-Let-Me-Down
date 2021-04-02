using UnityEngine;

public class RobotTimer : MonoBehaviour
{
    RobotFace[] robotFaces;
    int robotIndex;

    private void Awake()
    {
        robotFaces = new RobotFace[transform.childCount];
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            robotFaces[i] = transform.GetChild(i).GetComponent<RobotFace>();  
        }
    }
    public void SetTimer(float totalTime, float extraTime)
    {
        foreach (RobotFace robotFace in robotFaces)
        {
            robotFace.totalTime = totalTime;
            robotFace.extraTime = extraTime;
        }
    }

    public void StartTimer()
    {
        robotIndex = 0;
        robotFaces[0].emptying = true;
    }

    public void NextTimer()
    {
        robotFaces[robotIndex].emptying = false;
        robotFaces[robotIndex].AddExtraTime();
        
        robotIndex++;
        if (robotIndex > robotFaces.Length)
            robotIndex = 0;

        robotFaces[robotIndex].emptying = true;
    }

    public void OnTimeEnded()
    {

    }
}
