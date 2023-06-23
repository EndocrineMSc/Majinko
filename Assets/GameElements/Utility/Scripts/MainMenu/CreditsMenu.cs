using Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Utilities
{
    internal class CreditsMenu : MonoBehaviour
    {
        #region Fields and Properties

        private Canvas _creditsCanvas;
        private Button _backButton;

        #endregion

        #region Functions

        private void Awake()
        {
            _creditsCanvas = GetComponent<Canvas>();
            _creditsCanvas.enabled = false;
        }

        private void OnEnable()
        {
            MenuEvents.OnCreditsMenuOpened += OnCreditsOpened;
            MenuEvents.OnSettingsMenuOpened += OnOtherMenuOpened;
            MenuEvents.OnMainMenuOpened += OnOtherMenuOpened;
        }

        private void Start()
        {
            _backButton = GetComponentInChildren<Button>();
            _backButton.onClick.AddListener(OpenMainMenu);
        }

        private void OnDisable()
        {
            MenuEvents.OnCreditsMenuOpened -= OnCreditsOpened;
            MenuEvents.OnSettingsMenuOpened -= OnOtherMenuOpened;
            MenuEvents.OnMainMenuOpened -= OnOtherMenuOpened;
        }

        private void OnCreditsOpened()
        {
            _creditsCanvas.enabled = true;
        }

        private void OnOtherMenuOpened()
        {
            _creditsCanvas.enabled = false;
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
