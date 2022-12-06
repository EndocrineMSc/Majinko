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

        #endregion

        private new void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
            _manaBlitz.ShootAttack(_position);
            StartCoroutine(nameof(DestroyThisObject));
        }

        private IEnumerator DestroyThisObject()
        {
            yield return new WaitForSeconds(1f);
            Destroy(this);
        }
    }
}
