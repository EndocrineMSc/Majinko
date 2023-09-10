using EnumCollection;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Cards
{
    public class GlobalDeckManager : MonoBehaviour, IResetOnQuit
    {
        #region Fields and Properties

        public static GlobalDeckManager Instance { get; private set; }

        private readonly string SAVE_PATH = "GlobalDeck";

        private readonly int[] _apprenticeDeck = new int[] { (int) CardType.Divination, (int) CardType.Divination, (int) CardType.ManaBlitz,
            (int) CardType.ManaBlitz, (int) CardType.ManaBlitz, (int) CardType.ManaBlitz, (int) CardType.ManaShield,(int) CardType.ManaShield,
            (int) CardType.ManaShield, (int) CardType.ManaShield };

        public List<Card> GlobalDeck = new(); //List of all cards in the player deck, will store any modifications (added or removed cards) during a run

        public StartDeck StartDeck { get; private set; } // enum for choice of startdecks

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
            if(ES3.KeyExists(SAVE_PATH))
                GlobalDeck = ES3.Load<List<Card>>(SAVE_PATH);

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

        private void OnApplicationQuit()
        {
            ES3.Save(SAVE_PATH, GlobalDeck);
        }

        public void BuildStartDeck(StartDeck startDeck)
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

        public void RemoveCardFromGlobalDeck(Card card)
        {
            GlobalDeck.Remove(card);
        }

        public void OnGameReset()
        {
            GlobalDeck.Clear();
            ES3.DeleteKey(SAVE_PATH);
            BuildStartDeck(StartDeck.Apprentice);
        }
        #endregion
    }
}
