using EnumCollection;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PeggleWars.Cards.DeckManagement.Global
{
    /// <summary>
    /// This Class will handle the modifications to a deck during a run, and store the configuration of start decks (of potential different classes later on)
    /// </summary>
    public class GlobalDeckManager : MonoBehaviour
    {
        #region Fields/Properties

        public static GlobalDeckManager Instance { get; private set; }

        private int[] _apprenticeDeck = new int[] { 0, 1, 2, 3, 3, 4, 4, 5, 6, 5, 6 }; //stores the indeces of the cards in the list _allCards

        [SerializeField] private List<Card> _allCards; //List of all Cards, built from Resources Folder
        public List<Card> AllCards { get { return _allCards; } }
        
        private List<Card> _globalDeck = new(); //List of all cards in the player deck, will store any modifications (added or removed cards) during a run
        public List<Card> GlobalDeck
        {
            get { return _globalDeck; }
            set { _globalDeck = value; }
        }

        public StartDeck StartDeck { get; private set; } // enum for choice of startdecks

        private string RESOURCE_LOAD_PARAM = "CardPrefabVariants";

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }

            _allCards = Resources.LoadAll<Card>(RESOURCE_LOAD_PARAM).ToList();
            Instance.BuildStartDeck(StartDeck.Apprentice);
        }

        public void BuildStartDeck(StartDeck startDeck)
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

        public void RemoveCardFromGlobalDeck(Card card)
        {
            _globalDeck.Remove(card);
        }

        #endregion
    }
}
