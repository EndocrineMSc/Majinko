using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.TurnManagement;

namespace Orbs
{

    internal class OrbActionManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static OrbActionManager Instance { get; private set; }
        private List<Orb> _orbActions = new();

        private Vector2 _actionOrbSpawn;
        private float _xOrbOffset;
        private readonly string SPAWNPOINT_PARAM = "ActionOrbSpawn";

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
        }

        private void Start()
        {
            _actionOrbSpawn = SetActionOrbSpawn();
            _xOrbOffset = GetXOrbOffsSet();           
        }

        private void OnEnable()
        {
            LevelPhaseEvents.OnStartPlayerAttackPhase += OnPlayerPhaseStart;
        }

        private void OnDisable()
        {
            LevelPhaseEvents.OnStartPlayerAttackPhase -= OnPlayerPhaseStart;
        }

        private Vector2 SetActionOrbSpawn()
        {
            GameObject spawnPoint = GameObject.FindGameObjectWithTag(SPAWNPOINT_PARAM);
            Vector2 returnVector = spawnPoint.transform.position;
            return returnVector;
        }

        private float GetXOrbOffsSet()
        {
            GameObject spawnPoint = GameObject.FindGameObjectWithTag(SPAWNPOINT_PARAM);
            SpriteRenderer spriteRenderer = spawnPoint.GetComponent<SpriteRenderer>();
            float xOffSet = spriteRenderer.bounds.size.x;
            return xOffSet;
        }

        private void OnPlayerPhaseStart()
        {
            StartCoroutine(CheckOrbActions());
        }

        private IEnumerator CheckOrbActions()
        {
            foreach (Orb orb in _orbActions)
            {
                yield return StartCoroutine(orb.OrbEffect());
                Destroy(orb.gameObject);
                yield return new WaitForSeconds(0.2f);
            }
            _orbActions.Clear();
            yield return new WaitForSeconds(2f);
            PhaseManager.Instance.StartEnemyPhase();
        }

        internal void AddOrbToActionList(Orb orb)
        {
            Orb tempOrb = Instantiate(orb, new Vector2(_actionOrbSpawn.x + (_xOrbOffset * _orbActions.Count), _actionOrbSpawn.y), Quaternion.identity);
            tempOrb.SetActionOrbInactive();
            _orbActions.Add(tempOrb);
        }
        #endregion
    }
}
