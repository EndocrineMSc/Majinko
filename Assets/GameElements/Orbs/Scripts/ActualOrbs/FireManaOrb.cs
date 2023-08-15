using Characters.Enemies;
using Utility;
using System.Collections;
using UnityEngine;
using Attacks;

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
            AttackEvents.RaiseAttackFinished();
            Destroy(gameObject);
        }

        protected override void AdditionalEffectsOnCollision()
        {
            OrbActionManager.Instance.AddOrbToActionList(this);
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Fire Mana Orb</b><size=20%>\n\n<size=100%>Upon being hit, this orb spawns <b>1 Fire Mana</b> and inflicts " +
                "<b>" + _amountBurning + " Burning</b> on the closest enemy.";
        }
    }
}
