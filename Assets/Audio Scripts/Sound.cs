using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name = "Unnamed Sound";
    
    public AudioClip clip;

    //Serialized Values
    [Range(0f, 5f)]
    public float volume = 1;
    [Range(0.1f, 3f)]
    public float pitch = 1;
    public bool loop = false;
    public bool mute = false;
    
    [HideInInspector]
    public AudioSource source;

    public void Reset()
    {
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
        source.mute = mute;
    }

    //Mutes the clip until reset is called
    public void MuteTemp(bool mute)
    {
        source.mute = mute;
    }

    //Sets the clips volume to value until reset is called 
    public void SetVolumeTemp(float volume)
    {
        source.volume = volume;
    }
    
    //Sets the clips pitch to value until reset is called 
    public void SetPitchTemp(float pitch)
    {
        source.pitch = pitch;
    }

    public bool IsPlaying()
    {
        return source.isPlaying;
    }

}
