using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleOrbs;
using Cards.ScriptableCards;
using EnumCollection;

namespace Cards.Orbchangers
{
    public class OrbChangerCard : Card
    {
        private OrbType _orbType;
        private int _AmountOrbs;

        protected override void Start()
        {
            base.Start();
            ScriptableOrbChangerCard _scriptableOrbChangerCard;
            _scriptableOrbChangerCard = (ScriptableOrbChangerCard)base.ScriptableCard;
            _orbType = _scriptableOrbChangerCard.SpawnOrb;
            _AmountOrbs = _scriptableOrbChangerCard.AmountOrbs;
        }

        protected override void CardEffect()
        {
            base.CardEffect();
            OrbManager.Instance.SwitchOrbs(_orbType, _AmountOrbs);
        }
    }
}
