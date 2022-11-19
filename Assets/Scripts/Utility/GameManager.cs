using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using Monsters;
using Player;

namespace Vampeggle
{
    public class GameManager : MonoBehaviour
    {

        #region Fields

        public static GameManager Instance;
        private MonsterManager MonsterManager;
        GameObject[] _monsterList; 

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
            switch (state)
            {
                case (State.MainMenu):
                    _gameState = State.MainMenu;
                    break;

                case (State.Shooting):
                    _gameState = State.Shooting;
                    break;

                case (State.PlayerTurn):
                    _gameState = State.PlayerTurn;
                    Vampire.Nosferatu.ShootBat();
                    yield return new WaitForSeconds(2f);
                    StartCoroutine(WaitForStateChange(State.MonsterTurn));
                    break;

                case (State.MonsterTurn):
                    _gameState = State.MonsterTurn;
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
                    _gameState = State.GameOver;
                    break;

                case (State.Quit):
                    _gameState = State.Quit;
                    break;

            }
            yield break;
        }

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            StartCoroutine(Instance.SwitchState(State.Shooting));
            MonsterManager = GetComponent<MonsterManager>();
        }


        private IEnumerator WaitForStateChange(State state)
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(Instance.SwitchState(state));
        }   
    }
}
