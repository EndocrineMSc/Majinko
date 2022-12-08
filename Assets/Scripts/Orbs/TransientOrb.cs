using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleOrbs.PersistentOrbs;
using EnumCollection;
using PeggleWars.Audio;

namespace PeggleOrbs.TransientOrbs
{
    public class TransientOrb : Orb
    {


        #region Properties

        protected Orb _anchorOrb;

        public Orb AnchorOrb
        { 
            get { return _anchorOrb; }
            private set { _anchorOrb = value; }
        }

        #endregion

        #region Protected Functions

        protected override void OnCollisionEnter2D(Collision2D collision2D)
        {
            AudioManager.Instance.PlaySoundEffectNoLimit(SFX.BasicPeggleHit);
            gameObject.GetComponent<SpriteRenderer>().size += new Vector2(0.03f, 0.03f);
            OrbEffect();
            UnoccupyAnchorOrb();
            StartCoroutine(nameof(DestroyThisObject));
        }

        protected void UnoccupyAnchorOrb()
        {
            _anchorOrb.IsOccupied = false;
        }

        #endregion

        #region Public Functions

        public void SetAnchorOrb(Orb anchorOrb)
        {
            _anchorOrb = anchorOrb;
            anchorOrb.IsOccupied = true;
        }

        #endregion

        #region IEnumerators
        private IEnumerator DestroyThisObject()
        {
            yield return new WaitForSeconds(1f);
            Destroy(this);
        }

        #endregion
    }
}
