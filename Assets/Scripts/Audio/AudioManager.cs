using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnumCollection;
using PeggleWars;
using UnityEngine;

namespace PeggleWars.Audio
{
    //this script is intended as a singleton
    public class AudioManager : MonoBehaviour
    {
        #region Fields

        public static AudioManager Instance { get; private set; }

        private List<AudioSource> _gameTracks = new();
        private List<AudioSource> _soundEffects = new();

        [SerializeField] private GameObject _gameTracksObject;
        [SerializeField] private GameObject _soundEffectsObject;

        #endregion

        #region Public Functions

        //Fades music in or out depending on whether it is already
        //playing or not. Uses the enum Track to get a specific
        //Track according to the enum from the Track-List
        //Order in List will be according to order in GameObject
        public void FadeGameTrack(Track track, Fade fade)
        {
            AudioSource audioSource = _gameTracks[(int)track];

            if (fade == Fade.In)
            {
                audioSource.volume = 0;
                StartCoroutine(StartFade(audioSource, 3f, 1f));
            }
            else if (fade == Fade.Out) 
            {
                StartCoroutine(StartFade(audioSource, 3f, 0f));
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

        public void PlaySoundEffectNoLimit(SFX sfx)
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
        }

        private void Start()
        {
            _gameTracks = _gameTracksObject.GetComponents<AudioSource>().ToList<AudioSource>();
            _soundEffects = _soundEffectsObject.GetComponents<AudioSource>().ToList<AudioSource>();
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