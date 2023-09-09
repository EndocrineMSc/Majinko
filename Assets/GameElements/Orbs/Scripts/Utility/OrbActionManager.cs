using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.TurnManagement;
using Attacks;

namespace Orbs
{
    public class OrbActionManager : MonoBehaviour
    {
        #region Fields and Properties

        public static OrbActionManager Instance { get; private set; }

        //Orb Pool Instantiation
        private readonly int _actionOrbPoolSize = 40;
        private List<Orb> _actionOrbPool = new();
        [SerializeField] private Orb _basicOrbPrefab;
        private GameObject _actionOrbSpawnObject;
        private Vector2 _actionOrbSpawnPosition;
        private float _xOrbOffset;
        private readonly string SPAWNPOINT_PARAM = "ActionOrbSpawn";

        //Action Orb Handling
        private List<Orb> _claimedActionOrbs = new();

        //Pool Edge Case Handling
        private int _overShootCounter = 0;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void OnEnable()
        {
            LevelPhaseEvents.OnStartPlayerAttackPhase += HandleOrbAction;
            AttackEvents.OnAttackFinished += HandleOrbAction;
        }

        private void OnDisable()
        {
            LevelPhaseEvents.OnStartPlayerAttackPhase -= HandleOrbAction;
            AttackEvents.OnAttackFinished -= HandleOrbAction;
        }

        private void Start()
        {
            _actionOrbSpawnObject = GameObject.FindGameObjectWithTag(SPAWNPOINT_PARAM);
            _actionOrbSpawnPosition = SetActionOrbSpawn();
            _xOrbOffset = GetXOrbOffsSet();
            InstantiateActionListOrbs();
        }

        private Vector2 SetActionOrbSpawn()
        {
            GameObject spawnPoint = GameObject.FindGameObjectWithTag(SPAWNPOINT_PARAM);
            Vector2 returnVector = spawnPoint.transform.position;
            return returnVector;
        }

        private float GetXOrbOffsSet()
        {
            SpriteRenderer spriteRenderer = _actionOrbSpawnObject.GetComponent<SpriteRenderer>();
            float xOffSet = spriteRenderer.bounds.size.x;
            return xOffSet;
        }

        private void InstantiateActionListOrbs()
        {
            for (int i = 0; i < _actionOrbPoolSize; i++)
            {
                var xPosition = _actionOrbSpawnPosition.x + (_xOrbOffset * i);
                Vector2 instantiatePosition = new(xPosition, _actionOrbSpawnPosition.y);
                InstantiateNewActionOrb(instantiatePosition);
            }
        }

        private void InstantiateNewActionOrb(Vector2 instantiatePosition)
        {
            var tempOrb = Instantiate(_basicOrbPrefab, instantiatePosition, Quaternion.identity);
            tempOrb.GetComponent<SpriteRenderer>().enabled = false;
            tempOrb.transform.SetParent(_actionOrbSpawnObject.transform);
            tempOrb.SetActionOrbInactive();
            _actionOrbPool.Add(tempOrb);
        }

        public void AddOrbToActionList(OrbData orbData)
        {
            HandleActionPoolOrbAvailability();

            var actionOrb = _actionOrbPool[0];
            actionOrb.SetOrbData(orbData);
            actionOrb.GetComponent<SpriteRenderer>().enabled = true;

            _actionOrbPool.RemoveAt(0);
            _claimedActionOrbs.Add(actionOrb);
        }

        private void HandleOrbAction()
        {
            if (_claimedActionOrbs.Count > 0)
            {
                var orb = _claimedActionOrbs[0];
                orb.Data.OrbEffect();
                ReturnOrbToPool(orb);
                _claimedActionOrbs.RemoveAt(0);
            }
            else
            {
                StartCoroutine(StartNextPhaseWithDelay());
            }
        }

        private void ReturnOrbToPool(Orb orb)
        {
            _actionOrbPool.Insert(0, orb);
            orb.SetOrbData(_basicOrbPrefab.Data);
            orb.GetComponent<SpriteRenderer>().enabled = false;
        }

        private void HandleActionPoolOrbAvailability()
        {
            if (_actionOrbPool.Count == 0)
            {
                _overShootCounter++;
                var xPosition = _actionOrbSpawnPosition.x + (_xOrbOffset * (_actionOrbPoolSize + _overShootCounter));
                Vector2 instantiatePosition = new(xPosition, _actionOrbSpawnPosition.y);
                InstantiateNewActionOrb(instantiatePosition);
            }
        }

        private IEnumerator StartNextPhaseWithDelay()
        {
            yield return new WaitForSeconds(1);
            PhaseManager.Instance.StartEnemyPhase();
        }

        #endregion
    }
}
