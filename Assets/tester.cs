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
    // public Music merica2;

    [ContextMenu("Play mer")]
    void PlayMer()
    {
        m.music.Play(merica);
    }

    [ContextMenu("Stop mer")]
    void StopMer()
    {
        m.music.Stop(merica);
    }
    [ContextMenu("Fade Play mer")]
    void fPlayMer()
    {
        m.music.FadeIn(merica, 5f);
    }

    [ContextMenu("Fade Stop mer")]
    void fStopMer()
    {
        m.music.FadeOut(merica, 5f);
    }



}
