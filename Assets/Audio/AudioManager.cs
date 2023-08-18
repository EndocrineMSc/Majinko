using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    internal class AudioManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static AudioManager Instance { get; private set; }

        //Auto-built lists on Awake
        private List<AudioSource> _soundEffects;
        private List<AudioSource> _gameTracks;
        private const string GAMETRACKS_FOLDER = "GameTracks";
        private const string SOUNDEFFECTS_FOLDER = "SoundEffects";

        //Groups of the MasterMixer Asset
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioMixerGroup _SFX;
        [SerializeField] private AudioMixerGroup _music;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);

            //Builds the Lists to be used from the Assets/Resources Folder
            _gameTracks = BuildGameTrackList();
            _soundEffects = BuildSoundEffectList();
        }

        private List<AudioSource> BuildGameTrackList()
        {
            AudioClip[] gameTrackArray = Resources.LoadAll<AudioClip>(GAMETRACKS_FOLDER);
            List<AudioSource> audioSources = new();

            foreach (AudioClip _clip in gameTrackArray)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();

                audioSource.clip = _clip;
                audioSource.loop = true;
                audioSource.volume = 0;
                audioSource.playOnAwake = false;
                audioSource.outputAudioMixerGroup = _music;

                audioSources.Add(audioSource);
            }
            return audioSources;
        }

        private List<AudioSource> BuildSoundEffectList()
        {
            AudioClip[] soundEffectArray = Resources.LoadAll<AudioClip>(SOUNDEFFECTS_FOLDER);
            List<AudioSource> audioSources = new();

            foreach (AudioClip _clip in soundEffectArray)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();

                audioSource.clip = _clip;
                audioSource.outputAudioMixerGroup = _SFX;
                audioSource.playOnAwake = false;

                audioSources.Add(audioSource);
            }
            return audioSources;
        }

        // Start playing a GameTrack attached to the manager if it isn't playing already (for param see above)
        internal void PlayGameTrack(Track track)
        {
            AudioSource audioSource = _gameTracks[(int)track];

            if (!audioSource.isPlaying)
            { 
                audioSource.Play();
            }
        }

        internal void FadeInGameTrack(Track track, float fadeDuration = 3f)
        {
            AudioSource audioSource = _gameTracks[(int)track];   
            StartCoroutine(StartFade(audioSource, fadeDuration, 1f));
        }

        internal void FadeOutGameTrack(Track track, float fadeDuration = 3f)
        {
            AudioSource audioSource = _gameTracks[(int)track];
            StartCoroutine(StartFade(audioSource, fadeDuration, 0f));
        }

        //Plays a Sound Effect if it isn't playing already
        internal void PlaySoundEffectOnce(SFX sfx)
        {
            AudioSource audioSource = _soundEffects[(int)sfx];

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        //Plays a Sound Effect even if it is playing already
        internal void PlaySoundEffectWithoutLimit(SFX sfx)
        {
            AudioSource audioSource = _soundEffects[(int)sfx];
            audioSource.Play();
        }

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

        //Slider Functions for settings (needs to be public for unity sliders)
        public void SetMasterVolume(float volume)
        {
            _audioMixer.SetFloat("Master", volume > 0 ? Mathf.Log(volume) * 20f : -80f);
        }

        public void SetMusicVolume(float volume)
        {
            _audioMixer.SetFloat("Music", volume > 0 ? Mathf.Log(volume) * 20f : -80f);
        }

        public void SetEffectsVolume(float volume)
        {
            _audioMixer.SetFloat("SFX", volume > 0 ? Mathf.Log(volume) * 20f : -80f);

            //Play an exemplary SFX to give the play an auditory volume feedback
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0102_ManaBlitz_Spawn);
        }

        #endregion
    }

    #region Enums

    internal enum Track
    {
        _0001_LevelOne,
        _0002_MainMenu,
        GameTrackOne,
    }

    internal enum SFX
    {
        #region 0 - 100 UI-SFX

        _0001_ButtonClick,
        _0002_MouseOverCard,
        _0003_EndTurnClick,
        _0004_CardDrag,
        _0005_CardDragReturn,
        _0006_DrawHand,
        _0007_ShopCard_Picked,
        _0008_Mouse_Over_Button,

        #endregion

        #region 101 - 500 Attack-SFX

        _0101_ManaBlitz_Shot,
        _0102_ManaBlitz_Spawn,
        _0103_Blunt_Spell_Impact,
        _0104_Player_Takes_Damage,
        _0105_FireArrow_Impact,
        _0106_Icicle_Shot,
        _0107_Icicle_Impact,
        _0108_WraithCaster_Shot,
        _0109_FireArrow_Shot,
        _0110_LightningStrike,
        _0111_FireBomb,

        #endregion

        #region 501 - 750 Monster-SFX

        _0501_Zombie_Spawn,
        _0502_Zombie_Death,
        _0503_Wraith_Spawn,
        _0504_Wraith_Death,

        #endregion

        #region 751 - 1000 Arena-SFX

        _0751_Orb_Impact_01,
        _0752_Orb_Impact_02,
        _0753_Orb_Impact_03,
        _0754_Orb_Impact_04,
        _0755_Orb_Impact_05,
        _0756_Orb_Impact_06,
        _0757_Orb_Impact_07,
        _0758_Orb_Impact_08,
        _0759_Orb_Impact_09,
        _0760_Sphere_In_Portal,
        _0761_Sphere_Shot_01,
        _0762_Sphere_On_Wood_01,
        _0763_Sphere_On_Wood_02,
        _0764_Sphere_On_Wood_03,
        _0765_Sphere_On_Wood_04,
        _0766_Sphere_On_Wood_05,
        _0767_Sphere_On_Wood_06,
        _0768_Sphere_On_Wood_07,
        _0769_Sphere_On_Wood_08,
        _0770_Orb_Spawn_Whoosh,

        #endregion
    }

    #endregion
}