using System.Collections;
using System.Collections.Generic;
using EnumCollection;
using UnityEngine;
using UnityEngine.Audio;

namespace PeggleWars.Audio
{
    //this script is intended as a singleton
    //All Clips should be in the Folders Resources/"GameTracks" or Resources/"SoundEffects"
    //respectively
    public class AudioManager : MonoBehaviour
    {
        #region Fields

        public static AudioManager Instance { get; private set; }

        private List<AudioSource> _soundEffects;
        private List<AudioSource> _gameTracks;

        [SerializeField] private AudioMixerGroup _SFX;
        [SerializeField] private AudioMixerGroup _music;

        #endregion

        #region Public Functions

        //Fades music in or out depending on the Fade enum.
        //Uses the enum Track to get a specific Track from the Track-Array
        //Order in Array will be alphabetical
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

        public void PlayGameTrack(Track track)
        {
            AudioSource audioSource = _gameTracks[(int)track];

            if (!audioSource.isPlaying)
            { 
                audioSource.Play();
            }
        }

        //Plays a Sound Effect according to the enum index
        //if it isn't playing already
        public void PlaySoundEffect(SFX sfx)
        {
            AudioSource audioSource = _soundEffects[(int)sfx];

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        //Plays a Sound Effect according to the enum index
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