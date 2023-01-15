using PeggleWars.TurnManagement;
using PeggleWars;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleAttacks.AttackManager
{
    public class PlayerAttackManager : MonoBehaviour
    {
        #region Fields and Properties

        public static PlayerAttackManager Instance { get; private set; }
        private TurnManager _cardTurnManager;

        private List<float> _damageModificationsForTurn = new();

        private float _damageModifierTurn = 1;

        public float DamageModifierTurn
        {
            get { return _damageModifierTurn; }
            private set { _damageModifierTurn = value; }
        }

        #endregion

        #region Public Functions

        public float CalculateModifier()
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

        public void ModifiyDamage(float modifier)
        {
            _damageModificationsForTurn.Add(modifier);
            _damageModifierTurn = CalculateModifier();
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

        private void Start()
        {
            Instance._cardTurnManager = GameManager.Instance.GetComponent<TurnManager>();
            Instance._cardTurnManager.StartCardTurn += OnCardTurnStart;
        }

        private void OnDisable()
        {
            Instance._cardTurnManager.StartCardTurn -= OnCardTurnStart;
        }

        private void OnCardTurnStart()
        {
            _damageModificationsForTurn.Clear();
            _damageModifierTurn = 1;
        }
        #endregion
    }
}
