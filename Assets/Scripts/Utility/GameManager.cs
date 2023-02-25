using System.Collections;
using UnityEngine;
using EnumCollection;
using PeggleWars.Orbs.OrbActions;
using PeggleWars.Audio;
using PeggleWars.TurnManagement;
using UnityEngine.Events;

namespace PeggleWars
{
    public class GameManager : MonoBehaviour
    {
        #region Fields and Properties

        public static GameManager Instance { get; private set; }
        private AudioManager _audioManager;
        private TurnManager _turnManager;
        public UnityEvent LevelVictory;

        private GameState _gameState;

        public GameState GameState
        {
            get { return _gameState; }
            private set { _gameState = value; }
        }

        private int _turn = 1;

        public int Turn
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
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        private void Start()
        {
            _audioManager = AudioManager.Instance;
            _turnManager = TurnManager.Instance;
            StartCoroutine(WaitThenChangeState(GameState.MainMenu));
            _turnManager.EndEnemyTurn += OnEndEnemyTurn;
        }

        private void OnDisable()
        {
            _turnManager.EndEnemyTurn -= OnEndEnemyTurn;
        }

        public IEnumerator SwitchState(GameState state)
        {
            _gameState = state;

            switch (state)
            {
                case (GameState.MainMenu):
                    yield return new WaitForSeconds(1);
                    StartCoroutine(SwitchState(GameState.CardHandling));
                    break;

                case (GameState.CardHandling):
                    _turnManager.RaiseStartCardTurn();
                    _audioManager.PlayGameTrack(Track._0001_LevelOne);
                    _audioManager.FadeGameTrack(Track._0001_LevelOne, Fade.In);
                    break;

                case (GameState.Shooting):
                    //No action, other scripts may depend on this state
                    break;

                case (GameState.PlayerActions):                 
                    yield return StartCoroutine(OrbActionManager.Instance.HandleAllOrbEffects());
                    //OrbActionManager goes to EnemyTurn in Coroutine
                    break;

                case (GameState.EnemyTurn):
                    TurnManager.Instance.RaiseStartEnemyTurn();
                    break;

                case (GameState.LevelWon):
                    LevelVictory?.Invoke();
                    Debug.Log("You won the level!");
                    break;

                case (GameState.GameOver):
                    break;

                case (GameState.Quit):
                    break;

            }
        }
        
        public void EndCardTurnButton()
        {
            _audioManager.PlaySoundEffectOnce(SFX._0001_ButtonClick);

            if (_gameState == GameState.CardHandling)
            {
                _turnManager.RaiseEndCardTurn();
                StartCoroutine(SwitchState(GameState.Shooting));
            }
        }

        private IEnumerator WaitThenChangeState(GameState state)
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(Instance.SwitchState(state));
        }

        private void OnEndEnemyTurn()
        {
            StartCoroutine(SwitchState(GameState.CardHandling));
        }

        #endregion
    }
}
