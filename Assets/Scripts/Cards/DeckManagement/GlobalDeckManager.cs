using EnumCollection;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    internal class GlobalDeckManager : MonoBehaviour
    {
        #region Fields/Properties

        internal static GlobalDeckManager Instance { get; private set; }

        private int[] _apprenticeDeck = new int[] { (int) CardType.Divination, (int) CardType.Divination, (int) CardType.ManaBlitz,
            (int) CardType.ManaBlitz, (int) CardType.ManaBlitz, (int) CardType.ManaBlitz, (int) CardType.ManaShield,(int) CardType.ManaShield,
            (int) CardType.ManaShield, (int) CardType.ManaShield };

        
        [SerializeField] private List<Card> _globalDeck = new(); //List of all cards in the player deck, will store any modifications (added or removed cards) during a run
        internal List<Card> GlobalDeck
        {
            get { return _globalDeck; }
            set { _globalDeck = value; }
        }

        internal StartDeck StartDeck { get; private set; } // enum for choice of startdecks

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            if(_globalDeck.Count == 0)
            {
                BuildStartDeck(StartDeck.Apprentice);
            }
        }      

        internal void BuildStartDeck(StartDeck startDeck)
        {
            switch(startDeck)
            {
                case StartDeck.Apprentice:
                    for (int i = 0; i < _apprenticeDeck.Length; i++)
                    {
                        _globalDeck.Add(GlobalCardManager.Instance.AllCards[_apprenticeDeck[i]]);
                    }
                    break;
            }
        }

        internal void RemoveCardFromGlobalDeck(Card card)
        {
            _globalDeck.Remove(card);
        }

        #endregion
    }
}
