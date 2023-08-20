using Utility.TurnManagement;
using System.Collections.Generic;
using UnityEngine;
using Characters;

namespace Attacks
{
    internal class PlayerAttackDamageManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static PlayerAttackDamageManager Instance { get; private set; }

        private List<float> _damageModificationsForTurn = new();
        private readonly float _sicknessModifier = 0.894f;

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
            LevelPhaseEvents.OnStartEnemyPhase += OnStartEnemyPhase;
        }

        private void OnDisable()
        {
            LevelPhaseEvents.OnStartCardPhase -= OnCardPhaseStart;
            LevelPhaseEvents.OnEndCardPhase -= OnCardPhaseEnd;
            LevelPhaseEvents.OnStartEnemyPhase -= OnStartEnemyPhase;
        }

        internal float CalculateModifier()
        {
            float finalDamageModifier = 1;

            if (PlayerConditionTracker.SicknessStacks > 0)
            {
                var sicknessModifier = Mathf.Pow(_sicknessModifier, PlayerConditionTracker.SicknessStacks);
                finalDamageModifier *= sicknessModifier;
            }

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
            DamageModifierTurn = 1;
        }

        private void OnCardPhaseEnd()
        {
            DamageModifierTurn = CalculateModifier();
            _damageModificationsForTurn.Clear();
        }
        
        private void OnStartEnemyPhase()
        {
            PlayerConditionTracker.ResetSicknessStacks();            
        }

        #endregion
    }
}
