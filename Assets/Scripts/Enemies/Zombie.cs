using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleOrbs;

namespace Enemies.Zombies
{
    public class Zombie : Enemy
    {
        #region Fields

        [SerializeField] private Orb _rottedManaOrb;

        #endregion

        #region Public Functions

        public override void LoseHealth(int damage)
        {
            OrbManager.Instance.SwitchOrbs(_rottedManaOrb, 2);
            base.LoseHealth(damage);
        }

        #endregion
    }
}
