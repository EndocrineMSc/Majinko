using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.Enemies;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/FireManaOrbData")]
    public class FireManaOrbData : OrbData
    {
        public override void OrbEffect()
        {
            var enemyManager = EnemyManager.Instance;

            if (enemyManager != null && enemyManager.EnemiesInScene.Count > 0)
                enemyManager.EnemiesInScene[0].ApplyBurning(_burningStacks);
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Fire Mana Orb</b><size=20%>\n\n<size=100%>When hit, this orb spawns " +
                "<b>" + (_amountManaSpawned / 10) + " Fire Mana</b> and applies <b>" + _burningStacks + 
                " Burning</b> to the first enemy.";
        }
    }
}
