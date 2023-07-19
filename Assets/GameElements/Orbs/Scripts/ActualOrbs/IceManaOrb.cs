using Characters.Enemies;
using Utility;
using System.Collections;
using UnityEngine;

namespace Orbs
{
    internal class IceManaOrb : Orb, IAmPersistent
    {
        private EnemyManager _enemyManager;
        private int _amountFreezing = 1;

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
                enemy.ApplyFreezing(_amountFreezing);
            }
            yield return new WaitForSeconds(0.1f);
            OrbEvents.RaiseEffectEnd();
        }

        protected override void AdditionalEffectsOnCollision()
        {
            OrbActionManager.Instance.AddOrbToActionList(this);
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Upon being hit, this orb spawns Ice Mana and inflicts " + _amountFreezing + " freezing on the closest enemy.";
        }
    }
}
