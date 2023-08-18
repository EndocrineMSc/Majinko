using Attacks;

namespace ManaManagement
{
    internal class RottedMana : Mana
    {
        #region Fields

        private PlayerAttackDamageManager _playerAttackManager;
        private readonly float _attackModifier = 0.894f;

        #endregion

        #region Functions

        private void Start()
        {
            _playerAttackManager = PlayerAttackDamageManager.Instance;
        }

        private void OnDestroy()
        {
            _playerAttackManager.ModifyPlayerDamage(_attackModifier);
        }

        #endregion
    }
}
