using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using Enemies;
using Player;

namespace PeggleWars
{
    public class GameManager : MonoBehaviour
    {

        #region Fields

        public static GameManager Instance { get; private set; }
        private EnemyManager _enemyManager;

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
            _gameState= state;

            switch (state)
            {
                case (State.MainMenu):
                    break;

                case (State.PlayerTurn):
                    yield return new WaitForSeconds(2f);
                    StartCoroutine(WaitThenChangeState(State.MonsterTurn));
                    break;

                case (State.PlayerShooting):
                    break;

                case (State.MonsterTurn):
                    
                    if (_enemyManager.Enemies.Count < 4)
                    {
                        _enemyManager.SpawnGroundEnemy(EnemyType.CloakedZombie);
                    }
                    yield return StartCoroutine(_enemyManager.MoveRightEnemies());

                    _enemyManager.MeleeEnemiesAttack();
                    _enemyManager.RangedEnemiesAttack();

                    StartCoroutine(WaitThenChangeState(State.PlayerShooting));
                    break;

                case (State.GameOver):
                    break;

                case (State.Quit):
                    break;

            }
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

            StartCoroutine(Instance.SwitchState(State.PlayerShooting));
            _enemyManager = EnemyManager.Instance;
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
