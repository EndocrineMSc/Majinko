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
            if (GameManager.Instance.GameState == State.Shooting)
            {
                if (!_ballActive)
                {
                    _currentBall = Instantiate(_basicShot, new Vector3(1, 3), Quaternion.identity);
                    _ballActive = true;
                }

                if (_currentBall.DestroyBall)
                {
                    Destroy(_currentBall.gameObject);
                    _ballActive = false;
                    StartCoroutine(GameManager.Instance.SwitchState(State.PlayerActions));
                }
            }
        }

        #endregion
    }
}
