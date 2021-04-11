using DLMD.PlatformColors;
using UnityEngine;

public class AscendingPlatform : MonoBehaviour
{
    Animator animator;
    public bool skipAnimation;

    // Events
    public delegate void AscendingPlatformEventHandler();

    public event AscendingPlatformEventHandler OnAscended;
    public void Ascended()
    {
        OnAscended?.Invoke();
    }
    
    public event AscendingPlatformEventHandler OnSkipAnimation;
    public void SkipAnimation()
    {
        OnSkipAnimation?.Invoke();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        OnSkipAnimation += () => animator.enabled = false;
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

    private void OnEnable()
    {
        if (!skipAnimation)
            animator.Update(0f);
    }
}
