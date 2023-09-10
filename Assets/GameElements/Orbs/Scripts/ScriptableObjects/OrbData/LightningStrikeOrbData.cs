using Characters.Enemies;
using UnityEngine;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/LightningStrikeOrbData")]
    public class LightningStrikeOrbData : OrbData
    {
        public override void OrbEffect()
        {
            if (_orbAttackPrefab != null && EnemyManager.Instance != null)
            {
                Transform targetEnemy = EnemyManager.Instance.EnemiesInScene[^1].transform;
                var targetPosition = new Vector3(targetEnemy.position.x, 8.675f, targetEnemy.position.z);
                _orbAttackPrefab.ShootAttack(targetPosition);
            }
            else
                Debug.Log("OrbData: Lightning Strike couldn't be shot, either attack prefab or EnemyManager is null");
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Lightning Strike Orb</b><size=20%>\n\n<size=100%>When hit will cast " +
                "a <b>Lightning Strike</b> on the last enemy, dealing <b>" + _damage + "Damage</b>.";
        }   
    }
}
