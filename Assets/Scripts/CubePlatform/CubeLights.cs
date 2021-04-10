using UnityEngine;
using DLMD.PlatformColors;

public class CubeLights : MonoBehaviour
{
    AscendingPlatform ascendingPlatform;
    Renderer rend;

    PlatformColor color;

    public static Color green = new Color32(0, 255, 5, 255);
    public static Color blue = new Color32(0, 216, 255, 255);
    public static Color red = new Color32(255, 0, 0, 255);
    public static Color yellow = new Color32(255, 222, 0, 255);

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        ascendingPlatform = transform.parent.parent.gameObject.GetComponent<AscendingPlatform>();
    }

    private void OnEnable()
    {
        rend.material.SetColor("_EmissionColor", Color.black);
    }

    void Start()
    {
        ascendingPlatform.OnAscended += () => ResetColor();
    }


    public void ResetColor()
    {
        if (color == PlatformColor.red)
            rend.material.SetColor("_EmissionColor", red);
        else if (color == PlatformColor.green)
            rend.material.SetColor("_EmissionColor", green);
        else if (color == PlatformColor.yellow)
            rend.material.SetColor("_EmissionColor", yellow);
        else if (color == PlatformColor.blue)
            rend.material.SetColor("_EmissionColor", blue);
    }

    public void SetPlatformColor(PlatformColor pColor)
    {
        color = pColor;
    }
}
