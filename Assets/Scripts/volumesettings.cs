using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class volumesettings : MonoBehaviour
{
    [SerializeField] private AudioMixer MyMixer;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void SetMusicVolume()
    {
        if (MusicSlider != null && MyMixer != null)
        {
            float volume = MusicSlider.value;
            MyMixer.SetFloat("Music", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
            PlayerPrefs.SetFloat("musicVolume", volume);
        }
    }

    public void SetSFXVolume()
    {
        if (SFXSlider != null && MyMixer != null)
        {
            float volume = SFXSlider.value;
            MyMixer.SetFloat("SFX", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
            PlayerPrefs.SetFloat("sfxVolume", volume);
        }
    }

    private void LoadVolume()
    {
        if (MusicSlider != null)
        {
            MusicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
            SetMusicVolume();
        }

        if (SFXSlider != null)
        {
            SFXSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);
            SetSFXVolume();
        }
    }
}