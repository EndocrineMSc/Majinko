using Orbs;
using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Enemy/DeathEffects")]
    public class DeathEffectShroombie : EnemyDeathEffect
    {        
        public override void DeathEffect()
        {
            if (_enemy != null && OrbManager.Instance != null)
                OrbManager.Instance.SwitchOrbsWrap(OrbType.RottedOrb, _enemy.transform.position, 2);
        }
    }
}
