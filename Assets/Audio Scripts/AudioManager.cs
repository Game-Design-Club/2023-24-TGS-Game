using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //constant varriables used to access different groups in the audio mixer
    private const string MASTER_VOLUME = "MasterVolume";
    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SFX_VOLUME = "SFXVolume";
    
    //the audio mixer and its groups
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerGroup[] musicTrackGroups;
    
    //variables for dealing with music tracks
    private bool[] _trackInUse;
    private string[] _userOfTrack;
    public float[] trackVolume;

    //Volume of groups in decibles
    public float masterVolume = 1;
    public float musicVolume = 1;
    public float sfxVolume = 1;
    
    public Sound[] music;
    public Sound[] sfx;
    
    //Setting up all systems
    void Awake()
    {
        _trackInUse = new bool[musicTrackGroups.Length];
        Array.Fill(_trackInUse, false);
        _userOfTrack = new string[musicTrackGroups.Length];
        Array.Fill(_userOfTrack, "None");
        trackVolume = new float[musicTrackGroups.Length];
        Array.Fill(trackVolume, 1f);
        
        foreach (Sound sound in music)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.outputAudioMixerGroup = musicGroup;
            sound.Reset();
        }
        foreach (Sound sound in sfx)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.outputAudioMixerGroup = sfxGroup;
            sound.Reset();
        }
    }

    //***** Master *****
    
    public void MuteMaster(bool mute)
    {
        if (mute)
        {
            mixer.SetFloat(MASTER_VOLUME, -80f);
        }
        else
        {
            mixer.SetFloat(MASTER_VOLUME, masterVolume);
        }
    }

    public void SetMasterVolume(float decibels)
    {
        masterVolume = decibels;
        mixer.SetFloat(MASTER_VOLUME, masterVolume);
    }
    
    //****** SFX ********
    
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfx, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " was not found :(");
            return;
        }

        s.source.Play();
    }
    
    //Plays the sound effect at a specified volume and pitch (resets pitches to original values afterwards)
    public void PlaySFX(string name, float volume, float pitch)
    {
        Sound s = Array.Find(sfx, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " was not found :(");
            return;
        }
        
        s.SetVolumeTemp(volume);
        s.SetPitchTemp(pitch);

        s.source.Play();
        
        s.Reset();
    }
    
    public void MuteSFX(bool mute)
    {
        if (mute)
        {
            mixer.SetFloat(SFX_VOLUME, -80f);
        }
        else
        {
            mixer.SetFloat(SFX_VOLUME, sfxVolume);
        }
    }
    
    //mutes or unmutes the specified sound until reset is called on the sound
    public void MuteSFX(string name, bool mute) 
    {
        Sound s = Array.Find(sfx, sound => sound.name == name);
        s.MuteTemp(mute);
    }
    
    public void SetSFXVolume(float decibels)
    {
        sfxVolume = decibels;
        mixer.SetFloat(SFX_VOLUME, sfxVolume);
    }

    //resets all SFX to their original values
    public void ResetAllSFX()
    {
        foreach (Sound sound in sfx)
        {
            sound.Reset();
        }
    }
    
    //***** Music *****
    
    public void PlayMusic(string name)
    {
        //finds music
        Sound s = Array.Find(music, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + " was not found :(");
            return;
        }

        //gets next open track
        int track = FindNextOpenTrack();
        
        if (track == -1)
        {
            return;
        }

        //fills track
        s.source.outputAudioMixerGroup = musicTrackGroups[track];
        _trackInUse[track] = true;
        _userOfTrack[track] = name;
        
        //plays sound
        s.source.Play();
    }

    public void FadeMusicIn(string name, float duration)
    {
        //finds music
        Sound s = Array.Find(music, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + " was not found :(");
            return;
        }

        //finds next open track
        int track = FindNextOpenTrack();
        
        if (track == -1)
        {
            return;
        }

        string trackParam = GetTrackExposedParam(track);

        //fills track
        s.source.outputAudioMixerGroup = musicTrackGroups[track];
        _trackInUse[track] = true;
        _userOfTrack[track] = name;
        
        //mutes the track and then fades it back in over the given time
        MuteTrack(true, track);
        s.source.Play();
        StartCoroutine(FadeMixerGroup(trackParam, duration, trackVolume[track]));
    }

    public void StopMusic(string name)
    {
        Sound s = Array.Find(music, sound => sound.name == name);
        
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + " was not found :(");
            return;
        }

        int track = FindTrackOf(name);
        if (track == -1)
        {
            return;
        }
        
        s.source.outputAudioMixerGroup = musicGroup;
        _trackInUse[track] = false;
        _userOfTrack[track] = "None";
        trackVolume[track] = musicVolume;
        
        s.source.Stop();
    }
    
    public void FadeMusicOut(string name, float duration)
    {
        Sound s = Array.Find(music, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + " was not found :(");
            return;
        }

        int track = FindTrackOf(name);
        if (track == -1)
        {
            return;
        }
        
        //v this is done so it can run on its own time
        StartCoroutine(FadeMusicTrackOut(track, s, duration));
    }
    
    public void MuteTrack(bool mute, int track)
    {
        string trackName = GetTrackExposedParam(track);
        if (mute)
        {
            mixer.SetFloat(trackName, -80f);
        }
        else
        {
            mixer.SetFloat(trackName, trackVolume[track]);
        }
    }
    
    public void MuteMusic(bool mute)
    {
        if (mute)
        {
            mixer.SetFloat(MUSIC_VOLUME, -80f);
        }
        else
        {
            mixer.SetFloat(MUSIC_VOLUME, musicVolume);
        }
    }
    
    public void SetMusicVolume(float decibels)
    {
        musicVolume = decibels;
        mixer.SetFloat(MUSIC_VOLUME, musicVolume);
    }

    //returns the string of the exposed volume parameter of given track
    private string GetTrackExposedParam(int track)
    {
        return "Track" + track + "Volume";
    }

    private int FindNextOpenTrack()
    {
        int track = -1;
        for (int i = 0; i < _trackInUse.Length; i++)
        {
            if (_trackInUse[i] == false)
            {
                track = i;
                break;
            }
        }

        if (track == -1)
        {
            Debug.LogWarning("Music " + name + " unable to find open track");
        }

        return track;
    }

    private int FindTrackOf(string name)
    {
        int track = -1;
        for (int i = 0; i < _userOfTrack.Length; i++)
        {
            if (_userOfTrack[i] == name)
            {
                track = i;
                break;
            }
        }
        
        if (track == -1)
        {
            Debug.LogWarning("Tried to stop music: " + name + " but music was not playing");
        }

        return track;
    }
    
    //Fades a mixer group to a specified volume in a specified time
    //IMPORTANT: call must be wrapped in StartCoroutine(FadeMixerGroup(ex, 1, 1))
    //Edited from source: https://johnleonardfrench.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/
    private IEnumerator FadeMixerGroup(string groupExposedParam, float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        mixer.GetFloat(groupExposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            mixer.SetFloat(groupExposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }

        yield break;
    }
    
    //Fades a music track out and then stops the music and clears the track
    private IEnumerator FadeMusicTrackOut(int track, Sound music, float duration)
    {
        string trackParam = GetTrackExposedParam(track);

        float currentTime = 0;
        float currentVol;
        mixer.GetFloat(trackParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(-80f, 0.0001f, 1);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            mixer.SetFloat(trackParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }

        music.source.outputAudioMixerGroup = musicGroup;
        _trackInUse[track] = false;
        _userOfTrack[track] = "None";
        trackVolume[track] = musicVolume;
        
        music.source.Stop();
        yield break;
    }
}
