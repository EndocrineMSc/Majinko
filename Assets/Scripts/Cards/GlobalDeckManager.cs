using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Cards.DeckManagement.Global
{
    public class GlobalDeckManager : MonoBehaviour
    {
        #region Fields/Properties

        public static GlobalDeckManager Instance { get; private set; }

        private readonly int[] _apprenticeDeck = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private List<Card> _allCards;
        [SerializeField] private List<Card> _globalDeck = new();

        public List<Card> GlobalDeck
        {
            get { return _globalDeck; }
            set { _globalDeck = value; }
        }

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

            _allCards = Resources.LoadAll<Card>("CardPrefabVariants").ToList();
        }

        private void Start()
        {
            Instance.BuildStartDeck(StartDeck.Apprentice);
        }

        #endregion

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
    }
}
