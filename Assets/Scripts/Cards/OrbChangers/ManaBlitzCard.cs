using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleOrbs;

namespace Cards.Orbchangers
{
    public class ManaBlitzCard : Card
    {
        [SerializeField] private Orb _manaBlitzOrb;

        protected override void CardEffect()
        {
            base.CardEffect();
            Debug.Log("Mana Blitz Card Effect.");
            OrbManager.Instance.SwitchOrbs(_manaBlitzOrb, 2);
        }
    }
}
