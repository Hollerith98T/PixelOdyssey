using UnityEngine;
using UnityEngine.UI;

public class SettingSystem : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private GameObject settingsPanel;

    private void Start()
    {
        if (volumeSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        
        if (fullscreenToggle != null)
        {
            bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
            fullscreenToggle.isOn = isFullscreen;
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        }
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void EnableSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    public void DisableSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
}