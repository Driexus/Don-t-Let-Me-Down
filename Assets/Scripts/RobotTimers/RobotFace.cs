using UnityEngine;
using UnityEngine.UI;


public class RobotFace : MonoBehaviour
{
    // Child image which uses fill
    public Image image;

    // Empties on update if true
    public bool emptying;

    // Fills on update if true
    public bool filling;

    [HideInInspector]
    public float emptyTime = 5f;
    [HideInInspector]
    public float fillTime = 5f;
    [HideInInspector]
    public float extraTime = 2f;

    // Events
    public delegate void RobotFaceHandler();
    public event RobotFaceHandler OnEmpty;
    public event RobotFaceHandler OnFull;

    void Update()
    {
        if (emptying)
        {
            image.fillAmount -= Time.deltaTime / emptyTime;

            // Trigger OnEmpty event if panel has reached its end height position
            if (image.fillAmount <= 0)
            {
                emptying = false;
                image.fillAmount = 0;
                OnEmpty?.Invoke();
            }

        }

        if (filling)
        {
            image.fillAmount += Time.deltaTime / fillTime;

            // Trigger OnFull event if panel panel has reached its start height position
            if (image.fillAmount >= 1)
            {
                filling = false;
                image.fillAmount = 1;
                OnFull?.Invoke();
            }
        }
    }

    // Adds the extra time. If the sum time is more than max it sets to max
    public void AddExtraTime()
    {
        float fillToAdd = extraTime / fillTime;
        if (image.fillAmount + fillToAdd > 1)
            image.fillAmount = 1;
        else
            image.fillAmount += fillToAdd;
    }

    // Sets timer to max and resets ImagePos
    public void ResetToMax()
    {
        image.fillAmount = 1;
    }

    // Sets timer to min and resets ImagePos
    public void ResetToMin()
    {
        image.fillAmount = 0;
    }
}
