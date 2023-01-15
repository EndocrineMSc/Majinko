using System.Collections;
using System.Collections.Generic;
using EnumCollection;
using UnityEngine;
using UnityEngine.Audio;

namespace PeggleWars.Audio
{
    /// <summary>
    /// This script is intended as a singleton.
    /// All Clips should be in the Folders Resources/"GameTracks" or Resources/"SoundEffects", respectively.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        #region Fields and Properties

        public static AudioManager Instance { get; private set; }

        //Auto-built lists on Awake
        private List<AudioSource> _soundEffects;
        private List<AudioSource> _gameTracks;

        //Groups of the MasterMixer Asset
        [SerializeField] private AudioMixerGroup _SFX;
        [SerializeField] private AudioMixerGroup _music;

        #endregion

        #region Public Functions

        /// <summary>
        /// Fades music in or out depending on "Fade".
        /// Uses the enum Track to get a specific Track from the Track-Array (alhabetical order).
        /// The Track needs to be playing already for this to work.
        /// </summary>
        /// <param name="track">Entry in the enum "Track", signifies a track by name in the Track-Array which will be alphabetically sorted</param>
        /// <param name="fade">Value of fade "In" or "Out" will determine which way the music fades</param>
        public void FadeGameTrack(Track track, Fade fade)
        {
            AudioSource audioSource = _gameTracks[(int)track];   

            if (fade == Fade.In)
            {
                StartCoroutine(StartFade(audioSource, 3f, 1f));
            }
            else if (fade == Fade.Out) 
            {
                StartCoroutine(StartFade(audioSource, 3f, 0f));
            }
        }

        // Start playing a GameTrack attached to the manager if it isn't playing already (for param see above)
        public void PlayGameTrack(Track track)
        {
            AudioSource audioSource = _gameTracks[(int)track];

            if (!audioSource.isPlaying)
            { 
                audioSource.Play();
            }
        }

        //Plays a Sound Effect according to the enum index, if it isn't playing already
        public void PlaySoundEffectOnce(SFX sfx)
        {
            AudioSource audioSource = _soundEffects[(int)sfx];

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        //Plays a Sound Effect according to the enum index, even if it is playing already
        public void PlaySoundEffectWithoutLimit(SFX sfx)
        {
            AudioSource audioSource = _soundEffects[(int)sfx];
            audioSource.Play();
        }

        #endregion

        #region Private Functions

        private void Awake()
        {
            // If there is an instance, and it's not this one, delete this one
            // Instantiation of the singleton, will stay for all scenes
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            DontDestroyOnLoad(this);

            //Builds the Lists to be used from the Assets/Resources Folder
            _gameTracks = BuildGameTrackList();
            _soundEffects = BuildSoundEffectList();
        }

        /// <summary>
        /// Takes the music clips in the "GameTracks" Folder and builds looping AudioSources as components to the AudioManager GameObject.
        /// Volume of each AudioSource will be set to zero, so a fade in is necessary after starting to play the track.
        /// Also assigns the AudioMixerGroup to be used in the AudioOptionManager.
        /// </summary>
        /// <returns>List of all available GameTracks as AudioSources. Used for referencing which music to start and stop</returns>
        private List<AudioSource> BuildGameTrackList()
        {
            AudioClip[] _gameTrackArray = Resources.LoadAll<AudioClip>("GameTracks");
            List<AudioSource> _tempList = new();

            foreach (AudioClip _clip in _gameTrackArray)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();

                audioSource.clip = _clip;
                audioSource.loop = true;
                audioSource.volume = 0;
                audioSource.playOnAwake = false;
                audioSource.outputAudioMixerGroup = _music;

                _tempList.Add(audioSource);
            }
            return _tempList;
        }

        /// <summary>
        /// Takes the music clips in the "SoundEffects" Folder and builds AudioSources as components to the AudioManager GameObject.
        /// Also assigns the AudioMixerGroup to be used in the AudioOptionManager.
        /// </summary>
        /// <returns>List of all available SoundEffects as AudioSources. Used for referencing which SFX to play</returns>
        private List<AudioSource> BuildSoundEffectList()
        {
            AudioClip[] _soundEffectArray = Resources.LoadAll<AudioClip>("SoundEffects");
            List<AudioSource> _tempList = new();

            foreach (AudioClip _clip in _soundEffectArray)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();

                audioSource.clip = _clip;
                audioSource.outputAudioMixerGroup = _SFX;
                audioSource.playOnAwake = false;

                _tempList.Add(audioSource);
            }
            return _tempList;
        }

        #endregion

        #region IEnumerators

        /// <summary>
        /// Does the actual fading of an AudioSource with Lerp.
        /// </summary>
        /// <param name="audioSource">The AudioSource to be faded in or out</param>
        /// <param name="duration">How long it takes to reach max/min volume in seconds</param>
        /// <param name="targetVolume">max/min volume</param>
        /// <returns></returns>
        private IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float startVolume = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
                yield return null;
            }
            audioSource.volume = targetVolume;
        }

        #endregion
    }
}