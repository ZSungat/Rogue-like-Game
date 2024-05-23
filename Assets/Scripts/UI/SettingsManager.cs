using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public TMP_Dropdown graphicsDropdown;
    public Slider MasterSlider, MusicSlider, SFXSlider;
    public AudioMixer MainAudioMixer;

    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";
    private const string GraphicsQualityKey = "GraphicsQuality";

    private void Start()
    {
        // Initialize graphics quality dropdown
        graphicsDropdown.value = PlayerPrefs.GetInt(GraphicsQualityKey, QualitySettings.GetQualityLevel());
        graphicsDropdown.RefreshShownValue();
        graphicsDropdown.onValueChanged.AddListener(delegate { ChangedGraphicsQuality(); });

        // Initialize sliders
        MasterSlider.value = PlayerPrefs.GetFloat(MasterVolumeKey, GetAudioMixerVolume("MasterVolume"));
        MasterSlider.onValueChanged.AddListener(delegate { ChangedMasterVolume(); });

        MusicSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey, GetAudioMixerVolume("MusicVolume"));
        MusicSlider.onValueChanged.AddListener(delegate { ChangedMusicVolume(); });

        SFXSlider.value = PlayerPrefs.GetFloat(SFXVolumeKey, GetAudioMixerVolume("SFXVolume"));
        SFXSlider.onValueChanged.AddListener(delegate { ChangedSFXVolume(); });

        // Apply initial audio mixer values
        MainAudioMixer.SetFloat("MasterVolume", MasterSlider.value);
        MainAudioMixer.SetFloat("MusicVolume", MusicSlider.value);
        MainAudioMixer.SetFloat("SFXVolume", SFXSlider.value);
    }

    private float GetAudioMixerVolume(string parameter)
    {
        float value;
        if (MainAudioMixer.GetFloat(parameter, out value))
        {
            return value;
        }
        return 0f;
    }

    public void ChangedGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
        PlayerPrefs.SetInt(GraphicsQualityKey, graphicsDropdown.value);
        PlayerPrefs.Save();
    }

    public void ChangedMasterVolume()
    {
        MainAudioMixer.SetFloat("MasterVolume", MasterSlider.value);
        PlayerPrefs.SetFloat(MasterVolumeKey, MasterSlider.value);
        PlayerPrefs.Save();
    }

    public void ChangedMusicVolume()
    {
        MainAudioMixer.SetFloat("MusicVolume", MusicSlider.value);
        PlayerPrefs.SetFloat(MusicVolumeKey, MusicSlider.value);
        PlayerPrefs.Save();
    }

    public void ChangedSFXVolume()
    {
        MainAudioMixer.SetFloat("SFXVolume", SFXSlider.value);
        PlayerPrefs.SetFloat(SFXVolumeKey, SFXSlider.value);
        PlayerPrefs.Save();
    }
}
