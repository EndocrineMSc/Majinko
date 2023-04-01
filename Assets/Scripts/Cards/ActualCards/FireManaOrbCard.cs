using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Orbs;
using EnumCollection;

namespace PeggleWars.Cards
{
    internal class FireManaOrbCard : Card, IShiftOrbs
    {
        public int AmountOrbs { get; set; } = 1;

        protected override void CardEffect()
        {
            GlobalOrbManager.Instance.AddGlobalOrb(GlobalOrbManager.Instance.AllOrbsList[(int)OrbType.FireManaOrb]);
            OrbManager.Instance.SwitchOrbs(OrbType.FireManaOrb);

            _globalDeckManager.RemoveCardFromGlobalDeck(GlobalCardManager.Instance.AllCards[(int)_cardType]);
        }
    }
}