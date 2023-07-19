using Utility;
using System.Collections;
using UnityEngine;

namespace Orbs
{
    internal abstract class ForbiddenOrb : Orb, IAmExodia, IAmPersistent
    {
        protected GameObject _forbiddenE;
        protected GameObject _forbiddenX;
        protected GameObject _forbiddenO;
        protected GameObject _forbiddenD;
        protected GameObject _forbiddenI;
        protected GameObject _forbiddenA;

        protected override void SetReferences()
        {
            base.SetReferences();
            GetAllExodiaObjects();
        }

        internal override IEnumerator OrbEffect()
        {
            yield return null;
        }

        protected override void AdditionalEffectsOnCollision()
        {
            StartCoroutine(OrbEffect());
            CheckForFinishedExodia();
        }

        public void GetAllExodiaObjects()
        {
            _forbiddenE = GameObject.FindGameObjectWithTag("ForbiddenE");
            _forbiddenX = GameObject.FindGameObjectWithTag("ForbiddenX");
            _forbiddenO = GameObject.FindGameObjectWithTag("ForbiddenO");
            _forbiddenD = GameObject.FindGameObjectWithTag("ForbiddenD");
            _forbiddenI = GameObject.FindGameObjectWithTag("ForbiddenI");
            _forbiddenA = GameObject.FindGameObjectWithTag("ForbiddenA");
        }

        public void CheckForFinishedExodia()
        {
            if(_forbiddenE.GetComponent<SpriteRenderer>().enabled
                && _forbiddenX.GetComponent<SpriteRenderer>().enabled
                && _forbiddenO.GetComponent<SpriteRenderer>().enabled
                && _forbiddenD.GetComponent<SpriteRenderer>().enabled
                && _forbiddenI.GetComponent<SpriteRenderer>().enabled
                && _forbiddenA.GetComponent<SpriteRenderer>().enabled)
            {
                UtilityEvents.RaiseLevelVictory();
            }
        }
    }
}
