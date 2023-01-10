using PeggleAttacks.Player;
using System.Collections;
using UnityEngine;
using EnumCollection;
using PeggleOrbs.OrbActions;
using PeggleWars.Audio;

namespace PeggleOrbs.ManaBlitzOrb
{
    public class ManaBlitzOrb : Orb
    {
        #region Fields

        [SerializeField] private PlayerAttack _manaBlitz;

        #endregion

        #region Properties

        #endregion

        #region Public Functions
        public override IEnumerator OrbEffect()
        {
            StartCoroutine(base.OrbEffect());
            _manaBlitz.ShootAttack(_manaBlitz);
            yield return new WaitForSeconds(0.2f);
        }

        #endregion

        #region Protected Functions

        //will be called OnCollisionEnter2D from parent
        protected override void AdditionalEffectsOnCollision()
        {
            OrbActionManager.Instance.AddOrb(this);            
        }

        protected override void Start()
        {
            base.Start();

            if (transform.position.x <= 11)
            {
                AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX.SFX_0008_ManaBlitzSpawn);
            }
        }

        #endregion
    }
}
