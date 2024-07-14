using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioMixer _mixer;

    public Slider _musicSlider;
    public Slider _sfxSlider;
    public Slider _sensitivitySlider;
    public Slider _ambienceSlider;

    private void Start()
    {
        _mixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        _mixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));
        _mixer.SetFloat("AmbienceVol", PlayerPrefs.GetFloat("AmbienceVol"));

        _musicSlider.value = PlayerPrefs.GetFloat("MusicVol");
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVol");
        _ambienceSlider.value = PlayerPrefs.GetFloat("AmbienceVol");
        _sensitivitySlider.value = PlayerPrefs.GetFloat("SensitivityMultiplier");

        QualitySettings.SetQualityLevel((int)PlayerPrefs.GetInt("Quality"));

        gameObject.SetActive(false);
    }

    public void ChangeMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVol", _musicSlider.value);
        _mixer.SetFloat("MusicVol", _musicSlider.value);
    }
    public void ChangeSFXVolume()
    {
        PlayerPrefs.SetFloat("SFXVol", _sfxSlider.value);
        _mixer.SetFloat("SFXVol", _sfxSlider.value);
    }
    public void ChangeAmbienceVolume()
    {
        PlayerPrefs.SetFloat("AmbienceVol", _ambienceSlider.value);
        _mixer.SetFloat("AmbienceVol", _ambienceSlider.value);
    }
    public void ChangeSensitivity()
    {
        PlayerPrefs.SetFloat("SensitivityMultiplier", _sensitivitySlider.value);
    }
    public void SetQuality(int quality)
    {
        PlayerPrefs.SetInt("Quality", quality);
        QualitySettings.SetQualityLevel(quality);
    }
}
