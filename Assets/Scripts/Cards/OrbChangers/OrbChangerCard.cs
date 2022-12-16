using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleOrbs;
using Cards.ScriptableCards;
using Unity.VisualScripting;

namespace Cards.Orbchangers
{
    public class OrbChangerCard : Card
    {
        private Orb _orb;
        private int _AmountOrbs;

        protected override void Start()
        {
            base.Start();
            ScriptableOrbChangerCard _scriptableOrbChangerCard;
            _scriptableOrbChangerCard = (ScriptableOrbChangerCard)base.ScriptableCard;
            _orb = _scriptableOrbChangerCard.CardOrb;
            _AmountOrbs = _scriptableOrbChangerCard.AmountOrbs;
        }

        protected override void CardEffect()
        {
            base.CardEffect();
            OrbManager.Instance.SwitchOrbs(_orb, _AmountOrbs);
        }
    }
}
