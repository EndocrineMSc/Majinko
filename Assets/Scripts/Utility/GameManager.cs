using System.Collections;
using UnityEngine;
using EnumCollection;
using PeggleWars.Audio;
using PeggleWars.TurnManagement;
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
        private TurnManager _turnManager;

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
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            _audioManager = AudioManager.Instance;
            _turnManager = TurnManager.Instance;
            //StartCoroutine(WaitThenChangeState(GameState.MainMenu));
            TurnManager.Instance.EndEnemyTurn?.AddListener(OnEndEnemyTurn);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        internal IEnumerator SwitchState(GameState state)
        {
            _gameState = state;

            switch (state)
            {
                case (GameState.MainMenu):
                    yield return new WaitForSeconds(1);
                    StartCoroutine(SwitchState(GameState.CardHandling));
                    break;

                case (GameState.LevelSetup):
                    break;

                case (GameState.CardHandling):
                    TurnManager.Instance.StartCardTurn?.Invoke();
                    _audioManager.PlayGameTrack(Track._0001_LevelOne);
                    _audioManager.FadeGameTrack(Track._0001_LevelOne, Fade.In);
                    break;

                case (GameState.Shooting):
                    //No action, other scripts may depend on this state
                    break;

                case (GameState.PlayerActions):
                    TurnManager.Instance.StartPlayerAttackTurn?.Invoke();
                    //OrbActionManager goes to EnemyTurn
                    break;

                case (GameState.EnemyTurn):
                    TurnManager.Instance.StartEnemyTurn?.Invoke();
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
                StartCoroutine(WaitThenChangeState(GameState.LevelSetup));
            }
        }
        
        internal void EndCardTurnButton()
        {
            _audioManager.PlaySoundEffectOnce(SFX._0001_ButtonClick);

            if (_gameState == GameState.CardHandling)
            {
                TurnManager.Instance.EndCardTurn?.Invoke();
                StartCoroutine(SwitchState(GameState.Shooting));
            }
        }

        private IEnumerator WaitThenChangeState(GameState state)
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(Instance.SwitchState(state));
        }

        private void OnEndEnemyTurn()
        {
            StartCoroutine(SwitchState(GameState.CardHandling));
        }

        #endregion
    }
}
