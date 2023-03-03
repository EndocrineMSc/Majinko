using UnityEngine;
using EnumCollection;
using PeggleWars.TurnManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace PeggleWars.Shots
{
    /// <summary>
    /// Class for instantiating new shots when the old ones are gone. Also keeps track of any modifications
    /// to the ball made by cards or other effects.
    /// </summary>
    public class ShotManager : MonoBehaviour
    {
        #region Fields and Properties

        private List<Shot> _allShots = new();

        private Shot _currentShot;
        private TurnManager _cardTurnManager;

        private int _maxNumberOfIndicators;
        public int NumberOfIndicators { get => _maxNumberOfIndicators; set => _maxNumberOfIndicators = value; }
        
        private Shot _spawnShot;
        public Shot ShotToBeSpawned
        {
            get { return _spawnShot; }
            private set { _spawnShot = value; }
        }

        private int _maxIndicatorCollisions;
        public int MaxIndicatorCollisions
        {
            get { return _maxIndicatorCollisions; }
            set { _maxIndicatorCollisions = value; }
        }

        private int _maxNumberOfIndicatorsBaseline = 3;
        private int _maxIndicatorsCollisionsBaseline = 1;

        public static ShotManager Instance { get; private set; }

        private string RESOURCE_LOAD_PARAM = "ShotPrefabs";

        public UnityEvent OnShotStacked;

        public List<Shot> AllShots
        {
            get { return _allShots; }
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
            }

            _allShots = Resources.LoadAll<Shot>(RESOURCE_LOAD_PARAM).ToList();
            _spawnShot = _allShots[(int)ShotType.BasicShot];
        }

        private void Start()
        {
            _cardTurnManager = TurnManager.Instance;
            _cardTurnManager.StartCardTurn += OnStartCardTurn;
        }

        //Reset all temporary modifications to the shot
        private void OnStartCardTurn()
        {
            MaxIndicatorCollisions = _maxIndicatorsCollisionsBaseline;
            NumberOfIndicators = _maxNumberOfIndicatorsBaseline;
            _spawnShot = _allShots[(int)ShotType.BasicShot];
            SpawnShot();
        }

        private void OnDisable()
        {
            _cardTurnManager.StartCardTurn -= OnStartCardTurn;
        }

        // Update is called once per frame
        private void Update()
        {
            if (GameManager.Instance.GameState == GameState.Shooting && _currentShot.DestroyBall)
            {
                Destroy(_currentShot.gameObject);
                StartCoroutine(GameManager.Instance.SwitchState(GameState.PlayerActions));
            }
        }

        private void SpawnShot()
        {
            Vector2 spawnScreenPosition = new(Screen.width / 2 , Screen.height - (Screen.height / 4));
            Vector2 spawnWorldPosition = Camera.main.ScreenToWorldPoint(spawnScreenPosition);
            
            _currentShot = Instantiate(_spawnShot, spawnWorldPosition, Quaternion.identity);
        }

        public void SetShotToBeSpawned(Shot shot)
        {
            _spawnShot = shot;
            ReplaceShot();
        }

        private void ReplaceShot()
        {
            if(_currentShot != null)
            {
                Destroy(_currentShot.gameObject);
            }
            SpawnShot();
        }

        #endregion
    }
}
