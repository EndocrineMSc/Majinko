using UnityEngine;
using Characters.Enemies;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/FireBombOrbData")]
    public class FireBombOrbData : OrbData
    {
        [SerializeField] private GameObject _arenaFireBomb;

        public override void CollisionEffect()
        {
            base.CollisionEffect();

            if (_arenaFireBomb != null && _parentOrb != null)
                Instantiate(_arenaFireBomb, _parentOrb.transform);
        }

        public override void OrbEffect()
        {
            if (_orbAttackPrefab != null && EnemyManager.Instance != null)
                _orbAttackPrefab.ShootAttack(EnemyManager.Instance.EnemyPositions[0,0]);
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Fire Bomb Orb</b><size=20%>\n\n<size=100%>Will immediatley explode on impact, " +
                "activating other nearby orbs aswell. Will cast <b>Fire Bomb</b>, hitting all enemies " +
                "for <b>" + _damage + "</b> and applying <b>" + _burningStacks + "Burning</b> to each enemy.";
        }
    }
}
