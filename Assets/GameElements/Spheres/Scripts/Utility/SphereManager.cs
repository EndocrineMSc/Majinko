using UnityEngine;
using EnumCollection;
using Utility.TurnManagement;
using System.Collections.Generic;
using System.Linq;

namespace Spheres
{
    internal class SphereManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static SphereManager Instance { get; private set; }

        internal int NumberOfIndicators { get; set; }       
        internal int MaxIndicatorCollisions { get; set; }
        private readonly int _maxNumberOfIndicatorsBaseline = 3;
        private readonly int _maxIndicatorsCollisionsBaseline = 1;

        internal List<Sphere> AllSpheres { get; private set; }
        internal Sphere SphereToBeSpawned { get; private set; }
        private Sphere _currentSphere;

        [SerializeField] private AllSpheresCollection _allSpheresCollection;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            AllSpheres = _allSpheresCollection.AllSpheres.ToList();
            AllSpheres = AllSpheres.OrderBy(sphere => sphere.name).ToList();
            SphereToBeSpawned = AllSpheres[(int)SphereType.BasicSphere];
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
            SphereToBeSpawned = AllSpheres[(int)SphereType.BasicSphere];
            SpawnSphere();
        }

        private void SpawnSphere()
        {
            Vector2 spawnScreenPosition = new(Screen.width / 2 , Screen.height - (Screen.height / 4));
            Vector2 spawnWorldPosition = Camera.main.ScreenToWorldPoint(spawnScreenPosition);
            
            _currentSphere = Instantiate(SphereToBeSpawned, spawnWorldPosition, Quaternion.identity);
        }

        internal void SetSphereToBeSpawned(Sphere sphere)
        {
            SphereToBeSpawned = sphere;
            ReplaceSphere();
        }

        private void ReplaceSphere()
        {
            if(_currentSphere != null)
            {
                Destroy(_currentSphere.gameObject);
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
