using EnumCollection;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PeggleWars.Cards
{
    internal class GlobalDeckManager : MonoBehaviour
    {
        #region Fields/Properties

        internal static GlobalDeckManager Instance { get; private set; }

        private int[] _apprenticeDeck = new int[] { 0, 1, 2, 3, 3, 4, 4, 5, 6, 5, 6 }; //stores the indeces of the cards in the list _allCards

        [SerializeField] private List<Card> _allCards; //List of all Cards, built from Resources Folder
        internal List<Card> AllCards { get { return _allCards; } }
        
        [SerializeField] private List<Card> _globalDeck = new(); //List of all cards in the player deck, will store any modifications (added or removed cards) during a run
        internal List<Card> GlobalDeck
        {
            get { return _globalDeck; }
            set { _globalDeck = value; }
        }

        internal StartDeck StartDeck { get; private set; } // enum for choice of startdecks

        private string RESOURCE_LOAD_PARAM = "CardPrefabVariants";

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

            _allCards = Resources.LoadAll<Card>(RESOURCE_LOAD_PARAM).ToList();
            
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
                        _globalDeck.Add(_allCards[_apprenticeDeck[i]]);
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
