using UnityEngine;
using PeggleAttacks.AttackManager;

namespace PeggleWars.ManaManagement
{
    internal class RottedMana : Mana
    {
        #region Fields

        private PlayerAttackManager _playerAttackManager;
        private readonly float _attackModifier = 0.894f;

        #endregion

        #region Functions

        private void Start()
        {
            _playerAttackManager = PlayerAttackManager.Instance;
        }

        private void OnDestroy()
        {
            _playerAttackManager.ModifiyDamage(_attackModifier);
        }

        #endregion
    }
}
