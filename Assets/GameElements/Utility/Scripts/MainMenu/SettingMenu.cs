using Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Utilities
{
    internal class SettingMenu : MonoBehaviour
    {
        #region Fields and Properties

        private Canvas _settingsCanvas;
        private Button _backButton;

        [SerializeField] private Slider _masterVolume;
        [SerializeField] private Slider _musicVolume;
        [SerializeField] private Slider _sfxVolume;

        #endregion

        #region Functions

        private void Awake()
        {
            _settingsCanvas = GetComponent<Canvas>();
            _settingsCanvas.enabled = false;
        }

        private void OnEnable()
        {
            MenuEvents.OnSettingsMenuOpened += OnSettingsOpened;
            MenuEvents.OnMainMenuOpened += OnOtherMenuOpened;
            MenuEvents.OnCreditsMenuOpened += OnOtherMenuOpened;
        }

        private void Start()
        {
            _backButton = GetComponentInChildren<Button>();

            _backButton.onClick.AddListener(OpenMainMenu);

            _masterVolume.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
            _musicVolume.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
            _sfxVolume.onValueChanged.AddListener(AudioManager.Instance.SetEffectsVolume);
        }

        private void OnDisable()
        {
            MenuEvents.OnSettingsMenuOpened -= OnSettingsOpened;
            MenuEvents.OnMainMenuOpened -= OnOtherMenuOpened;
            MenuEvents.OnCreditsMenuOpened -= OnOtherMenuOpened;
        }

        private void OnSettingsOpened()
        {
            _settingsCanvas.enabled = true;
        }
        
        private void OnOtherMenuOpened()
        {
            _settingsCanvas.enabled = false;
        }

        private void OpenMainMenu()
        {
            PlayButtonClick();
            MenuEvents.RaiseMainMenuOpened();
        }

        private void PlayButtonClick()
        {
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0001_ButtonClick);
        }

        #endregion
    }
}
