using UnityEngine;

public class CubeFlicker : MonoBehaviour
{
    Renderer rend;
    Color startColor;

    float counter;
    public float flickerDuration = 0.1f;
    bool isOff;

    public float minimumFlickerTime = 0.1f;
    public float maximumFlickerTime = 0.3f;

    float flickerTime;

    private void Awake()
    {      
        rend = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        startColor = rend.material.GetColor("_EmissionColor");
        counter = 0f;

        flickerTime = Random.Range(minimumFlickerTime, maximumFlickerTime);
    }

    private void Update()
    {            
        counter += Time.deltaTime;
        if (isOff && counter >= flickerDuration)
        {
            rend.material.SetColor("_EmissionColor", startColor);
            isOff = !isOff;
            counter = 0f;
        }

        else if (!isOff && counter >= flickerTime)
        {        
            rend.material.SetColor("_EmissionColor", Color.black);
            isOff = !isOff;
            counter = 0f;
            flickerTime = Random.Range(minimumFlickerTime, maximumFlickerTime);
        }
    }
}
