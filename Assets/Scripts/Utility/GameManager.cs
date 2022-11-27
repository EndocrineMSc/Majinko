using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using Monsters;
using Player;

namespace PeggleWars
{
    public class GameManager : MonoBehaviour
    {

        #region Fields

        public static GameManager Instance { get; private set; }
        private MonsterManager MonsterManager;

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
                    Vampire.Nosferatu.ShootBat();
                    yield return new WaitForSeconds(2f);
                    StartCoroutine(WaitForStateChange(State.MonsterTurn));
                    break;

                case (State.Shooting):
                    break;

                case (State.MonsterTurn):
                    GameObject[] _monsterList;
                    _monsterList = GameObject.FindGameObjectsWithTag("Monster");
                    if (_monsterList.Length < 4)
                    {
                        MonsterManager.SpawnZombie();
                    }
                    _monsterList = GameObject.FindGameObjectsWithTag("Monster");
                    Debug.Log("Start of Coroutine");
                    yield return StartCoroutine(MonsterManager.MoveRightMonsters(_monsterList));
                    MonsterManager.MeleeMonstersAttack(_monsterList);
                    _turn++;
                    Vampire.Nosferatu.Damage = 0;
                    StartCoroutine(WaitForStateChange(State.Shooting));
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
            Instance = this;           

            StartCoroutine(Instance.SwitchState(State.Shooting));
            MonsterManager = GetComponent<MonsterManager>();
        }

        #endregion

        #region IEnumerators

        private IEnumerator WaitForStateChange(State state)
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(Instance.SwitchState(state));
        }

        #endregion
    }
}
