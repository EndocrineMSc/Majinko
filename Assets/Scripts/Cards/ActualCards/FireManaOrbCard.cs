using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Orbs;
using EnumCollection;

namespace PeggleWars.Cards
{
    internal class FireManaOrbCard : Card
    { 
        protected override void CardEffect()
        {
            GlobalOrbManager.Instance.AddGlobalOrb(GlobalOrbManager.Instance.AllOrbsList[(int)OrbType.FireManaOrb]);
            OrbManager.Instance.SwitchOrbs(OrbType.FireManaOrb);

            _globalDeckManager.RemoveCardFromGlobalDeck(_globalDeckManager.AllCards[(int)_cardType]);
        }
    }
}