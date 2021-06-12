using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public string musicPreferenceKey;
    public string musicParameterKey;
    public string sfxPreferenceKey;
    public string sfxParameterKey;

    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Awake()
    {
        SetAudioVolume(musicParameterKey, PlayerPrefs.GetFloat(musicPreferenceKey, 1f));
        SetAudioVolume(sfxParameterKey, PlayerPrefs.GetFloat(sfxPreferenceKey, 1f));

        if (musicVolumeSlider) musicVolumeSlider.value = PlayerPrefs.GetFloat(musicPreferenceKey, 1f);
        if (sfxVolumeSlider) sfxVolumeSlider.value = PlayerPrefs.GetFloat(sfxPreferenceKey, 1f);
    }

    public void SetMusic(float volume)
    {
        SetAudioVolume(musicParameterKey, volume);
        PlayerPrefs.SetFloat(musicPreferenceKey, volume);
        PlayerPrefs.Save();
    }

    public void SetSFX(float volume)
    {
        SetAudioVolume(sfxParameterKey, volume);
        PlayerPrefs.SetFloat(sfxPreferenceKey, volume);
        PlayerPrefs.Save();
    }

    private void SetAudioVolume(string key, float volume)
    {
        if (volume != 0)
            audioMixer.SetFloat(key, 80.0f * Mathf.Log(volume, 20));
        else
            audioMixer.SetFloat(key, -140f);
    }
}
