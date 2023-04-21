using UnityEngine;
using UnityEngine.Audio;
using EnumCollection;

namespace PeggleWars.Audio.Options
{
    /// <summary>
    /// Intended as a component to the AudioManager.
    /// Provides functions for master, music and sound effect volume sliders.
    /// The AudioManager set the respective AudioGroups of the AudioSources automatically, depending on the folder the clips were in.
    /// </summary>
    internal class AudioOptionManager : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private AudioMixer _audioMixer;

        #endregion

        #region Public Functions

        /// <summary>
        /// Actual functions for volume sliders.
        /// The actual volume is in decibel, which is on a logarithmic scale.
        /// </summary>
        /// <param name="volume">Float set by the respective slider on a linear scale (between -80 and 20)</param>

        public void SetMasterVolume(float volume)
        {
            _audioMixer.SetFloat("Master", volume > 0 ? Mathf.Log(volume) *20f : -80f);
        }

        public void SetMusicVolume(float volume)
        {
            _audioMixer.SetFloat("Music", volume > 0 ? Mathf.Log(volume) * 20f : -80f);
        }

        public void SetEffectsVolume(float volume)
        {
            _audioMixer.SetFloat("SFX", volume > 0 ? Mathf.Log(volume) * 20f : -80f);
            
            //Play an exemplary SFX to give the play an auditory volume feedback
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0002_BasicPeggleHit);
        }

        #endregion
    }
}
