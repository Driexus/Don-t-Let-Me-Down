using UnityEngine;

public class RobotFace : MonoBehaviour
{
    public bool emptying = false;
    public RectTransform panel;
    public RectTransform image;
    public float timeToEmpty = 15f;

    Vector3 ImagePos;
    float speed;
    float endHeight;
    float startHeight;

    float CurrentHeight { get { return panel.localPosition.y; } }

    private void Start()
    {
        ImagePos = image.position;
        speed = panel.rect.height / timeToEmpty;
        startHeight = panel.localPosition.y;
        endHeight = panel.localPosition.y - panel.rect.height;
    }

    void Update()
    {
        if (emptying)
        {
            panel.localPosition -= Vector3.up * (speed * Time.deltaTime);
            image.position = ImagePos;

            if (panel.localPosition.y <= endHeight)
                OnEmpty();
        }  
        
        if (test)
        {
            AddTime(timetoadd);
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

    public void AddTime(float seconds)
    {
        float percentageFill = seconds / timeToEmpty;
        float heightToAdd = seconds * speed;
        if (CurrentHeight + heightToAdd > startHeight)
            heightToAdd = startHeight - CurrentHeight;

        panel.localPosition += Vector3.up * heightToAdd;
        image.position = ImagePos;
    }
}
