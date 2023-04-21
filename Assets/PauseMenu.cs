using PeggleWars.Audio;
using PeggleWars.Audio.Options;
using PeggleWars.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace PeggleWars.Menus
{
    internal class PauseMenu : MonoBehaviour
    {
        #region Fields and Properties

        private Canvas _settingsCanvas;
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _musicSlilder;
        [SerializeField] private Slider _volumeSlider;

        #endregion
        // Start is called before the first frame update
        void Start()
        {
            _settingsCanvas = GetComponent<Canvas>();
            _settingsCanvas.enabled = false;
            AudioOptionManager _audioOptionManager = AudioManager.Instance.GetComponent<AudioOptionManager>();
            _masterSlider.onValueChanged.AddListener(_audioOptionManager.SetMasterVolume);
            _musicSlilder.onValueChanged.AddListener(_audioOptionManager.SetMusicVolume);
            _volumeSlider.onValueChanged.AddListener(_audioOptionManager.SetEffectsVolume);

            PauseControl.Instance.PauseAndUnpauseGame?.AddListener(OpenAndCloseCanvas);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                PauseControl.Instance.PauseAndUnpauseGame?.Invoke();
            }
        }

        void OpenAndCloseCanvas()
        {
            if (_settingsCanvas.isActiveAndEnabled)
            {
                _settingsCanvas.enabled = false;
            }
            else
            {
                _settingsCanvas.enabled = true;
            }
        }

        private void OnDisable()
        {
            PauseControl.Instance.PauseAndUnpauseGame?.RemoveListener(OpenAndCloseCanvas);
        }
    }
}
