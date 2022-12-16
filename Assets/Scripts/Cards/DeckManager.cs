using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using TMPro;

namespace Cards.DeckManagement
{
    public class DeckManager : MonoBehaviour
    {
        #region Fields

        public static DeckManager Instance { get; private set; }

        private List<Card> _globalDeck = new();
        private List<Card> _localDeck = new();
        private List<Card> _discardPile = new();
        private List<Card> _abolishedPile = new();

        private Card[] _cardLibrary;

        public StartDeck StartDeck { get; private set; }

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
        }

        private void Start()
        {
            Instance._cardLibrary = GetComponents<Card>();

            Instance.BuildStartDeck(StartDeck.Apprentice); //temporary line until choice screen and logic is implemented
        }

        #endregion

        #region Public Functions

        public void BuildStartDeck(StartDeck startDeck)
        {
            switch (startDeck)
            {
                case StartDeck.Apprentice:
                    break;
            }
        }

        #endregion
    }
}
