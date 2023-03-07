using EnumCollection;
using PeggleWars.Cards.DeckManagement;
using PeggleWars.Cards.DeckManagement.Global;
using PeggleWars.Orbs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleWars.Cards
{
    public class IceManaOrbCard : Card
    {
        protected override void CardEffect()
        {
            GlobalOrbManager globalOrbManager = GlobalOrbManager.Instance;
            globalOrbManager.AddGlobalOrb(globalOrbManager.AllOrbsList[(int)OrbType.IceManaOrb]);

            OrbManager orbManager = OrbManager.Instance;
            orbManager.SwitchOrbs(OrbType.IceManaOrb);

            _globalDeckManager.RemoveCardFromGlobalDeck(this);
        }
    }
}
