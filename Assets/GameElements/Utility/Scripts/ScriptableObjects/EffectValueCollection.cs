using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utility
{
    [CreateAssetMenu(menuName = "EffectValueCollection")]
    public class EffectValueCollection : ScriptableObject
    {
        [SerializeField] private int _damage;
        [SerializeField] private int _shieldStacks;
        [SerializeField] private int _burningStacks;
        [SerializeField] private int _freezingStacks;
        [SerializeField] private int _percentToFreeze;

        public int Damage
        {
            get { return _damage; }
            private set { _damage = value; }
        }

        public int ShieldStacks
        {
            get { return _shieldStacks; }
            private set { _shieldStacks = value; }
        }

        public int BurningStacks
        {
            get { return _burningStacks; }
            private set { _burningStacks = value; }
        }

        public int FreezingStacks
        {
            get { return _freezingStacks; }
            private set { _freezingStacks = value; }
        }
        
        public int PercentToFreeze
        {
            get { return _percentToFreeze; }
            private set { _percentToFreeze = value;}
        }
    }
}
