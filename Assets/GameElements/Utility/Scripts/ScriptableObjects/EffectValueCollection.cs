using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utility
{
    [CreateAssetMenu(menuName = "EffectValueCollection")]
    internal class EffectValueCollection : ScriptableObject
    {
        public int Damage;
        public int ShieldStacks;
        public int BurningStacks;
        public int FreezingStacks;
        public int PercentToFreeze;
    }
}
