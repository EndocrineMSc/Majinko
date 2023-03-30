using UnityEngine;
using EnumCollection;
using PeggleWars.TurnManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace PeggleWars.Spheres
{
    internal class SphereManager : MonoBehaviour
    {
        #region Fields and Properties

        private List<Sphere> _allShots = new();

        private Sphere _currentShot;

        private int _maxNumberOfIndicators;
        internal int NumberOfIndicators { get => _maxNumberOfIndicators; set => _maxNumberOfIndicators = value; }
        
        private Sphere _spawnShot;
        internal Sphere ShotToBeSpawned
        {
            get { return _spawnShot; }
            private set { _spawnShot = value; }
        }

        private int _maxIndicatorCollisions;
        internal int MaxIndicatorCollisions
        {
            get { return _maxIndicatorCollisions; }
            set { _maxIndicatorCollisions = value; }
        }

        private readonly int _maxNumberOfIndicatorsBaseline = 3;
        private readonly int _maxIndicatorsCollisionsBaseline = 1;

        internal static SphereManager Instance { get; private set; }

        private string RESOURCE_LOAD_PARAM = "SpherePrefabs";

        internal List<Sphere> AllShots
        {
            get { return _allShots; }
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
            }

            _allShots = Resources.LoadAll<Sphere>(RESOURCE_LOAD_PARAM).ToList();
            _spawnShot = _allShots[(int)ShotType.BasicShot];
            ResetIndicatorNumbers();
        }

        private void Start()
        {
            TurnManager.Instance.StartCardTurn?.AddListener(OnStartCardTurn);           
        }

        private void OnDisable()
        {
            TurnManager.Instance.StartCardTurn?.RemoveListener(OnStartCardTurn);
        }

        //Reset all temporary modifications to the shot
        private void OnStartCardTurn()
        {
            ResetIndicatorNumbers();
            _spawnShot = _allShots[(int)ShotType.BasicShot];
            SpawnShot();
        }

        private void SpawnShot()
        {
            Vector2 spawnScreenPosition = new(Screen.width / 2 , Screen.height - (Screen.height / 4));
            Vector2 spawnWorldPosition = Camera.main.ScreenToWorldPoint(spawnScreenPosition);
            
            _currentShot = Instantiate(_spawnShot, spawnWorldPosition, Quaternion.identity);
        }

        internal void SetShotToBeSpawned(Sphere shot)
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

        private void ResetIndicatorNumbers()
        {
            MaxIndicatorCollisions = _maxIndicatorsCollisionsBaseline;
            NumberOfIndicators = _maxNumberOfIndicatorsBaseline;
        }

        #endregion
    }
}
