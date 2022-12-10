using PeggleAttacks.Player;
using System.Collections;
using UnityEngine;
using EnumCollection;
using PeggleOrbs.OrbActions;

namespace PeggleOrbs.TransientOrbs.ManaBlitzOrb
{
    public class ManaBlitzOrb : TransientOrb
    {
        #region Fields

        [SerializeField] private PlayerAttack _manaBlitz;

        #endregion

        #region Properties

        #endregion

        #region Public Functions
        public override IEnumerator OrbEffect()
        {
            StartCoroutine(base.OrbEffect());
            _manaBlitz.ShootAttack(_manaBlitz);
            yield return new WaitForSeconds(0.2f);
        }

        #endregion

        #region Protected Functions

        //will be called OnCollisionEnter2D from parent
        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
            OrbActionManager.Instance.AddOrb(this);
        }

        #endregion

    }
}
