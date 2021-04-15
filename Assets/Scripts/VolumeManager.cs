using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public Slider volumeSlider;
    private void Start()
    {
        volumeSlider.onValueChanged.AddListener((value) => ChangeVolume(value));
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
    }

    public void ChangeVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("volume", newVolume);
        AudioListener.volume = newVolume;
    }
}
