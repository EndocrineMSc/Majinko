using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards.StartDecks
{
    [CreateAssetMenu(menuName = "StartDecks")]
    public class ApprenticeDeck : ScriptableObject
    {
        private readonly int[] _startDeck = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] CardIndeces { get => _startDeck; }
    }
}
