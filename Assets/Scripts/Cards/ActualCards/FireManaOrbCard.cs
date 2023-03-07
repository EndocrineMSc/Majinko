using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Orbs;
using EnumCollection;
using PeggleWars.Cards.DeckManagement.Global;

namespace PeggleWars.Cards
{
    public class FireManaOrbCard : Card
    {
        protected override void SetReferencesToLevelComponents()
        {
            base.SetReferencesToLevelComponents();
            _globalDeckManager = GlobalDeckManager.Instance;
        }

        protected override void CardEffect()
        {
            GlobalOrbManager globalOrbManager = GlobalOrbManager.Instance;
            globalOrbManager.AddGlobalOrb(globalOrbManager.AllOrbsList[(int)OrbType.FireManaOrb]);

            OrbManager orbManager = OrbManager.Instance;
            orbManager.SwitchOrbs(OrbType.FireManaOrb);

            _globalDeckManager.RemoveCardFromGlobalDeck(this);
        }
    }
}