using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using AudioSource = UnityEngine.AudioSource;

namespace Audio_Scripts
{
    public class MusicManager : MonoBehaviour
    {
        //constant varriables used to access different groups in the audio mixer
        private const string MUSIC_VOLUME = "MusicVolume";
    
        //the audio mixer and its groups
        private AudioMixer mixer;
        [SerializeField] private AudioMixerGroup musicGroup;
        [SerializeField] private AudioMixerGroup deactivatedGroup;
        [SerializeField] private AudioMixerGroup[] musicTrackGroups;
    
        //variables for dealing with music tracks
        private Music[] _userOfTrack;
        private float[] _trackVolume;

        //Volume of groups
        [Range(0f, 1f)]
        public float musicVolume = 1;


        //Setting up all systems
        void Awake()
        {
            mixer = musicGroup.audioMixer;
            mixer.SetFloat(MUSIC_VOLUME, ConvertToDecibels(musicVolume));
            
            //initiating track storage
            _userOfTrack = new Music[musicTrackGroups.Length];
            Array.Fill(_userOfTrack, null);
            _trackVolume = new float[musicTrackGroups.Length];
            Array.Fill(_trackVolume, 0f);
        }

        internal AudioSource AddClip(AudioClip clip)
        {
            return gameObject.AddComponent<AudioSource>();
        }
        
        internal void RemoveSource(AudioSource source)
        {
            Destroy(source);
        }

        //converts a volume level from 0f-1f to corresponding value in decibels
        private float ConvertToDecibels(float volume)
        {
            volume = Mathf.Clamp(volume, 0.0001f, 1f);
            return Mathf.Log10(volume) * 20;
        }

        //***** Music *****
        /// <summary>
        /// Fades into a specified time of Musics clip.
        /// </summary>
        /// <param name="music">Music to fade in.</param>
        /// <param name="startTime">Starting time of the music.</param>
        /// <param name="duration">How long the it takes for the music to fade in.</param>
        public void FadeIn(Music music, float duration, float startTime = 0f)
        {
            if (FindTrackOf(music) != -1)
            {
                Debug.LogWarning($"Tried to play Music: {music.musicName} but it was already playing");
                return;
            }
            int track = FindNextOpenTrack();
            
            MoveOntoTrack(track, music);
            StartCoroutine(FadeTrackIn(track, duration));
            music.SetTime(startTime);
            music.Play();
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
                Debug.LogWarning($"Tried to stop Music: {music.musicName} but it wasn't playing");
                return;
            }
            
            StartCoroutine(FadeOutAndEmptyTrack(track, duration));
        }

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

        private IEnumerator FadeIntoAfterPauseCoroutine(Music currentMusic, Music newMusic, float outDuration, float pause,
            float inDuration)
        {
            FadeOut(currentMusic, outDuration);
            yield return new WaitForSeconds(outDuration + pause);
            FadeIn(newMusic, inDuration);
        }

        //mutes the music audio group
        public void Mute(bool mute)
        {
            if (mute)
            {
                mixer.SetFloat(MUSIC_VOLUME, ConvertToDecibels(0f));
            }
            else
            {
                mixer.SetFloat(MUSIC_VOLUME, ConvertToDecibels(musicVolume));
            }
        }
    
        //mutes the audio group of the given track
        private void MuteTrack(bool mute, int track)
        {
            string trackName = GetTrackExposedParam(track);
            if (mute)
            {
                mixer.SetFloat(trackName, ConvertToDecibels(0f));
            }
            else
            {
                mixer.SetFloat(trackName, ConvertToDecibels(_trackVolume[track]));
            }
        }
    
        //sets the volume of the musics audio group
        public void SetVolume(float volume)
        {
            musicVolume = ConvertToDecibels(volume);
            mixer.SetFloat(MUSIC_VOLUME, ConvertToDecibels(musicVolume));
        }
    
        //sets the volume of given track audio group
        private void SetTrackVolume(int track, float volume)
        {
            string trackName = GetTrackExposedParam(track);
            _trackVolume[track] = volume;
            mixer.SetFloat(trackName, ConvertToDecibels(_trackVolume[track]));
        }

        private void MoveOntoTrack(int track, Music music)
        {
            music.AddSources();
            music.CurrentGroup = musicTrackGroups[track];
            _userOfTrack[track] = music;
            foreach (AudioSource source in music.Sources)
            {
                source.outputAudioMixerGroup = musicTrackGroups[track];
            }
            SetTrackVolume(track, 0f);
        }
        
        private void ClearTrack(int track)
        {
            Music music = _userOfTrack[track];
            _userOfTrack[track] = null;
            foreach (AudioSource source in music.Sources)
            {
                source.outputAudioMixerGroup = deactivatedGroup;
            }
            SetTrackVolume(track, 0f);
            music.CurrentGroup = null;
            music.Stop();
            music.RemoveSources();
        }
        
        //returns the string of the exposed volume parameter of given track (gives access to the volume of the track)
        private string GetTrackExposedParam(int track)
        {
            return "Track" + track + "Volume";
        }

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
        
        private IEnumerator FadeTrackIn(int track, float duration)
        {
            string trackParam = GetTrackExposedParam(track);
            MuteTrack(true, track);
            
            float currentTime = 0;
            float currentVol;
            mixer.GetFloat(trackParam, out currentVol);
            currentVol = Mathf.Pow(10, currentVol / 20);
            float targetValue = 1f;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
                mixer.SetFloat(trackParam, Mathf.Log10(newVol) * 20);
                yield return null;
            }

            yield break;
        }
    
        //Fades a music track out and then stops the music and clears the track
        private IEnumerator FadeOutAndEmptyTrack(int track, float duration)
        {
            string trackParam = GetTrackExposedParam(track);

            float currentTime = 0;
            float currentVol;
            mixer.GetFloat(trackParam, out currentVol);
            currentVol = Mathf.Pow(10, currentVol / 20);
            float targetValue = 0f;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
                mixer.SetFloat(trackParam, Mathf.Log10(newVol) * 20);
                yield return null;
            }

            ClearTrack(track);
            yield break;
        }
    }
}
