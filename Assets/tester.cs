using System;
using System.Collections;
using System.Collections.Generic;
using Audio_Scripts;
using UnityEngine;

public class tester : MonoBehaviour
{
    [SerializeField]
    private AudioManager m;

    public AudioSource ooga;
    public Music piano;
    private int i;
    

    [ContextMenu("Play ooga")]
    void PlayOoga()
    {
        m.sfx.Play(ooga);
    }
    
    [ContextMenu("Play piano")]
    void PlayPiano()
    {
        m.music.FadeIn(piano, 0f, 5f);
    }


}
