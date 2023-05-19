using UnityEngine;
using EnumCollection;
using Utility.TurnManagement;
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
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            _allShots = Resources.LoadAll<Sphere>(RESOURCE_LOAD_PARAM).ToList();
            _spawnShot = _allShots[(int)SphereType.BasicSphere];
            ResetIndicatorNumbers();
        }

        private void OnEnable()
        {
            LevelPhaseEvents.OnStartCardPhase += OnStartCardPhase;
        }

        private void OnDisable()
        {
            LevelPhaseEvents.OnStartCardPhase -= OnStartCardPhase;
        }

        //Reset all temporary modifications to the shot
        private void OnStartCardPhase()
        {
            ResetIndicatorNumbers();
            _spawnShot = _allShots[(int)SphereType.BasicSphere];
            SpawnSphere();
        }

        private void SpawnSphere()
        {
            Vector2 spawnScreenPosition = new(Screen.width / 2 , Screen.height - (Screen.height / 4));
            Vector2 spawnWorldPosition = Camera.main.ScreenToWorldPoint(spawnScreenPosition);
            
            _currentShot = Instantiate(_spawnShot, spawnWorldPosition, Quaternion.identity);
        }

        internal void SetSphereToBeSpawned(Sphere shot)
        {
            _spawnShot = shot;
            ReplaceSphere();
        }

        private void ReplaceSphere()
        {
            if(_currentShot != null)
            {
                Destroy(_currentShot.gameObject);
            }
            SpawnSphere();
        }

        private void ResetIndicatorNumbers()
        {
            MaxIndicatorCollisions = _maxIndicatorsCollisionsBaseline;
            NumberOfIndicators = _maxNumberOfIndicatorsBaseline;
        }

        #endregion
    }
}
