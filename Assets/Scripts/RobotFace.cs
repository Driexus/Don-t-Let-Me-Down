using UnityEngine;
using UnityEngine.UI;


public class RobotFace : MonoBehaviour
{
    public RectTransform panel;
    public Image image;
    public Color color;

    // Empties on update if true
    public bool emptying;

    [HideInInspector]
    public float totalTime = 15f;
    [HideInInspector]
    public float extraTime = 5f;

    // Starting Image Position
    Vector3 ImagePos;
    
    // Emptying speed
    float speed;

    // Panel end height (Y)
    float endHeight;

    // Panel start height (Y)
    float startHeight;

    float CurrentHeight { get { return panel.localPosition.y; } }

    private void Start()
    {
        ImagePos = image.transform.position;
        speed = panel.rect.height / totalTime;
        startHeight = panel.localPosition.y;
        endHeight = panel.localPosition.y - panel.rect.height;

        // Set Image Color and Sprite
        image.color = color;
        image.sprite = GetComponent<Image>().sprite;
    }

    void Update()
    {
        if (emptying)
        {
            // Move panel down
            panel.localPosition -= Vector3.up * speed * Time.deltaTime;
            // Reset image position
            image.transform.position = ImagePos;

            // Trigger OnEmpty event if panel has reached its end height postion
            if (panel.localPosition.y <= endHeight)
                OnEmpty();
        }  
        
        if (test)
        {
            AddExtraTime();
            test = false;
        }    

    }

    public bool test;
    public float timetoadd = 2f;

    void OnEmpty()
    {
        emptying = false;
        Debug.Log("empty");
    }

    // Adds the extra time. If the sum time is more than max it sets to max
    public void AddExtraTime()
    {
        float percentageFill = extraTime / totalTime;
        float heightToAdd = extraTime * speed;
        if (CurrentHeight + heightToAdd > startHeight)
            heightToAdd = startHeight - CurrentHeight;

        panel.localPosition += Vector3.up * heightToAdd;
        image.transform.position = ImagePos;
    }
}
