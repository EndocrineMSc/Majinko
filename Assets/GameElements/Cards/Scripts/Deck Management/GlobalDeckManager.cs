using EnumCollection;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Cards
{
    internal class GlobalDeckManager : MonoBehaviour, IResetOnQuit
    {
        #region Fields and Properties

        internal static GlobalDeckManager Instance { get; private set; }

        private readonly int[] _apprenticeDeck = new int[] { (int) CardType.Divination, (int) CardType.Divination, (int) CardType.ManaBlitz,
            (int) CardType.ManaBlitz, (int) CardType.ManaBlitz, (int) CardType.ManaBlitz, (int) CardType.ManaShield,(int) CardType.ManaShield,
            (int) CardType.ManaShield, (int) CardType.ManaShield };

        internal List<Card> GlobalDeck { get; set; } = new(); //List of all cards in the player deck, will store any modifications (added or removed cards) during a run

        internal StartDeck StartDeck { get; private set; } // enum for choice of startdecks

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            if (GlobalDeck.Count == 0)
                BuildStartDeck(StartDeck.Apprentice);
        }

        private void OnEnable()
        {
            UtilityEvents.OnGameReset += OnGameReset;
        }

        private void OnDisable()
        {
            UtilityEvents.OnGameReset -= OnGameReset;
        }

        internal void BuildStartDeck(StartDeck startDeck)
        {
            switch(startDeck)
            {
                case StartDeck.Apprentice:
                    for (int i = 0; i < _apprenticeDeck.Length; i++)
                    {
                        GlobalDeck.Add(GlobalCardManager.Instance.AllCards[_apprenticeDeck[i]]);
                    }
                    break;
            }
        }

        internal void RemoveCardFromGlobalDeck(Card card)
        {
            GlobalDeck.Remove(card);
        }

        public void OnGameReset()
        {
            GlobalDeck.Clear();
            BuildStartDeck(StartDeck.Apprentice);
        }
        #endregion
    }
}
