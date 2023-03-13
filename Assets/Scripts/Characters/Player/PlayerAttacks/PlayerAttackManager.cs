using PeggleWars.TurnManagement;
using PeggleWars;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleAttacks.AttackManager
{
    internal class PlayerAttackManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static PlayerAttackManager Instance { get; private set; }
        private TurnManager _cardTurnManager;

        private List<float> _damageModificationsForTurn = new();

        private float _damageModifierTurn = 1;

        internal float DamageModifierTurn
        {
            get { return _damageModifierTurn; }
            private set { _damageModifierTurn = value; }
        }

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
            TurnManager.Instance.StartCardTurn?.AddListener(OnCardTurnStart);
        }

        private void OnDisable()
        {
            TurnManager.Instance.StartCardTurn?.RemoveListener(OnCardTurnStart);
        }

        internal float CalculateModifier()
        {
            float finalDamageModifier = 1;
            float damageModifierSum = 0;

            if (_damageModificationsForTurn.Count > 0)
            {
                foreach (float modifier in _damageModificationsForTurn)
                {
                    damageModifierSum += modifier;
                }

                float damageModifierMean = damageModifierSum / _damageModificationsForTurn.Count;
                finalDamageModifier *= damageModifierMean;
            }

            return finalDamageModifier;
        }

        internal void ModifiyDamage(float modifier)
        {
            _damageModificationsForTurn.Add(modifier);
            _damageModifierTurn = CalculateModifier();
        }

        private void OnCardTurnStart()
        {
            _damageModificationsForTurn.Clear();
            _damageModifierTurn = 1;
        }

        #endregion
    }
}
