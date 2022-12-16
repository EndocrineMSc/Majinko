using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using PeggleOrbs;

namespace Cards.ScriptableCards
{
    public interface IScriptableOrbChangerCard
    {
        public Orb CardOrb { get; }
        public int AmountOrbs { get; }
    }
}
