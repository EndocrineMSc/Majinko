using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Orbs;
using EnumCollection;

namespace PeggleWars.Cards
{
    internal class PermanentExhaustOrbChangerCard : OrbChangerCard
    { 
        protected override void CardEffect()
        {
            GlobalOrbManager.Instance.AddGlobalOrb(GlobalOrbManager.Instance.AllOrbsList[(int)_orbType]);
            OrbManager.Instance.SwitchOrbs(_orbType);

            _globalDeckManager.RemoveCardFromGlobalDeck(_globalDeckManager.AllCards[(int)_cardType]);
        }
    }
}