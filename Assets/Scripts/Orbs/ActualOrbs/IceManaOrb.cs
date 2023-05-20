using Characters.Enemies;
using PeggleWars.ScrollDisplay;
using System.Collections;
using System.Net.NetworkInformation;
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
                enemy.ApplyFreezing(1);
            }
            yield return new WaitForSeconds(0.1f);
            OrbEvents.Instance.OrbEffectEnd?.Invoke();
        }

        protected override void AdditionalEffectsOnCollision()
        {
            OrbActionManager.Instance.AddOrbToActionList(this);
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Upon being hit, this orb spawns Ice Mana and inflicts freezing on all enemies.";
        }
    }
}
