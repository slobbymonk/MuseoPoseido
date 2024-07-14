using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    public AudioClip song;

    public AudioMixerGroup music;
    public AudioMixerGroup sfx;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            if(s != sounds[0])
                s.source.outputAudioMixerGroup = sfx;
            else
                s.source.outputAudioMixerGroup = music;

            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    private void Start()
    {
        StartMusic(sounds[0].clip);
    }
    void StartMusic(AudioClip music)
    {
        sounds[0].source.clip = music;

        Play("BGMusic");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(ChangeMusic(song, 3));
        }
    }

    IEnumerator ChangeMusic(AudioClip newMusic, float transitionTime)
    {
        float t = 0;
        float startVolume = sounds[0].volume;
        float endVolume = 0;

        while (t < transitionTime)
        {
            t += Time.deltaTime;
            float normalizedTime = t / transitionTime;

            sounds[0].source.volume = Mathf.Lerp(startVolume, endVolume, normalizedTime);
            yield return null;
        }

        StartMusic(newMusic);
        sounds[0].source.volume = startVolume;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }
}
