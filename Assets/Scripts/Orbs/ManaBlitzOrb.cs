using PeggleAttacks.Player;
using System.Collections;
using UnityEngine;
using EnumCollection;

namespace PeggleOrbs.ManaBlitzOrb
{
    public class ManaBlitzOrb : Orb
    {
        #region Fields

        [SerializeField] private PlayerAttack _manaBlitz;

        #endregion

        #region Properties

        protected int _damage = 20;

        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        #endregion

        private new void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
            _manaBlitz.ShootAttack(this, PlayerAttackTarget.FirstEnemy, _damage);
            StartCoroutine(nameof(DestroyThisObject));
        }

        private IEnumerator DestroyThisObject()
        {
            yield return new WaitForSeconds(1f);
            Destroy(this);
        }
    }
}
