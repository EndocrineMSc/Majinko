using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Audio;
using System;
using PeggleWars;
using UnityEngine.SceneManagement;

namespace Utility
{
    //This script is to be attached to the MainMenuCanvas in the MainMenu Scene
    internal class MainMenu : MonoBehaviour
    {
        #region Fields and Properties

        private Canvas _menuCanvas;

        private readonly string START_BUTTON_PARAM = "StartButton";
        private readonly string SETTINGS_BUTTON_PARAM = "SettingsButton";
        private readonly string CREDITS_BUTTON_PARAM = "CreditsButton";
        private readonly string TUTORIAL_BUTTON_PARAM = "TutorialButton";
        private readonly string FIRSTWORLD_SCENE_PARAM = "LoadingScreen";

        private GameObject _startButton;
        private GameObject _settingsButton;
        private GameObject _creditsButton;
        private GameObject _tutorialButton;

        #endregion

        #region Functions

        private void Awake()
        {
            _menuCanvas = GetComponent<Canvas>();
        }

        private void OnEnable()
        {
            MenuEvents.OnMainMenuOpened += OnMainMenuOpened;
            MenuEvents.OnSettingsMenuOpened += OnOtherMenuOpened;
            MenuEvents.OnCreditsMenuOpened += OnOtherMenuOpened;
        }

        private void Start()
        {
            _startButton = GameObject.FindGameObjectWithTag(START_BUTTON_PARAM);
            _settingsButton = GameObject.FindGameObjectWithTag(SETTINGS_BUTTON_PARAM);
            _creditsButton = GameObject.FindGameObjectWithTag(CREDITS_BUTTON_PARAM);
            _tutorialButton = GameObject.FindGameObjectWithTag(TUTORIAL_BUTTON_PARAM);

            _startButton.GetComponent<Button>().onClick.AddListener(StartGame);
            _settingsButton.GetComponent<Button>().onClick.AddListener(OpenSettings);
            _creditsButton.GetComponent<Button>().onClick.AddListener(OpenCredits);
            _tutorialButton.GetComponent<Button>().onClick.AddListener(OpenTutorial);
        }

        private void OnDisable()
        {
            MenuEvents.OnMainMenuOpened -= OnMainMenuOpened;
            MenuEvents.OnSettingsMenuOpened -= OnOtherMenuOpened;
            MenuEvents.OnCreditsMenuOpened -= OnOtherMenuOpened;
        }

        private void OpenTutorial()
        {
            PlayButtonClick();
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.Tutorial);
        }

        private void OnMainMenuOpened()
        {
            _menuCanvas.enabled = true;
        }

        private void OnOtherMenuOpened()
        {
            _menuCanvas.enabled = false;
        }

        public void OpenSettings()
        {
            PlayButtonClick();
            MenuEvents.RaiseSettingsMenuOpened();
        }

        public void OpenCredits()
        {
            PlayButtonClick();
            MenuEvents.RaiseCreditsOpened();
        }

        public void StartGame()
        {
            PlayButtonClick();
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.WorldOne);
            SceneManager.LoadSceneAsync(FIRSTWORLD_SCENE_PARAM);
        }

        private void PlayButtonClick()
        {
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0001_ButtonClick);
        }

        #endregion
    }
}
