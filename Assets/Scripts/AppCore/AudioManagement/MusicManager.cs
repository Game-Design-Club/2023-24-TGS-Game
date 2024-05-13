using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace AppCore.AudioManagement
{
    public class MusicManager : MonoBehaviour { // This class is used to manage the playing of music in the game, including adaptive music
        //the audio mixer and its groups
        private AudioMixer _mixer = null;
        [SerializeField] private AudioMixerGroup musicGroup;
        [SerializeField] private AudioMixerGroup deactivatedGroup;
        [SerializeField] private AudioMixerGroup[] musicTrackGroups;
    
        //variables for dealing with music tracks
        private Music[] _userOfTrack;
        private float[] _trackVolume;

        //Volume of groups

        [Range(0f, 1f)] [SerializeField] private float musicVolume;

        //***** Unity Functions *****
        void Awake()
        {
            _mixer = musicGroup.audioMixer;
            _mixer.SetFloat(MixerConstants.MusicVolume, AudioManager.ConvertToDecibels(musicVolume));
            
            //initiating track storage
            _userOfTrack = new Music[musicTrackGroups.Length];
            Array.Fill(_userOfTrack, null);
            _trackVolume = new float[musicTrackGroups.Length];
            Array.Fill(_trackVolume, 0f);
        }
        
        private void OnValidate()
        {
            if (_mixer == null) return;
            _mixer.SetFloat(MixerConstants.MusicVolume, AudioManager.ConvertToDecibels(musicVolume));
        }

        //******* Music *******
        
        //***** Public Functions *****
        
        //*** Basic start and stop ***
        
        public void Play(Music music, float startTime = 0)
        {
            if (AlreadyPlaying(music))
            {
                Debug.LogWarning($"Tried to play Music: {music} but it was already playing");
                return;
            }

            int track = FindNextOpenTrack();
            
            SetTrackVolume(track, 1f);
            PlayOnTrack(music, track, startTime);
        }

        public void Stop(Music music)
        {
            int track = FindTrackOf(music);
            if (track == -1)
            {
                Debug.LogWarning($"Tried to stop Music: {music} but it wasn't playing");
                return;
            }
            
            StopMusicAndClearTrack(FindTrackOf(music));
        }
        
        //*** Fading in and out ***
        
        /// <summary>
        /// Fades into a specified time of Musics clip.
        /// </summary>
        /// <param name="music">Music to fade in.</param>
        /// <param name="startTime">Starting time of the music.</param>
        /// <param name="duration">How long the it takes for the music to fade in.</param>
        public void FadeIn(Music music, float duration, float startTime = 0f)
        {
            if (AlreadyPlaying(music))
            {
                Debug.LogWarning($"Tried to fade into Music: {music} but it was already playing");
                return;
            }
            
            int track = FindNextOpenTrack();
            
            StartCoroutine(FadeTrackIn(track, duration));
            PlayOnTrack(music, track, startTime);
        }
        
        /// <summary>
        /// Fades music out.
        /// </summary>
        /// <param name="music">Music to fade out.</param>
        /// <param name="duration">How long the it takes for the music to fade out.</param>
        public void FadeOut(Music music, float duration)
        {
            int track = FindTrackOf(music);
            if (track == -1)
            {
                Debug.LogWarning($"Tried to fade out of Music: {music} but it wasn't playing");
                return;
            }
            
            StartCoroutine(FadeOutAndEmptyTrack(track, duration));
        }
        
        //*** Transitions ***

        /// <summary>
        /// Fades out one music while fading into the start of another.
        /// </summary>
        /// <param name="currentMusic">Music that is being faded out of.</param>
        /// <param name="newMusic">Music that is being faded into.</param>
        /// <param name="duration">How long the it takes for the music to fully change.</param>
        public void FadeIntoStart(Music currentMusic, Music newMusic, float duration)
        {
            FadeOut(currentMusic, duration);
            FadeIn(newMusic, duration);
        }
        
        /// <summary>
        /// Fades out one music while fading into another (the newMusic will pick up where the currentMusic left off).
        /// </summary>
        /// <param name="currentMusic">Music that is being faded out of.</param>
        /// <param name="newMusic">Music that is being faded into.</param>
        /// <param name="duration">How long the it takes for the music to fully change.</param>
        public void FadeIntoCurrentTime(Music currentMusic, Music newMusic, float duration)
        {
            FadeIn(newMusic, duration, currentMusic.GetTime());
            FadeOut(currentMusic, duration);
        }

        /// <summary>
        /// Fades out one music and after a pause fades into another music.
        /// </summary>
        /// <param name="currentMusic">Music that is being faded out of.</param>
        /// <param name="newMusic">Music that is being faded into.</param>
        /// <param name="outDuration">How long the it takes for the currentMusic to fade out.</param>
        /// <param name="pause">Length of the pause between currentMusic fading out and new music fading in.</param>
        /// <param name="inDuration">How long the it takes for the newMusic to fade in.</param>
        public void FadeIntoAfterPause(Music currentMusic, Music newMusic, float outDuration, float pause,
            float inDuration)
        {
            StartCoroutine(FadeIntoAfterPauseCoroutine(currentMusic, newMusic, outDuration, pause, inDuration));
        }
        
        //*** Volume controls ***
        
        //mutes the music audio group
        public void Mute(bool mute = true)
        {
            _mixer.SetFloat(MixerConstants.MusicVolume,
                mute ? AudioManager.ConvertToDecibels(0f) : AudioManager.ConvertToDecibels(musicVolume));
        }
        
        //mutes the track of given music
        public void MuteMusic(Music music, bool mute = true)
        {
            int track = FindTrackOf(music);
            if (track == -1)
            {
                Debug.LogWarning($"Tried to {(mute ? "mute" : "unmute")}: {music.name} but it was not already playing");
                return;
            }
            
            MuteTrack(track, mute);
        }
    
        //sets the volume of the musics audio group
        public void SetVolume(float volume)
        {
            musicVolume = volume;
            _mixer.SetFloat(MixerConstants.MusicVolume, AudioManager.ConvertToDecibels(musicVolume));
        }
    
        //sets the volume of the musics track
        public void SetMusicVolume(Music music, float volume)
        {
            int track = FindTrackOf(music);
            if (track == -1)
            {
                Debug.LogWarning($"Tried to change volume of: {music.name} but it was not already playing");
                return;
            }
            SetTrackVolume(track, volume);
        }
        
        //***** Internal Functions *****
        
        //creates and returns a new AudioSource
        internal AudioSource GetNewSource()
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            return source;
        }
        
        //Destroys the given AudioSource
        internal void RemoveSource(AudioSource source)
        {
            Destroy(source);
        }

        //***** Private functions *****
        
        //checks if the music is already playing
        
        //*** Processing Functions ***
        private bool AlreadyPlaying(Music music)
        {
            return FindTrackOf(music) != -1;
        }
        
        //returns the string of the exposed volume parameter of given track (gives access to the volume of the track)
        private static string GetTrackExposedParam(int track)
        {
            return "Track" + track + "Volume";
        }
        
        //*** Finder functions ***
        
        //finds the first track that is not playing any music
        private int FindNextOpenTrack()
        {
            int track = -1;
            for (int i = 0; i < _userOfTrack.Length; i++)
            {
                if (_userOfTrack[i] == null)
                {
                    track = i;
                    break;
                }
            }

            return track;
        }
        
        //finds the track that the music with the given name is playing on
        private int FindTrackOf(Music music)
        {
            int track = -1;
            for (int i = 0; i < _userOfTrack.Length; i++)
            {
                if (_userOfTrack[i] == music)
                {
                    track = i;
                    break;
                }
            }
        
            return track;
        }
        
        //*** Volume Controls ***
        
        //mutes the audio group of the given track
        private void MuteTrack(int track, bool mute = true)
        {
            string trackName = GetTrackExposedParam(track);
            _mixer.SetFloat(trackName,
                mute ? AudioManager.ConvertToDecibels(0f) : AudioManager.ConvertToDecibels(_trackVolume[track]));
        }
    
        //sets the volume of given track audio group
        private void SetTrackVolume(int track, float volume)
        {
            string trackName = GetTrackExposedParam(track);
            _trackVolume[track] = volume;
            _mixer.SetFloat(trackName, AudioManager.ConvertToDecibels(_trackVolume[track]));
        }
        
        //*** Moving Music on and off tracks ***

        //moves the music onto given track and plays it
        private void PlayOnTrack(Music music, int track, float startTime)
        {
            MoveOntoTrack(track, music);
            music.SetTime(startTime);
            music.Play();
        }

        //moves music onto a track
        private void MoveOntoTrack(int track, Music music)
        {
            music.CurrentGroup = musicTrackGroups[track];
            music.AddSources();
            _userOfTrack[track] = music;
            foreach (AudioSource source in music.Sources)
            {
                source.outputAudioMixerGroup = musicTrackGroups[track];
            }
        }
        
        //stops the music on the given track and then clears the track
        private void StopMusicAndClearTrack(int track)
        {
            Music music = _userOfTrack[track];
            music.CurrentGroup = null;
            _userOfTrack[track] = null;
            foreach (AudioSource source in music.Sources)
            {
                source.outputAudioMixerGroup = deactivatedGroup;
            }
            SetTrackVolume(track, 0f);
            music.Stop();
            music.RemoveSources();
        }

        //*** Coroutines ***
        
        //The Coroutine for the FadeIntoAfterPause function
        private IEnumerator FadeIntoAfterPauseCoroutine(Music currentMusic, Music newMusic, float outDuration, float pause,
            float inDuration)
        {
            FadeOut(currentMusic, outDuration);
            yield return new WaitForSeconds(outDuration + pause);
            FadeIn(newMusic, inDuration);
        }
        
        //Fades a track in
        private IEnumerator FadeTrackIn(int track, float duration)
        {
            string trackParam = GetTrackExposedParam(track);
            MuteTrack(track);
            
            float currentTime = 0;
            float currentVol;
            _mixer.GetFloat(trackParam, out currentVol);
            currentVol = Mathf.Pow(10, currentVol / 20);
            float targetValue = 1f;
            while (currentTime < duration)
            {
                currentTime += Time.unscaledDeltaTime;
                float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
                _mixer.SetFloat(trackParam, Mathf.Log10(newVol) * 20);
                yield return null;
            }
        }
    
        //Fades a music track out and then stops the music and clears the track
        private IEnumerator FadeOutAndEmptyTrack(int track, float duration)
        {
            string trackParam = GetTrackExposedParam(track);

            float currentTime = 0;
            float currentVol;
            _mixer.GetFloat(trackParam, out currentVol);
            currentVol = Mathf.Pow(10, currentVol / 20);
            float targetValue = 0f;
            while (currentTime < duration)
            {
                currentTime += Time.unscaledDeltaTime;
                float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
                _mixer.SetFloat(trackParam, Mathf.Log10(newVol) * 20);
                yield return null;
            }

            StopMusicAndClearTrack(track);
        }
    }
}
