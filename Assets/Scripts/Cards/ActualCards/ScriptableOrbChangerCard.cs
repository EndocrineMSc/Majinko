using PeggleOrbs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace Cards.ScriptableCards
{
    [CreateAssetMenu]
    public class ScriptableOrbChangerCard : ScriptableCard
    {
        [SerializeField] private OrbType _spawnOrb;
        public OrbType SpawnOrb { get => _spawnOrb; }


        [SerializeField] private int _amountOrbs;
        public int AmountOrbs { get => _amountOrbs; }
    }
}
