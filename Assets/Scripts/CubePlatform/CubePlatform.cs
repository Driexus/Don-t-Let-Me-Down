using UnityEngine;
using DLMD.PlatformColors;

public class CubePlatform : MonoBehaviour
{
    AscendingPlatform ascendingPlatfom;
    Animator animator;
    Renderer rend;

    Vector3 startPos;
    PlatformColor color;

    public static Color yellow = new Color32(255, 199, 138, 255);
    public static Color green = new Color32(86, 219, 69, 255);
    public static Color blue = new Color32(61, 192, 243, 255);
    public static Color red = new Color32(214, 47, 51, 255);

    private void Awake()
    {
        startPos = transform.localPosition;
        animator = GetComponent<Animator>();
        rend = GetComponent<Renderer>();
        ascendingPlatfom = transform.parent.parent.gameObject.GetComponent<AscendingPlatform>();

        ascendingPlatfom.OnAscended += () => animator.enabled = true;
        ascendingPlatfom.OnSkipAnimation += () => gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        rend.enabled = true;
        animator.enabled = false;
        transform.localPosition = startPos;
        ResetColor();
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

    public void DisableRenderer()
    {
        rend.enabled = false;
    }
}
