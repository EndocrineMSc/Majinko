using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using UnityEngine.Events;
using PeggleWars.TurnManagement;

namespace PeggleWars.Orbs
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

            OrbEvents.Instance.OrbEffectEnd?.AddListener(OnOrbEffectEnd);
            TurnManager.Instance.StartPlayerAttackTurn?.AddListener(OnPlayerTurnStart);            
        }

        private void OnDisable()
        {
            OrbEvents.Instance.OrbEffectEnd?.RemoveListener(OnOrbEffectEnd);
            TurnManager.Instance.StartPlayerAttackTurn?.RemoveListener(OnPlayerTurnStart);
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

        private void OnPlayerTurnStart()
        {
            StartCoroutine(CheckOrbActions());
        }

        private void OnOrbEffectEnd()
        {
            StartCoroutine(CheckOrbActions());
        }

        private IEnumerator CheckOrbActions()
        {
            if (_orbActions.Count > 0)
            {
                Orb orb = _orbActions[0];
                StartCoroutine(HandleOrbEffect(orb));
                if (_orbActions.Count == 1)
                {
                    _orbActions.Clear();
                }
                else
                {
                    _orbActions.RemoveAt(0);
                }               
            }
            else
            {
                yield return new WaitForSeconds(2f);
                StartCoroutine(GameManager.Instance.SwitchState(GameState.EnemyTurn));
            }
        }

        private IEnumerator HandleOrbEffect(Orb orb)
        {
            yield return StartCoroutine(orb.OrbEffect());
            Destroy(orb.gameObject);
        }

        internal void AddOrbToActionList(Orb orb)
        {
            Orb tempOrb = Instantiate(orb, new Vector2(_actionOrbSpawn.x + (_xOrbOffset * _orbActions.Count), _actionOrbSpawn.y), Quaternion.identity);
            _orbActions.Add(tempOrb);
        }

        #endregion
    }
}
