using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Relics
{
    [CreateAssetMenu (menuName= "Relic List by Rarity")]
    internal class RelicRarityList : ScriptableObject
    {
        public List<Relic> RelicList = new();
    }
}
