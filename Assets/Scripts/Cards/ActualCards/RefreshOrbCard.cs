using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Orbs;
using EnumCollection;

namespace PeggleWars.Cards
{
    internal class RefreshOrbCard : Card
    { 
        protected override void CardEffect()
        {
            GlobalOrbManager.Instance.AddGlobalOrb(GlobalOrbManager.Instance.AllOrbsList[(int)OrbType.RefreshOrb]);
            OrbManager.Instance.SwitchOrbs(OrbType.RefreshOrb);

            _globalDeckManager.RemoveCardFromGlobalDeck(_globalDeckManager.AllCards[(int)_cardType]);
        }
    }
}