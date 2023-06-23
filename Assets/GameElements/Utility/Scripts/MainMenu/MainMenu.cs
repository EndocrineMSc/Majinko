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
        private readonly string TITLE_PARAM = "GameTitle";
        private readonly string TUTORIAL_SCENE_PARAM = "Tutorial";
        private readonly string FIRSTWORLD_SCENE_PARAM = "LoadingScreen";
        private readonly float _startButtonXAnimationDistance = 200f;
        private readonly float _settingsButtonXAnimationDistance = 400;
        private readonly float _creditsButtonXAnimationDistance = 600;
        private readonly float _howToPlayButtonXAnimationDistance = -750;
        private readonly float _buttonAnimationDuration = 1f;
        private readonly float _titleYAnimationDistance = 150f;
        private readonly float _titleAnimationDuration = 2f;

        private GameObject _startButton;
        private GameObject _settingsButton;
        private GameObject _creditsButton;
        private GameObject _tutorialButton;
        private Image _gameTitle;

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
            _gameTitle = GameObject.FindGameObjectWithTag(TITLE_PARAM).GetComponent<Image>();

            StartCoroutine(AnimateMenuUI());

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
            SceneManager.LoadSceneAsync(TUTORIAL_SCENE_PARAM);
        }


        private IEnumerator AnimateMenuUI()
        {
            AnimateTitle();
            yield return new WaitForSeconds(_titleAnimationDuration);
            AnimateButton(_startButton, _startButtonXAnimationDistance);
            AnimateButton(_tutorialButton, _howToPlayButtonXAnimationDistance);
            yield return new WaitForSeconds(0.2f);
            AnimateButton(_settingsButton, _settingsButtonXAnimationDistance);
            yield return new WaitForSeconds(0.2f);
            AnimateButton(_creditsButton, _creditsButtonXAnimationDistance);
            yield return new WaitForSeconds(0.2f);
        }

        private void AnimateButton(GameObject button, float buttonXAnimationDistance)
        {
            button.GetComponent<RectTransform>().DOLocalMoveX(buttonXAnimationDistance, _buttonAnimationDuration).SetEase(Ease.OutBounce);
        }

        private void AnimateTitle()
        {
            _gameTitle.rectTransform.DOLocalMoveY(_titleYAnimationDistance, _titleAnimationDuration).SetEase(Ease.Linear);
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
            SceneManager.LoadSceneAsync(FIRSTWORLD_SCENE_PARAM);
        }

        private void PlayButtonClick()
        {
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0001_ButtonClick);
        }

        #endregion
    }
}
