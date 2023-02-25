using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;


namespace PeggleWars.Orbs.OrbActions
{
    /// <summary>
    /// This class handles all effects of orbs with effects during the playerturn (as opposed to orbs with instant effects).
    /// Will switch the GameState to the enemies turn after handling all orb effects.
    /// </summary>
    public class OrbActionManager : MonoBehaviour
    {
        #region Fields and Properties

        public static OrbActionManager Instance { get; private set; }
        private List<Orb> _orbActions = new();
        private int _orbCounter = 0;

        #endregion

        #region Public Functions
        public IEnumerator HandleAllOrbEffects()
        {
            foreach (Orb orb in _orbActions)
            {
                yield return StartCoroutine(orb.OrbEffect());
                Destroy(orb.gameObject);
                yield return new WaitForSeconds(0.5f);
            }

            _orbActions.Clear();
            ResetOrbCounter();

            yield return new WaitForSeconds(2f);
            StartCoroutine(GameManager.Instance.SwitchState(GameState.EnemyTurn));
        }

        public void AddOrbToActionList(Orb orb)
        {
            Orb tempOrb = Instantiate(orb, new Vector2(11.3f, 3.8f - (0.5f * _orbCounter)), Quaternion.identity);
            _orbActions.Add(tempOrb);
            _orbCounter++;
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

        private void ResetOrbCounter()
        {
            _orbCounter = 0;
        }

        #endregion
    }
}
