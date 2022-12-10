using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars;
using EnumCollection;


namespace PeggleOrbs.OrbActions
{
    public class OrbActionManager : MonoBehaviour
    {
        #region Fields

        public static OrbActionManager Instance { get; private set; }
        private List<Orb> _orbActions = new();

        private int _orbCounter = 0;

        #endregion

        #region Properties


        #endregion

        #region Public Functions

        public void AddOrb(Orb orb)
        {
            Orb tempOrb = Instantiate(orb, new Vector2(11.3f, 3.8f - (0.5f * _orbCounter)), Quaternion.identity);
            _orbActions.Add(tempOrb);
            _orbCounter++;
        }

        public void ResetOrbCounter()
        {
            _orbCounter = 0;
        }

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

        public IEnumerator HandleAllOrbEffects()
        {
            foreach (Orb orb in _orbActions)
            {
                yield return StartCoroutine(orb.OrbEffect());
            }

            foreach (Orb orb in _orbActions)
            {
                Destroy(orb.gameObject);
            }

            _orbActions.Clear();
            ResetOrbCounter();

            StartCoroutine(GameManager.Instance.SwitchState(State.EnemyTurn));
        }

        #endregion
    }
}
