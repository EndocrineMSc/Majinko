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
        [SerializeField] private OrbType _orbType;
        [SerializeField] private int _amountOrbs;

        protected override void CardEffect()
        {
            base.CardEffect();
            OrbManager.Instance.SwitchOrbs(_orbType, _amountOrbs);
        }
    }
}
