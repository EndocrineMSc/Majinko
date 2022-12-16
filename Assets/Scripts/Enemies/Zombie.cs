using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleOrbs;
using EnumCollection;

namespace Enemies.Zombies
{
    public class Zombie : Enemy
    {

        #region Public Functions

        protected override void HandleDeath()
        {
            base.HandleDeath();
            OrbManager.Instance.SwitchOrbs(OrbType.RottedOrb, 2);
        }

        #endregion
    }
}
