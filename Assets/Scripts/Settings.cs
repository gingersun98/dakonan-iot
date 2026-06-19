using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Sliders - Music")]
    public Slider musicSlider;
    public TextMeshProUGUI musicDisplayText;

    [Header("Sliders - SFX")]
    public Slider SFXSlider;
    public TextMeshProUGUI sfxDisplayText;

    private void OnEnable()
    {
        musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("Music", 1));
        SFXSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SFX", 1));
    }

    public void SetVolume(bool isMusic)
    {
        if (isMusic)
        {
            PlayerPrefs.SetFloat("Music", musicSlider.value);
            if (musicDisplayText != null)
            musicDisplayText.text = ((int)(musicSlider.value * 100)).ToString();
        } else
        {
            PlayerPrefs.SetFloat("SFX", SFXSlider.value);
            if (sfxDisplayText != null)
            sfxDisplayText.text = ((int)(SFXSlider.value * 100)).ToString();
        }
    }
}
