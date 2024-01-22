using Tools.TesterScript;

using UnityEngine;

namespace AppCore.AudioManagement
{
    public class AudioManagementTester : Tester
    {
        private AudioManager _audioManager;

        public AudioClip oogaSFX;
        public Music mericaMusic;
        public Music pianoMusic;

        private void Awake() {
            _audioManager = App.Instance.audioManager;
        }

        [ContextMenu("Play Oooga")]
        void PlayOoga()
        {
            _audioManager.sfx.Play(oogaSFX);
            DebugLog("Playing Oooga");
            
        }
    
        [ContextMenu("Play Merica")]
        void PlayMerica()
        {
            _audioManager.music.Play(mericaMusic);
            DebugLog("Playing Merica");
        }

        [ContextMenu("Stop Merica")]
        void StopMerica()
        {
            _audioManager.music.Stop(mericaMusic);
            DebugLog("Stopping Merica");
        }
        [ContextMenu("Fade in Merica")]
        void FadeInMerica()
        {
            _audioManager.music.FadeIn(mericaMusic, 5f);
            DebugLog("Fading in Merica");
        }

        [ContextMenu("Fade out Merica")]
        void FadeOutMerica()
        {
            _audioManager.music.FadeOut(mericaMusic, 5f);
            DebugLog("Fading out Merica");
        }

        [ContextMenu("Half Vol Merica")]
        void HalfVolMerica()
        {
            _audioManager.music.SetMusicVolume(mericaMusic, 0.5f);
            DebugLog("Half Volume Merica");
        }

        [ContextMenu("Full Vol Merica")]
        void FullVolMerica()
        {
            _audioManager.music.SetMusicVolume(mericaMusic, 1f);
            DebugLog("Full Volume Merica");
        }

        [ContextMenu("Mute Merica")]
        void MuteMerica()
        {
            _audioManager.music.MuteMusic(mericaMusic);
            DebugLog("Muting Merica");
        }
    
        [ContextMenu("UnMute Merica")]
        void UnMuteMerica()
        {
            _audioManager.music.MuteMusic(mericaMusic, false);
            DebugLog("Unmuting Merica");
        }
    
        [ContextMenu("Play Piano")]
        void PlayPiano()
        {
            _audioManager.music.Play(pianoMusic);
            DebugLog("Playing Piano");
        }

        [ContextMenu("Stop Piano")]
        void StopPiano()
        {
            _audioManager.music.Stop(pianoMusic);
            DebugLog("Stopping Piano");
        }
        [ContextMenu("Fade in Piano")]
        void FadeInPiano()
        {
            _audioManager.music.FadeIn(pianoMusic, 5f);
            DebugLog("Fading in Piano");
        }

        [ContextMenu("Fade out Piano")]
        void FadeOutPiano()
        {
            _audioManager.music.FadeOut(pianoMusic, 5f);
            DebugLog("Fading out Piano");
        }

        [ContextMenu("Half Vol Piano")]
        void HalfVolPiano()
        {
            _audioManager.music.SetMusicVolume(pianoMusic, 0.5f);
            DebugLog("Half Volume Piano");
        }

        [ContextMenu("Full Vol Piano")]
        void FullVolPiano()
        {
            _audioManager.music.SetMusicVolume(pianoMusic, 1f);
            DebugLog("Full Volume Piano");
        }

        [ContextMenu("Mute Piano")]
        void MutePiano()
        {
            _audioManager.music.MuteMusic(pianoMusic);
            DebugLog("Muting Piano");
        }
    
        [ContextMenu("UnMute Piano")]
        void UnMutePiano()
        {
            _audioManager.music.MuteMusic(pianoMusic, false);
            DebugLog("Unmuting Piano");
        }
    
    
        [ContextMenu("Fade into start Merica")]
        void FadeIntoStartMerica()
        {
            _audioManager.music.FadeIntoStart(pianoMusic, mericaMusic, 5f);
            DebugLog("Fading into start Merica");
        }
        [ContextMenu("Fade into start piano")]
        void FadeIntoStartPiano()
        {
            _audioManager.music.FadeIntoStart(mericaMusic, pianoMusic, 5f);
            DebugLog("Fading into start Piano");
        }
    
    
        [ContextMenu("Fade into current Merica")]
        void FadeIntoCurrentMerica()
        {
            _audioManager.music.FadeIntoCurrentTime(pianoMusic, mericaMusic, 5f);
            DebugLog("Fading into current Merica");
        }
        [ContextMenu("Fade into current piano")]
        void FadeIntoCurrentPiano()
        {
            _audioManager.music.FadeIntoCurrentTime(mericaMusic, pianoMusic, 5f);
            DebugLog("Fading into current Piano");
        }
    
    
        [ContextMenu("Fade into pause Merica")]
        void FadeIntoPauseMerica()
        {
            _audioManager.music.FadeIntoAfterPause(pianoMusic, mericaMusic, 5f, 1f, 5f);
            DebugLog("Fading into pause Merica");
        }
        [ContextMenu("Fade into pause piano")]
        void FadeIntoSPausePiano()
        {
            _audioManager.music.FadeIntoAfterPause(mericaMusic, pianoMusic, 5f, 1f, 5f);
            DebugLog("Fading into pause Piano");
        }
    



    }
}
