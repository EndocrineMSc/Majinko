using System.Collections;
using UnityEngine;
using EnumCollection;
using PeggleWars;
using PeggleWars.Audio;

namespace PeggleWars.Orbs.ManaShieldOrb
{
    public class ManaShieldOrb : Orb
    {
        #region Fields and Properties

        private Player _player;

        [SerializeField] private int _shieldValue;

        public int ShieldValue
        {
            get { return _shieldValue; }
            set { _shieldValue = value; }
        }

        #endregion

        #region Public Functions

        public override IEnumerator OrbEffect()
        {
            StartCoroutine(base.OrbEffect());
            _player.Shield += _shieldValue;
            yield return new WaitForSeconds(0.2f);
        }

        #endregion

        #region Protected Functions

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
