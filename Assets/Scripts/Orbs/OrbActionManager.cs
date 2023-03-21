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
            TurnManager.Instance.StartPlayerAttackTurn?.AddListener(OnPlayerTurnStart);            
        }

        private void OnDisable()
        {
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

        private IEnumerator CheckOrbActions()
        {
            foreach (Orb orb in _orbActions)
            {
                yield return StartCoroutine(orb.OrbEffect());
                Destroy(orb.gameObject);
            }
            _orbActions.Clear();
            yield return new WaitForSeconds(2f);
            StartCoroutine(GameManager.Instance.SwitchState(GameState.EnemyTurn));
        }

        internal void AddOrbToActionList(Orb orb)
        {
            Orb tempOrb = Instantiate(orb, new Vector2(_actionOrbSpawn.x + (_xOrbOffset * _orbActions.Count), _actionOrbSpawn.y), Quaternion.identity);
            tempOrb.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(DisableOrbCollider(tempOrb));
            _orbActions.Add(tempOrb);
        }

        private IEnumerator DisableOrbCollider(Orb orb)
        {
            yield return new WaitForSeconds(0.2f);
            orb.gameObject.GetComponent<Collider2D>().enabled = false;
        }

        #endregion
    }
}
