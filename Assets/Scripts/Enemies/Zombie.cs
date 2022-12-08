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

        #region Properties

        #endregion

        #region Public Functions

        public override void LoseHealth(int damage)
        {
            //ToDo: Spawn in Rotted Mana Orbs
            base.LoseHealth(damage);
        }

        #endregion

        #region Protected Functions


        #endregion

        #region IEnumerators

        #endregion
    }
}
