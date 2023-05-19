using System.Collections;
using UnityEngine;
using EnumCollection;
using Audio;
using Utility.TurnManagement;
using UnityEngine.Events;
using PeggleWars.Orbs;
using UnityEngine.SceneManagement;

namespace PeggleWars
{
    internal class GameManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static GameManager Instance { get; private set; }
        private AudioManager _audioManager;
        private PhaseManager _turnManager;

        [SerializeField] private GameState _gameState;

        internal GameState GameState
        {
            get { return _gameState; }
            private set { _gameState = value; }
        }

        private int _turn = 1;

        internal int Turn
        {
            get { return _turn; }
            private set { _turn = value; }
        }

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
        }

        private void Start()
        {
            _audioManager = AudioManager.Instance;
            //StartCoroutine(WaitThenChangeState(GameState.MainMenu));
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;           
        }

        internal IEnumerator SwitchState(GameState state)
        {
            _gameState = state;

            switch (state)
            {
                case (GameState.MainMenu):
                    yield return new WaitForSeconds(1);
                    StartCoroutine(SwitchState(GameState.StartLevel));
                    break;

                case (GameState.StartLevel):
                    _audioManager.PlayGameTrack(Track._0001_LevelOne);
                    _audioManager.FadeInGameTrack(Track._0001_LevelOne);
                    break;

                case (GameState.LevelWon):
                    break;

                case (GameState.NewLevel):
                    SceneManager.LoadScene("LoadingScreen");
                    break;

                case (GameState.GameOver):
                    break;

                case (GameState.Quit):
                    break;

            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name.Contains("Level"))
            {
                StartCoroutine(WaitThenChangeState(GameState.StartLevel));
            }
        }

        private IEnumerator WaitThenChangeState(GameState state)
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(Instance.SwitchState(state));
        }

        #endregion
    }

    internal enum GameState
    {
        MainMenu,
        StartLevel,
        LevelWon,
        NewLevel,
        GameOver,
        Quit,
    }
}
