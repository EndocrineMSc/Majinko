using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.TurnManagement;
using Characters;
using Attacks;

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
        private bool _attacksFinished;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            _actionOrbSpawn = SetActionOrbSpawn();
            _xOrbOffset = GetXOrbOffsSet();           
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

        private void HandleOrbAction()
        {
            StartCoroutine(DelayedOrbEffect());
        }

        private IEnumerator DelayedOrbEffect()
        {
            yield return new WaitForSeconds(0.5f);
            if (_orbActions.Count > 0)
            {
                var orb = _orbActions[0];
                StartCoroutine(orb.OrbEffect());
                _orbActions.RemoveAt(0);
            }
            else
            {
                if (!_attacksFinished)
                    StartCoroutine(StartNextPhaseWithDelay());
            }
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

        internal void AddOrbToActionList(Orb orb)
        {
            Orb tempOrb = Instantiate(orb, new Vector2(_actionOrbSpawn.x + (_xOrbOffset * _orbActions.Count), _actionOrbSpawn.y), Quaternion.identity);
            tempOrb.SetActionOrbInactive();
            _orbActions.Add(tempOrb);
        }

        private IEnumerator StartNextPhaseWithDelay()
        {
            _attacksFinished = true;
            yield return new WaitForSeconds(1);
            PhaseManager.Instance.StartEnemyPhase();
            _attacksFinished = false;
        }

        #endregion
    }
}
