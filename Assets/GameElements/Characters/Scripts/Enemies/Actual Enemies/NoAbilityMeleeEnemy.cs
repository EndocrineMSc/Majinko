using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Enemies
{
    public class NoAbilityMeleeEnemy : Enemy
    {
        protected override void AttackEffect()
        {
            //no attack effect
        }

        protected override void EndTurnEffect()
        {
            //no end turn effect
        }

        protected override void OnDeathEffect()
        {
            //no death effect
        }

        protected override void StartTurnEffect()
        {
            //no start turn effect
        }
    }
}
