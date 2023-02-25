using UnityEngine;
using EnumCollection;
using PeggleWars.TurnManagement;

namespace PeggleWars.Shots
{
    /// <summary>
    /// Class for instantiating new shots when the old ones are gone. Also keeps track of any modifications
    /// to the ball made by cards or other effects.
    /// </summary>
    public class ShotManager : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private BasicShot _basicShot;
        private BasicShot _currentBall;
        private TurnManager _cardTurnManager;
        private bool _ballActive;

        private int _maxNumberOfIndicators;
        public int NumberOfIndicators { get => _maxNumberOfIndicators; set => _maxNumberOfIndicators = value; }

        private int _maxIndicatorCollisions;
        public int MaxIndicatorCollisions
        {
            get { return _maxIndicatorCollisions; }
            set { _maxIndicatorCollisions = value; }
        }

        private int _maxNumberOfIndicatorsBaseline = 3;
        private int _maxIndicatorsCollisionsBaseline = 1;

        public static ShotManager Instance { get; private set; }

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
        }

        private void Start()
        {
            _cardTurnManager = TurnManager.Instance;
            _cardTurnManager.StartCardTurn += OnStartCardTurn;
        }

        //Reset all temporary modifications to the shot
        private void OnStartCardTurn()
        {
            Instance.MaxIndicatorCollisions = _maxIndicatorsCollisionsBaseline;
            Instance.NumberOfIndicators = _maxNumberOfIndicatorsBaseline;
        }

        private void OnDisable()
        {
            _cardTurnManager.StartCardTurn -= OnStartCardTurn;
        }

        // Update is called once per frame
        private void Update()
        {
            if (GameManager.Instance.GameState == GameState.Shooting)
            {
                if (!_ballActive)
                {
                    SpawnBall();
                }

                if (_currentBall.DestroyBall)
                {
                    Destroy(_currentBall.gameObject);
                    _ballActive = false;
                    StartCoroutine(GameManager.Instance.SwitchState(GameState.PlayerActions));
                }
            }
        }

        private void SpawnBall()
        {
            Vector2 spawnScreenPosition = new Vector2 (Screen.width / 2 , Screen.height - (Screen.height / 4));
            Vector2 spawnWorldPosition = Camera.main.ScreenToWorldPoint(spawnScreenPosition);
            
            _currentBall = Instantiate(_basicShot, spawnWorldPosition, Quaternion.identity);
            _ballActive = true;
        }

        #endregion
    }
}
