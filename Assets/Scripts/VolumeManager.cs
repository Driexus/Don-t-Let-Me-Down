using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle volumeToggle;
    private void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener((value) => ChangeSliderVolume(value));
            volumeSlider.value = PlayerPrefs.GetFloat("SliderVolume", 1f);
        }

        if (volumeToggle != null)
        {
            volumeToggle.onValueChanged.AddListener((value) => ChangeToggleVolume(value));

            if (PlayerPrefs.GetInt("ToggleVolume", 1) == 0)
                volumeToggle.isOn = false;
            else
                volumeToggle.isOn = true;
        }
    }

    void ChangeSliderVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("SliderVolume", newVolume);
        SetVolume();
    }

    void ChangeToggleVolume(bool enabled)
    {
        if (enabled)
            PlayerPrefs.SetInt("ToggleVolume", 1);
        else
            PlayerPrefs.SetInt("ToggleVolume", 0);
        SetVolume();
    }

    void SetVolume()
    {
        float newVolume = 1f;
        
        if (PlayerPrefs.HasKey("SliderVolume"))
            newVolume = PlayerPrefs.GetFloat("SliderVolume");

        if (PlayerPrefs.HasKey("ToggleVolume"))
            newVolume *= (float)PlayerPrefs.GetInt("ToggleVolume");

        AudioListener.volume = newVolume;
    }
}
