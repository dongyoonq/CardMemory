using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<Sound> musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Start()
    {
        musicSounds = new List<Sound>()
        {
            CreateSound(Resources.Load<AudioClip>("Sound/InGameBgm"), "InGame"),
            CreateSound(Resources.Load<AudioClip>("Sound/TitleBgm"), "Title"),
        };

        sfxSounds = new List<Sound>
        {
            CreateSound(Resources.Load<AudioClip>("Sound/BtnClick"), "BtnClick"),
            CreateSound(Resources.Load<AudioClip>("Sound/CardFlip"), "CardFlip"),
            CreateSound(Resources.Load<AudioClip>("Sound/MatchingCard"), "Match"),
            CreateSound(Resources.Load<AudioClip>("Sound/FailGame"), "Fail"),
            CreateSound(Resources.Load<AudioClip>("Sound/SuccessGame"), "Success"),
            CreateSound(Resources.Load<AudioClip>("Sound/Victory"), "Victory"),
        };

        GameObject _musicSource = new GameObject();
        _musicSource.AddComponent<AudioSource>();
        _musicSource.name = "Music Source";
        _musicSource.transform.parent = transform;
        musicSource = _musicSource.GetComponent<AudioSource>();

        GameObject _sfxSource = new GameObject();
        _sfxSource.AddComponent<AudioSource>();
        _sfxSource.name = "SFX Source";
        _sfxSource.transform.parent = transform;
        sfxSource = _sfxSource.GetComponent<AudioSource>();

        PlayMusic($"Title{UnityEngine.Random.Range(1, 4)}");
        musicSource.loop = true;

        PlayMusic("Title");
    }

    public void PlayMusic(string name)
    {
        Sound sound = musicSounds.Find(x => x.name == name);

        if (sound == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = sound.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = sfxSounds.Find(x => x.name == name);

        if (sound == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }

    public void StopMusic()
    {
        musicSource?.Stop();
    }

    public void StopSfx()
    {
        sfxSource?.Stop();
    }

    Sound CreateSound(AudioClip clip, string name)
    {
        Sound sound = new Sound();
        sound.clip = clip;
        sound.name = name;
        return sound;
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}