using Utility.TurnManagement;
using System.Collections.Generic;
using UnityEngine;

namespace Attacks
{
    internal class PlayerAttackDamageManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static PlayerAttackDamageManager Instance { get; private set; }

        private List<float> _damageModificationsForTurn = new();

        internal float DamageModifierTurn { get; private set; } = 1;

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
            LevelPhaseEvents.OnStartCardPhase += OnCardPhaseStart;
            LevelPhaseEvents.OnEndCardPhase += OnCardPhaseEnd;
        }

        private void OnDisable()
        {
            LevelPhaseEvents.OnStartCardPhase -= OnCardPhaseStart;
            LevelPhaseEvents.OnEndCardPhase -= OnCardPhaseEnd;
        }

        internal float CalculateModifier()
        {
            float finalDamageModifier = 1;

            if (_damageModificationsForTurn.Count > 0)
                foreach (float modifier in _damageModificationsForTurn)
                    finalDamageModifier *= modifier;

            return finalDamageModifier;
        }

        internal void ModifyPlayerDamage(float modifier)
        {
            _damageModificationsForTurn.Add(modifier);
        }

        private void OnCardPhaseStart()
        {
            _damageModificationsForTurn.Clear();
            DamageModifierTurn = 1;
        }

        private void OnCardPhaseEnd()
        {
            DamageModifierTurn = CalculateModifier();
        }

        #endregion
    }
}
