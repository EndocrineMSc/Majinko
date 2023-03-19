using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PeggleWars.Enemies
{
    internal abstract class MeleeEnemy : Enemy
    {
        internal override void Attack()
        {
            base.Attack();
            Player.Instance.TakeDamage(_damage);
        }
    }
}
