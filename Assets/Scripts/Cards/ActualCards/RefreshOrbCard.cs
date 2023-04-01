using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Orbs;
using EnumCollection;

namespace PeggleWars.Cards
{
    internal class RefreshOrbCard : Card, IShiftOrbs
    {
        public int AmountOrbs { get; set; } = 1;

        protected override void CardEffect()
        {
            GlobalOrbManager.Instance.AddGlobalOrb(GlobalOrbManager.Instance.AllOrbsList[(int)OrbType.RefreshOrb]);
            OrbManager.Instance.SwitchOrbs(OrbType.RefreshOrb);

            _globalDeckManager.RemoveCardFromGlobalDeck(GlobalCardManager.Instance.AllCards[(int)_cardType]);
        }
    }
}