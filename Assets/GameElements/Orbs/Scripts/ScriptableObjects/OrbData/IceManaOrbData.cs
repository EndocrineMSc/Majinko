using Characters.Enemies;
using UnityEngine;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/IceManaOrbData")]
    public class IceManaOrbData : OrbData
    {
        public override void OrbEffect()
        {
            var enemyManager = EnemyManager.Instance;

            if (enemyManager != null && enemyManager.EnemiesInScene.Count > 0)
                enemyManager.EnemiesInScene[0].ApplyFreezing(_freezingStacks);
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Ice Mana Orb</b><size=20%>\n\n<size=100%>When hit, " +
                "this orb spawns <b>"+ (_amountManaSpawned / 10) + " Ice Mana</b> and applies <b>" 
                + _freezingStacks + " Freezing</b> to the first enemy.";
        }
    }
}
