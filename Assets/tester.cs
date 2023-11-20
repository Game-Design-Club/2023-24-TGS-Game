using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : MonoBehaviour
{
    [SerializeField]
    private AudioManager m;

    private void Awake()
    {
    }

    [ContextMenu("Start Piano")]
    public void StartPiano()
    {
        m.PlayMusic("piano");
    }

    [ContextMenu("Start music")]
    public void StartMusic()
    {
        m.PlayMusic("music");
    }
    
    [ContextMenu("Stop Piano")]
    public void StopPiano()
    {
        m.StopMusic("piano");
    }

    [ContextMenu("Stop music")]
    public void StopMusic()
    {
        m.StopMusic("music");
    }

    [ContextMenu("Fade music in")]
    public void FadeMusicIn()
    {
        m.FadeMusicIn("music", 5);
    }
    
    [ContextMenu("Fade piano in")]
    public void FadePianoIn()
    {
        m.FadeMusicIn("piano", 5);
    }

    [ContextMenu("Fade music out")]
    public void FadeMusicOut()
    {
        m.FadeMusicOut("music", 5);
    }
    
    [ContextMenu("Fade piano out")]
    public void FadePianoOut()
    {
        m.FadeMusicOut("piano", 5);
    }
    
    
}
