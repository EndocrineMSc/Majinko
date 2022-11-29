using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleOrbs
{
    public class ManaBlitzOrb : Orb
    {
        private new void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
            ShootBlitz();
            StartCoroutine(nameof(DestroyThisObject));
        }

        private void ShootBlitz()
        {
            //ToDo: Enemy targeted Mana blitz shot animation
            //ToDo: Deal Damage to Enemy
        }

        private IEnumerator DestroyThisObject()
        {
            yield return new WaitForSeconds(1);
            Destroy(this);
        }
    }
}
