using PeggleWars.Characters;
using PeggleWars.Enemies;
using PeggleWars.ScrollDisplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PeggleWars.Orbs
{
    internal class IntangibleEnemyOrb : Orb
    {
        public override void SetDisplayDescription()
        {
            ScrollDisplayer scrollDisplayer = GetComponent<ScrollDisplayer>();
            scrollDisplayer.DisplayDescription = "Curse Orb. \n Will turn all enemies with the ability to do so intangible upon being hit." +
                "\n \n You feel the remnants of a sould trapped in this orb.";
        }

        protected override void AdditionalEffectsOnCollision()
        {
            StartCoroutine(OrbEffect());
        }

        internal override IEnumerator OrbEffect()
        {
            foreach(Enemy enemy in EnemyManager.Instance.EnemiesInScene)
            {
                if(enemy.TryGetComponent<ICanBeIntangible>(out ICanBeIntangible intangibleEnemy))
                {
                    intangibleEnemy.SetIntangible();
                }
            }
            yield return null;
        }
    }
}