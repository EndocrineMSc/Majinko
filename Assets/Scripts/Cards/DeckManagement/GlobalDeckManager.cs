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

        private readonly int[] _apprenticeDeck = new int[] { 0, 0, 1, 1, 1, 1, 2, 2, 2, 2 }; //stores the indeces of the cards in the list _allCards

        private List<Card> _allCards; //List of all Cards, built from Resources Folder
        
        private List<Card> _globalDeck = new(); //List of all cards in the player deck, will store any modifications (added or removed cards) during a run
        public List<Card> GlobalDeck
        {
            get { return _globalDeck; }
            set { _globalDeck = value; }
        }

        public StartDeck StartDeck { get; private set; } // enum for choice of startdecks

        #endregion

        #region Private Functions

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

            _allCards = Resources.LoadAll<Card>("CardPrefabVariants").ToList();
            Instance.BuildStartDeck(StartDeck.Apprentice);
        }

        #endregion

        #region Public Functions

        public void BuildStartDeck(StartDeck startDeck)
        {
            switch(startDeck)
            {
                case StartDeck.Apprentice:
                    for (int i = 0; i < 10; i++)
                    {
                        _globalDeck.Add(_allCards[_apprenticeDeck[i]]);
                    }
                    break;
            }
        }

        #endregion
    }
}
