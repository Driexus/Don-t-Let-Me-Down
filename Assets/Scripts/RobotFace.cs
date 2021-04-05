using UnityEngine;
using UnityEngine.UI;


public class RobotFace : MonoBehaviour
{
    public RectTransform panel;
    public Image image;
    public Color color;

    // Empties on update if true
    public bool emptying;

    // Fills on update if true
    public bool filling;

    public float EmptyTime
    {
        set
        {
            emptyTime = value;
            emptyingSpeed = panel.rect.height / emptyTime;
        }
    }
    float emptyTime = 5f;

    public float FillTime
    {
        set
        {
            fillTime = value;
            fillingSpeed = panel.rect.height / fillTime;
        }
    }
    float fillTime = 5f;

    [HideInInspector]
    public float extraTime = 2f;

    // Starting Image Position
    Vector3 ImagePos;
    
    // The speed which the timer depletes ingame
    float emptyingSpeed;

    // The speed which the timer fills on memorization phase
    float fillingSpeed;

    // Panel end height (Y)
    float endHeight;

    // Panel start height (Y)
    float startHeight;

    float CurrentHeight { get { return panel.localPosition.y; } }

    private void Start()
    {
        ImagePos = image.transform.position;
        EmptyTime = emptyTime;
        startHeight = panel.localPosition.y;
        endHeight = panel.localPosition.y - panel.rect.height;

        // Set Image Color and Sprite
        image.color = color;
        image.sprite = GetComponent<Image>().sprite;

        // Stop emptying OnEmpty
        OnEmpty += () => { emptying = false; };

        // Stop filling OnFull
        OnFull += () => { filling = false; };
    }

    void Update()
    {
        if (emptying)
        {
            // Move panel down
            panel.localPosition -= Vector3.up * emptyingSpeed * Time.deltaTime;
            // Reset image position
            image.transform.position = ImagePos;

            // Trigger OnEmpty event if panel has reached its end height position
            if (panel.localPosition.y <= endHeight)
                OnEmpty?.Invoke();
        }

        if (filling)
        {
            // Move panel up
            panel.localPosition += Vector3.up * fillingSpeed * Time.deltaTime;
            // Reset image position
            image.transform.position = ImagePos;

            // Trigger OnFull event if panel panel has reached its start height position
            if (panel.localPosition.y >= startHeight)
                OnFull?.Invoke();
        }
    }

    public delegate void RobotFaceHandler();
    public event RobotFaceHandler OnEmpty;
    public event RobotFaceHandler OnFull;

    // Adds the extra time. If the sum time is more than max it sets to max
    public void AddExtraTime()
    {
        float percentageFill = extraTime / emptyTime;
        // Multiply with emptyingSpeed to find how much height we need to add
        float heightToAdd = extraTime * emptyingSpeed;
        if (CurrentHeight + heightToAdd > startHeight)
            heightToAdd = startHeight - CurrentHeight;

        panel.localPosition += Vector3.up * heightToAdd;
        image.transform.position = ImagePos;
    }

    // Sets timer to max and resets ImagePos
    public void ResetToMax()
    {
        ImagePos = image.transform.position;
        panel.localPosition = new Vector3(panel.localPosition.x, startHeight, panel.localPosition.z);
        image.transform.position = ImagePos;
    }

    // Sets timer to min and resets ImagePos
    public void ResetToMin()
    {
        ImagePos = image.transform.position;
        panel.localPosition -= Vector3.up * (CurrentHeight - endHeight);
        image.transform.position = ImagePos;
    }
}
