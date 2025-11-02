using UnityEngine;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    public static BrightnessManager Instance;

    [SerializeField] private Image brightnessOverlay; // UI overlay to control brightness

    private float brightnessValue = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Load saved brightness
        brightnessValue = PlayerPrefs.GetFloat("Brightness", 0.5f);
        ApplyBrightness(brightnessValue);
    }

    public void SetBrightness(float value)
    {
        brightnessValue = value;
        ApplyBrightness(value);
        PlayerPrefs.SetFloat("Brightness", value);
    }

    private void ApplyBrightness(float value)
    {
        if (brightnessOverlay != null)
        {
            Color c = brightnessOverlay.color;
            c.a = 1f - value; // Lower value = darker
            brightnessOverlay.color = c;
        }
    }
}