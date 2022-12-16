using PeggleOrbs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards.ScriptableCards
{
    [CreateAssetMenu]
    public class ScriptableOrbChangerCard : ScriptableCard
    {
        [SerializeField] private Orb _cardOrb;
        public Orb CardOrb { get => _cardOrb; }


        [SerializeField] private int _amountOrbs;
        public int AmountOrbs { get => _amountOrbs; }
    }
}
