using Characters.Enemies;
using PeggleWars.ScrollDisplay;
using System.Collections;
using UnityEngine;


namespace Orbs
{
    internal class FireManaOrb : Orb, IAmPersistent
    {
        private EnemyManager _enemyManager;
        private int _amountBurning = 1;

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
                enemy.ApplyBurning(_amountBurning);
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
            displayOnScroll.DisplayDescription = "Upon being hit, this orb spawns Fire Mana and inflicts " + _amountBurning + " burning on the closest enemy.";
        }
    }
}
