using System.Collections;
using UnityEngine;

namespace PeggleWars.Orbs
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
            StartCoroutine(base.OrbEffect());
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

        #endregion
    }
}
