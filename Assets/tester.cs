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
    public Music merica2;

    private void Start()
    {
        merica.Initiate();
        merica2.Initiate();

    }

    [ContextMenu("Play mer")]
    void PlayMer()
    {
        m.music.FadeIn(merica, 5f);
    }

    [ContextMenu("Stop mer")]
    void StopMer()
    {
        m.music.FadeOut(merica, 5f);
    }

    [ContextMenu("Start mer2")]
    void SartPiano()
    {
        m.music.FadeIn(merica2, 5f);
    }

    [ContextMenu("stop mer2")]
    void EndPiano()
    {
        m.music.FadeOut(merica2, 5f);
    }

    [ContextMenu("fade to mer2")]
    void Fas()
    {
        m.music.FadeIntoCurrentTime(merica, merica2, 5f);
    }

    [ContextMenu("fade to mer")]
    void Fasa()
    {
        m.music.FadeIntoCurrentTime(merica2, merica, 5f);
    }
    
    


}
