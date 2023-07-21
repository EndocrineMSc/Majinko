using UnityEngine;
using UnityEngine.UI;
using Audio;
using DG.Tweening;
using System.Collections;

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
        private readonly string QUIT_BUTTON_PARAM = "QuitButton";
        private readonly string TUTORIAL_BUTTON_PARAM = "TutorialButton";
        private readonly string CONTINUE_BUTTON_PARAM = "ContinueButton";

        private GameObject _startButton;
        private GameObject _settingsButton;
        private GameObject _creditsButton;
        private GameObject _tutorialButton;
        private GameObject _quitButton;
        private GameObject _continueButton;

        [SerializeField] private Image _blackFadeImage;

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
            _blackFadeImage.enabled = true;
            _blackFadeImage.DOFade(0, LoadHelper.LoadDuration);
            _startButton = GameObject.FindGameObjectWithTag(START_BUTTON_PARAM);
            _settingsButton = GameObject.FindGameObjectWithTag(SETTINGS_BUTTON_PARAM);
            _creditsButton = GameObject.FindGameObjectWithTag(CREDITS_BUTTON_PARAM);
            _tutorialButton = GameObject.FindGameObjectWithTag(TUTORIAL_BUTTON_PARAM);
            _quitButton = GameObject.FindGameObjectWithTag(QUIT_BUTTON_PARAM);
            _continueButton = GameObject.FindGameObjectWithTag(CONTINUE_BUTTON_PARAM);

            _continueButton.GetComponent<Button>().onClick.AddListener(ContinueGame);
            _startButton.GetComponent<Button>().onClick.AddListener(NewGame);
            _settingsButton.GetComponent<Button>().onClick.AddListener(OpenSettings);
            _creditsButton.GetComponent<Button>().onClick.AddListener(OpenCredits);
            _tutorialButton.GetComponent<Button>().onClick.AddListener(OpenTutorial);
            _quitButton.GetComponent<Button>().onClick.AddListener(QuitGame);

            if (!ES3.KeyExists("GlobalDeck"))
                _continueButton.SetActive(false);    
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
            UtilityEvents.RaiseGameReset();
            StartCoroutine(LoadWithFade(SceneName.Tutorial));
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

        public void ContinueGame()
        {
            PlayButtonClick();
            var sceneToLoad = SceneName.WorldOne;

            if (ES3.KeyExists(LoadHelper.CURRENTSCENE_SAVE_PATH))
                sceneToLoad = ES3.Load<SceneName>(LoadHelper.CURRENTSCENE_SAVE_PATH);

            StartCoroutine(LoadWithFade(sceneToLoad));
        }

        public void NewGame()
        {
            PlayButtonClick();
            UtilityEvents.RaiseGameReset();
            StartCoroutine(LoadWithFade(SceneName.WorldOne));
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void PlayButtonClick()
        {
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0001_ButtonClick);
        }

        private IEnumerator LoadWithFade(SceneName sceneName)
        {
            _blackFadeImage.DOFade(1, LoadHelper.LoadDuration);
            yield return new WaitForSeconds(LoadHelper.LoadDuration);
            LoadHelper.LoadSceneWithLoadingScreen(sceneName);
        }

        #endregion
    }
}
