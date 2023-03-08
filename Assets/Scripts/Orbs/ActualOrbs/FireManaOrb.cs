using PeggleWars.Enemies;
using PeggleWars.Orbs.OrbActions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PeggleWars.Orbs
{
    public class FireManaOrb : Orb
    {
        private EnemyManager _enemyManager;

        protected override void SetReferences()
        {
            base.SetReferences();
            _enemyManager = EnemyManager.Instance;
        }

        public override IEnumerator OrbEffect()
        {
            if (_enemyManager.EnemiesInScene.Count > 0)
            {
                Enemy enemy = _enemyManager.EnemiesInScene[0];
                enemy.SetOnFire(1);
            }
            yield return null;
        }

        protected override void AdditionalEffectsOnCollision()
        {
            base.AdditionalEffectsOnCollision();
            OrbActionManager.Instance.AddOrbToActionList(this);
        }
    }
}
