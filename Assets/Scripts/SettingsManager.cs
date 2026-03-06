using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Références")]
    public AudioMixer mainMixer;
    public GameObject settingsPanel;

    [Header("Sliders")]
    public Slider volumeSlider;
    public Slider sensitivitySlider;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("SavedVolume", 0.75f);
        float savedSens = PlayerPrefs.GetFloat("SavedSensitivity", 0.2f);

        if (volumeSlider != null) volumeSlider.value = savedVolume;
        if (sensitivitySlider != null) sensitivitySlider.value = savedSens;

        SetVolume(savedVolume);
        SetSensitivity(savedSens);

        settingsPanel.SetActive(false);
    }

    public void SetVolume(float sliderValue)
    {
        float mixValue = Mathf.Log10(sliderValue) * 20f;

        if (mainMixer != null)
        {
            mainMixer.SetFloat("MasterVolume", mixValue);
        }

        PlayerPrefs.SetFloat("SavedVolume", sliderValue);
    }

    public void SetSensitivity(float sliderValue)
    {
        PlayerPrefs.SetFloat("SavedSensitivity", sliderValue);

        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.mouseSensitivity = sliderValue;
        }
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        PlayerPrefs.Save(); 
    }
}