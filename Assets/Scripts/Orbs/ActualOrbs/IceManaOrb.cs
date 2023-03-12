using PeggleWars.Enemies;
using System.Collections;
using UnityEngine;

namespace PeggleWars.Orbs
{
    internal class IceManaOrb : Orb
    {
        private EnemyManager _enemyManager;

        protected override void SetReferences()
        {
            base.SetReferences();
            _enemyManager = EnemyManager.Instance;
        }

        internal override IEnumerator OrbEffect()
        {
            if (_enemyManager.EnemiesInScene.Count > 0)
            {
                Enemy enemy = _enemyManager.EnemiesInScene[0];
                enemy.Freeze(1);
            }
            yield return new WaitForSeconds(0.1f);
            OrbEvents.Instance.OrbEffectEnd?.Invoke();
        }

        protected override void AdditionalEffectsOnCollision()
        {
            OrbActionManager.Instance.AddOrbToActionList(this);
        }
    }
}
