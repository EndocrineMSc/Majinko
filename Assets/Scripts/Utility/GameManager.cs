using System.Collections;
using UnityEngine;
using EnumCollection;
using PeggleWars.Orbs.OrbActions;
using PeggleWars.Audio;
using PeggleWars.TurnManagement;

namespace PeggleWars
{
    public class GameManager : MonoBehaviour
    {
        #region Fields and Properties

        public static GameManager Instance { get; private set; }
        private AudioManager _audioManager;
        private TurnManager _turnManager;

        private State _gameState;

        public State GameState
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
            StartCoroutine(WaitThenChangeState(State.MainMenu));
            _turnManager.EndEnemyTurn += OnEndEnemyTurn;
        }

        private void OnDisable()
        {
            _turnManager.EndEnemyTurn -= OnEndEnemyTurn;
        }

        public IEnumerator SwitchState(State state)
        {
            _gameState = state;

            switch (state)
            {
                case (State.MainMenu):
                    yield return new WaitForSeconds(1);
                    StartCoroutine(SwitchState(State.CardHandling));
                    break;

                case (State.CardHandling):
                    _turnManager.RaiseStartCardTurn();
                    _audioManager.PlayGameTrack(Track._0001_LevelOne);
                    _audioManager.FadeGameTrack(Track._0001_LevelOne, Fade.In);
                    break;

                case (State.Shooting):
                    //No action, other scripts may depend on this state
                    break;

                case (State.PlayerActions):                 
                    yield return StartCoroutine(OrbActionManager.Instance.HandleAllOrbEffects());
                    //OrbActionManager goes to EnemyTurn in Coroutine
                    break;

                case (State.EnemyTurn):
                    TurnManager.Instance.RaiseStartEnemyTurn();
                    break;

                case (State.GameOver):
                    break;

                case (State.Quit):
                    break;

            }
        }
        
        public void EndCardTurnButton()
        {
            _audioManager.PlaySoundEffectOnce(SFX._0001_ButtonClick);

            if (_gameState == State.CardHandling)
            {
                _turnManager.RaiseEndCardTurn();
                StartCoroutine(SwitchState(State.Shooting));
            }
        }

        private IEnumerator WaitThenChangeState(State state)
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(Instance.SwitchState(state));
        }

        private void OnEndEnemyTurn()
        {
            StartCoroutine(SwitchState(State.CardHandling));
        }

        #endregion
    }
}
