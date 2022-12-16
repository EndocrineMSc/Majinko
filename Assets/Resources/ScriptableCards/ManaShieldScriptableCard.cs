using Cards.ScriptableCards;
using EnumCollection;
using PeggleOrbs;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Cards.ScriptableCards
{
    public class ManaShieldScriptableCard : ScriptableOrbChangerCard
    {
        public new string CardName { get; } = "Mana Blitz";

        public new string CardDescription { get; } = "Spawns two Mana Blitz Orbs. When hit, the apprentice Shoots a Mana Blitz at the enemy, dealing 10 damage.";

        public new int ManaCost { get; } = 10;

        public new ManaType ManaType { get; } = ManaType.BaseMana;

        public new CardType CardType { get; } = CardType.Attack;

        public new Orb CardOrb { get; } = Resources.Load("ManaShieldOrb").GetComponent<Orb>();

        public new int AmountOrbs { get; } = 2;
    }
}
