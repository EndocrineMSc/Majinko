using System.Collections;
using UnityEngine;
using Attacks;
using Utility;
using Characters;

namespace Orbs
{
    internal class ManaBlitzOrb : Orb, IHaveBark
    {
        #region Fields and Properties

        [SerializeField] private Attack _manaBlitz;
        public string Bark { get; } = "Mana Blitz!";

        #endregion

        #region Functions

        internal override IEnumerator OrbEffect()
        {
            _manaBlitz.ShootAttack(Player.Instance.transform.position);
            yield return new WaitForSeconds(0.2f);
        }

        //will be called OnCollisionEnter2D from parent
        protected override void AdditionalEffectsOnCollision()
        {
            OrbActionManager.Instance.AddOrbToActionList(this);            
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Mana Blitz Sphere</b><size=20%>\n\n<size=100%>Hitting this orb will enable " +
                "the player to cast a standard <b>Mana Blitz</b> on the closest enemy, dealing <b>10 damage</b>.\n" +
                "One of the earliest spells taught to young witches.";
        }

        #endregion
    }
}
