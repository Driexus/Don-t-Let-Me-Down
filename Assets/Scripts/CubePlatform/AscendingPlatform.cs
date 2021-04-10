using DLMD.PlatformColors;
using UnityEngine;

public class AscendingPlatform : MonoBehaviour
{
    public delegate void CubePlatformEventHandler();
    public event CubePlatformEventHandler OnAscended;


    public void Ascended()
    {
        OnAscended?.Invoke();
    }

    public void SetPlatformColor(PlatformColor color)
    { 
        CubeLights[] cubeLights = GetComponentsInChildren<CubeLights>();
        CubePlatform[] cubePlatforms = GetComponentsInChildren<CubePlatform>();
        foreach (CubePlatform cubePlatform in cubePlatforms)
            cubePlatform.SetPlatformColor(color);

        foreach (CubeLights cubeLight in cubeLights)
            cubeLight.SetPlatformColor(color);
    }
}
