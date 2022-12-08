using PeggleAttacks.Player;
using System.Collections;
using UnityEngine;
using EnumCollection;

namespace PeggleOrbs.TransientOrbs.ManaBlitzOrb
{
    public class ManaBlitzOrb : TransientOrb
    {
        #region Fields

        [SerializeField] private PlayerAttack _manaBlitz;

        #endregion

        #region Properties

        #endregion

        #region Protected Functions

        //will be called OnCollisionEnter2D from parent
        protected override void OrbEffect()
        {
            base.OrbEffect();
            _manaBlitz.ShootAttack(transform.position, _manaBlitz);
        }

        #endregion

    }
}
