using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("Theme");
    }

    private void Update()
    {
        CheckMusicVolume();
        CheckSoundVolume();
    }

    private void CheckMusicVolume()
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetInt("Music") == 1)
            {
                UnPause("Theme");
            }
            else if (PlayerPrefs.GetInt("Music") == 0)
            {
                Pause("Theme");
            }
        }
    }

    private void CheckSoundVolume()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 1)
            {
                foreach (var s in sounds)
                {
                    if (s.name != "Theme")
                    {
                        s.source.volume = s.volume = 1;
                    }
                }
            }
            else if (PlayerPrefs.GetInt("Sound") == 0)
            {
                foreach (var s in sounds)
                {
                    if (s.name != "Theme")
                    {
                        s.source.volume = s.volume = 0;
                    }
                }
            }
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Pause();
    }

    public void UnPause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.UnPause();
    }

    private void RandomPopSound()
    {
        int clipToPlay = UnityEngine.Random.Range(0, 1);

        if (clipToPlay == 0)
        {
            Play("Pop1");
        }
        else
        {
            Play("Pop2");
        }
    }
}
