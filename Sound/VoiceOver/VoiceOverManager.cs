using System.Collections.Generic;
using UnityEngine;

public class VoiceOverManager : LanguageComponent<AudioSource, AudioClip>
{
    [SerializeField] private VoiceOverLine[] voiceOverLines;

    private Dictionary<string, AudioClip[]> _voiceOverLines;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        _voiceOverLines = new Dictionary<string, AudioClip[]>();
        foreach (var line in voiceOverLines)
        {
            _voiceOverLines[line._voiceOverTrigger] = line._audioClips;
        }
    }

    public string FindKeyByClip(AudioClip clip)
    {
        foreach (var kvp in _voiceOverLines)
        {
            if (ArrayContainsClip(kvp.Value, clip))
            {
                return kvp.Key;
            }
        }
        return null; // Clip not found in dictionary
    }
    private bool ArrayContainsClip(AudioClip[] array, AudioClip clip)
    {
        foreach (var item in array)
        {
            if (item == clip)
            {
                return true;
            }
        }
        return false;
    }

    protected override void SetLanguage()
    {
        //Save how long the clip has been playing
        var playingLength = _source.time;
        //Only play again after changing the clip if it was already playing
        bool wasPlaying = _source.isPlaying;
        
        //Only when replacing the clip should this happen
        if(_source.clip != null)
        {
            //Replace clip
            AudioClip clipToFind = _source.clip;

            string correspondingKey = FindKeyByClip(clipToFind);

            if (correspondingKey != null)
            {
                _source.clip = _voiceOverLines[correspondingKey][PlayerPrefs.GetInt("LanguageIndex")];
            }
            else
            {
                Debug.LogError("AudioClip not found in the VoiceOverManager dictionary.");
            }
        }

        //Play the audiosource again, because it automatically stops after changing the clip
        if (wasPlaying) _source.Play();

        //Set playing time to be the same as it was before changing clips
        _source.time = playingLength;
    }

    public void SpeakLine(string lineTrigger)
    {
        _source.clip = _voiceOverLines[lineTrigger][PlayerPrefs.GetInt("LanguageIndex")];
        _source.Play();
    }
}