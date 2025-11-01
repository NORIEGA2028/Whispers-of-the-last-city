using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettings : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private MusicManager musicManager;

    void Start()
    {
        musicManager = Object.FindFirstObjectByType<MusicManager>();

        // Load saved values from MusicManager’s keys
        float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        musicSlider.value = savedMusic;
        sfxSlider.value = savedSFX;

        // Apply those values immediately
        ApplyMusicVolume(savedMusic);
        ApplySFXVolume(savedSFX);

        // Add listeners to sliders
        musicSlider.onValueChanged.AddListener(ApplyMusicVolume);
        sfxSlider.onValueChanged.AddListener(ApplySFXVolume);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ApplyMusicVolume(float value)
    {
        if (musicManager != null)
        {
            musicManager.SetMusicVolume(value);
            PlayerPrefs.SetFloat("MusicVolume", value);
        }
    }

    public void ApplySFXVolume(float value)
    {
        if (musicManager != null)
        {
            musicManager.SetSFXVolume(value);
            PlayerPrefs.SetFloat("SFXVolume", value);
        }
    }
}