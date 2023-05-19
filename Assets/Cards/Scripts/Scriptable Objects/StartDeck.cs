using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards.StartDecks
{
    [CreateAssetMenu]
    public class StartDeck : ScriptableObject
    {
        [SerializeField] private int[] _startDeck = new int[10];
        public int[] CardIndeces { get => _startDeck; }
    }
}
