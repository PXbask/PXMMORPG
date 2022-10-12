using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioMixer audioMixer;
    public AudioSource musicAudioSource;
    public AudioSource soundAudioSource;

    private const string MusicPath = "Music/";
    private const string SoundPath = "Sound/";

    private bool musicOn;
    public bool MusicOn
    {
        get { return musicOn; }
        set
        {
            musicOn = value;
            this.MusicMute(!musicOn);
        }
    }
    private bool soundOn;
    public bool SoundOn
    {
        get { return soundOn; }
        set
        {
            soundOn = value;
            this.SoundMute(!soundOn);
        }
    }
    private int musicVolume;
    public int MusicVolume
    {
        get { return musicVolume; }
        set
        {
            if(musicVolume != value)
            {
                this.musicVolume = value;
                if(musicOn) this.SetVolume("MusicVolume", musicVolume);
            }
        }
    }
    private int soundVolume;
    public int SoundVolume
    {
        get { return soundVolume; }
        set
        {
            if (soundVolume != value)
            {
                this.soundVolume = value;
                if (soundOn) this.SetVolume("SoundVolume", soundVolume);
            }
        }
    }
    private void Start()
    {
        this.musicOn = Config.MusicOn;
        this.soundOn = Config.SoundOn;
        this.musicVolume = Config.MusicVolume;
        this.soundVolume = Config.SoundVolume;
    }
    internal void PlayMusic(string name)
    {
        AudioClip clip = Resloader.Load<AudioClip>(MusicPath + name);
        if(clip == null)
        {
            Debug.LogWarningFormat("PlayMusic:{0} not exist", name);
            return;
        }
        if (musicAudioSource.isPlaying)
        {
            musicAudioSource.Stop();
        }
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }

    internal void PlaySound(string name)
    {
        AudioClip clip = Resloader.Load<AudioClip>(MusicPath + name);
        if (clip == null)
        {
            Debug.LogWarningFormat("PlayMusic:{0} not exist", name);
            return;
        }
        soundAudioSource.PlayOneShot(clip);
    }
    private void MusicMute(bool mute)
    {
        this.SetVolume("MusicVolume", mute ? 0 : MusicVolume);
    }
    private void SoundMute(bool mute)
    {
        this.SetVolume("SoundVolume", mute ? 0 : MusicVolume);
    }

    private void SetVolume(string name, int value)
    {
        float volume = value * 0.5f - 50f;
        this.audioMixer.SetFloat(name, volume);
    }
    protected void PlayClipOnAudioSource(AudioSource audioSource, AudioClip clip, bool isLoop)
    {
        return;
    }
}
