using Characters;
using Utility;
using System.Collections;
using UnityEngine;

namespace Orbs
{
    internal class ManaShieldOrb : Orb
    {
        #region Fields and Properties

        private Player _player;

        [SerializeField] private int _shieldValue;

        internal int ShieldValue
        {
            get { return _shieldValue; }
            set { _shieldValue = value; }
        }

        #endregion

        #region Functions

        internal override IEnumerator OrbEffect()
        {
            _player.Shield += _shieldValue;
            yield return new WaitForSeconds(0.2f);
        }

        //will be called OnCollisionEnter2D from parent
        protected override void AdditionalEffectsOnCollision()
        {
            StartCoroutine(OrbEffect());         
        }

        protected override void SetReferences()
        {
            base.SetReferences();
            _player = Player.Instance;
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Mana Shield Orb</b><size=20%>\n\n<size=100%>Hitting this orb will " +
                "cast <b>Mana Shield</b>, granting the player <b>10 shield</b>. \n" +
                "One of the earliest spells taught to wizard apprentices.";
        }

        #endregion
    }
}
