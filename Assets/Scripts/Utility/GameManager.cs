using System.Collections;
using UnityEngine;
using EnumCollection;
using Enemies;
using PeggleOrbs.OrbActions;
using PeggleWars.Audio;
using PeggleWars.TurnManagement;

namespace PeggleWars
{
    public class GameManager : MonoBehaviour
    {

        #region Fields

        public static GameManager Instance { get; private set; }
        private EnemyManager _enemyManager;
        private AudioManager _audioManager;
        private CardTurnManager _cardTurnManager;

        #endregion

        #region Properties

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

        #region Public Functions

        public IEnumerator SwitchState(State state)
        {
            _gameState = state;

            switch (state)
            {
                case (State.MainMenu):
                    yield return new WaitForSeconds(1);
                    StartCoroutine(Instance.SwitchState(State.CardHandling));
                    break;

                case (State.CardHandling):
                    Instance._cardTurnManager.RaiseStartCardTurn();
                    _audioManager.PlayGameTrack(Track.Track_0001_LevelOne);
                    _audioManager.FadeGameTrack(Track.Track_0001_LevelOne, Fade.In);
                    //End Turn Button calls State Change
                    break;

                case (State.Shooting):
                    //Player Shot calls Statechange here?
                    //Probably would be nice to work with an event here? But is there any benefit to that right now?
                    break;

                case (State.PlayerActions):                 
                    yield return StartCoroutine(OrbActionManager.Instance.HandleAllOrbEffects());
                    //OrbActionManager goes to EnemyTurn in Coroutine
                    break;

                case (State.EnemyTurn):

                    //this part should probably not be handled in here but in a separate script for now it's fine
                    if (_enemyManager.Enemies.Count < 4)
                    {
                        _enemyManager.SpawnGroundEnemy(EnemyType.CloakedZombie);
                    }
                    yield return StartCoroutine(_enemyManager.MoveRightEnemies());

                    _enemyManager.MeleeEnemiesAttack();
                    _enemyManager.RangedEnemiesAttack();

                    StartCoroutine(WaitThenChangeState(State.CardHandling));
                    break;

                case (State.GameOver):
                    break;

                case (State.Quit):
                    break;

            }
        }

        public void EndCardTurn()
        {
            Instance._cardTurnManager.RaiseEndCardTurn();
            StartCoroutine(Instance.SwitchState(State.Shooting));
        }

        #endregion

        #region Private Functions

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

        // Start is called before the first frame update
        void Start()
        {         
            _enemyManager = EnemyManager.Instance;
            _audioManager = AudioManager.Instance;
            _cardTurnManager = CardTurnManager.Instance;
            StartCoroutine(Instance.WaitThenChangeState(State.MainMenu));
        }

        #endregion

        #region IEnumerators

        private IEnumerator WaitThenChangeState(State state)
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(Instance.SwitchState(state));
        }

        #endregion
    }
}
