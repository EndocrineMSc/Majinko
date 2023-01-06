using UnityEngine;
using EnumCollection;
using PeggleWars.TurnManagement;

namespace PeggleWars.Shots
{
    public class ShotManager : MonoBehaviour
    {

        #region Fields and Properties

        [SerializeField] private BasicShot _basicShot;
        private BasicShot _currentBall;
        private CardTurnManager _cardTurnManager;
        private bool _ballActive;

        private int _maxNumberOfIndicators;
        public int NumberOfIndicators { get => _maxNumberOfIndicators; set => _maxNumberOfIndicators = value; }

        private int _maxIndicatorCollisions;
        public int MaxIndicatorCollisions
        {
            get { return _maxIndicatorCollisions; }
            set { _maxIndicatorCollisions = value; }
        }

        private int _staticMaxNumberOfIndicators = 3;
        private int _staticMaxIndicatorsCollisions = 1;

        public static ShotManager Instance { get; private set; }

        #endregion

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
            _cardTurnManager = CardTurnManager.Instance;
            _cardTurnManager.StartCardTurn += OnStartCardTurn;
        }

        private void OnStartCardTurn()
        {
            Instance.MaxIndicatorCollisions = _staticMaxIndicatorsCollisions;
            Instance.NumberOfIndicators = _staticMaxNumberOfIndicators;
        }

        private void OnDisable()
        {
            _cardTurnManager.StartCardTurn -= OnStartCardTurn;
        }

        // Update is called once per frame
        void Update()
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
    }
}
