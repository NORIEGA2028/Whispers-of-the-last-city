using UnityEngine;
using UnityEngine.UI;

public class PauseSettings : MonoBehaviour
{
    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider brightnessSlider;

    private MusicManager musicManager;

    void Start()
    {
        // Find the existing MusicManager in the scene
        musicManager = Object.FindFirstObjectByType<MusicManager>();

        // Load saved values
        float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0.5f);

        // Set sliders
        if (musicSlider) musicSlider.value = savedMusic;
        if (sfxSlider) sfxSlider.value = savedSFX;
        if (brightnessSlider) brightnessSlider.value = savedBrightness;

        // Add listeners
        if (musicSlider) musicSlider.onValueChanged.AddListener(OnMusicChange);
        if (sfxSlider) sfxSlider.onValueChanged.AddListener(OnSFXChange);
        if (brightnessSlider) brightnessSlider.onValueChanged.AddListener(OnBrightnessChange);

        ApplyBrightness(savedBrightness);
    }

    void OnMusicChange(float value)
    {
        if (musicManager) musicManager.SetMusicVolume(value);
    }

    void OnSFXChange(float value)
    {
        if (musicManager) musicManager.SetSFXVolume(value);
    }

    void OnBrightnessChange(float value)
    {
        ApplyBrightness(value);
        PlayerPrefs.SetFloat("Brightness", value);
    }

    void ApplyBrightness(float value)
    {
        // Simple brightness using a dark overlay (recommended way)
        RenderSettings.ambientLight = Color.white * Mathf.Clamp(value + 0.3f, 0.3f, 1f);
    }
}