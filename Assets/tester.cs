using System;
using System.Collections;
using System.Collections.Generic;
using Audio_Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class tester : MonoBehaviour
{
    [SerializeField]
    private AudioManager m;

    public AudioClip ooga;
    public Music merica;
    public Music piano;
    
    [ContextMenu("Play Oooga")]
    void PlayOoga()
    {
        m.sfx.Play(ooga);
    }
    
    [ContextMenu("Play Merica")]
    void PlayMerica()
    {
        m.music.Play(merica);
    }

    [ContextMenu("Stop Merica")]
    void StopMerica()
    {
        m.music.Stop(merica);
    }
    [ContextMenu("Fade in Merica")]
    void FadeInMerica()
    {
        m.music.FadeIn(merica, 5f);
    }

    [ContextMenu("Fade out Merica")]
    void FadeOutMerica()
    {
        m.music.FadeOut(merica, 5f);
    }

    [ContextMenu("Half Vol Merica")]
    void HalfVolMerica()
    {
        m.music.SetMusicVolume(merica, 0.5f);
    }

    [ContextMenu("Full Vol Merica")]
    void FullVolMerica()
    {
        m.music.SetMusicVolume(merica, 1f);
    }

    [ContextMenu("Mute Merica")]
    void MuteMerica()
    {
        m.music.MuteMusic(merica);
    }
    
    [ContextMenu("UnMute Merica")]
    void UnMuteMerica()
    {
        m.music.MuteMusic(merica, false);
    }
    
    [ContextMenu("Play Piano")]
    void PlayPiano()
    {
        m.music.Play(piano);
    }

    [ContextMenu("Stop Piano")]
    void StopPiano()
    {
        m.music.Stop(piano);
    }
    [ContextMenu("Fade in Piano")]
    void FadeInPiano()
    {
        m.music.FadeIn(piano, 5f);
    }

    [ContextMenu("Fade out Piano")]
    void FadeOutPiano()
    {
        m.music.FadeOut(piano, 5f);
    }

    [ContextMenu("Half Vol Piano")]
    void HalfVolPiano()
    {
        m.music.SetMusicVolume(piano, 0.5f);
    }

    [ContextMenu("Full Vol Piano")]
    void FullVolPiano()
    {
        m.music.SetMusicVolume(piano, 1f);
    }

    [ContextMenu("Mute Piano")]
    void MutePiano()
    {
        m.music.MuteMusic(piano);
    }
    
    [ContextMenu("UnMute Piano")]
    void UnMutePiano()
    {
        m.music.MuteMusic(piano, false);
    }
    
    
    [ContextMenu("Fade into start Merica")]
    void FadeIntoStartMerica()
    {
        m.music.FadeIntoStart(piano, merica, 5f);
    }
    [ContextMenu("Fade into start piano")]
    void FadeIntoStartPiano()
    {
        m.music.FadeIntoStart(merica, piano, 5f);
    }
    
    
    [ContextMenu("Fade into current Merica")]
    void FadeIntoCurrentMerica()
    {
        m.music.FadeIntoCurrentTime(piano, merica, 5f);
    }
    [ContextMenu("Fade into current piano")]
    void FadeIntoCurrentPiano()
    {
        m.music.FadeIntoCurrentTime(merica, piano, 5f);
    }
    
    
    [ContextMenu("Fade into pause Merica")]
    void FadeIntoPauseMerica()
    {
        m.music.FadeIntoAfterPause(piano, merica, 5f, 1f, 5f);
    }
    [ContextMenu("Fade into pause piano")]
    void FadeIntoSPausePiano()
    {
        m.music.FadeIntoAfterPause(merica, piano, 5f, 1f, 5f);
    }
    



}
