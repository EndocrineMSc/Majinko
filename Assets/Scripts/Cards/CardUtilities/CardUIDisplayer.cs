using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EnumCollection;

namespace PeggleWars.Cards
{
    internal class CardUIDisplayer : MonoBehaviour
    {
        #region Fields

        private Card _card;
        [SerializeField] private TextMeshProUGUI _cardName;
        [SerializeField] private TextMeshProUGUI _cardType;
        [SerializeField] private TextMeshProUGUI _basicCost;
        [SerializeField] private TextMeshProUGUI _fireCost;
        [SerializeField] private TextMeshProUGUI _iceCost;
        [SerializeField] private GameObject _fireBubble;
        [SerializeField] private GameObject _iceBubble;
        [SerializeField] private TextMeshProUGUI _cardText;
        [SerializeField] private GameObject _exhaustBackground;
        [SerializeField] private GameObject _commonBackground;
        [SerializeField] private GameObject _rareBackground;
        [SerializeField] private GameObject _epicBackground;
        [SerializeField] private GameObject _legendaryBackground;

        #endregion

        #region Functions

        private void Start()
        {
            _card = GetComponent<Card>();
            SetCardTexts();
            HideUnusedElements();
            ActivateRarityBackground();
        }

        private void SetCardTexts()
        {
            _cardName.text = _card.CardName;
            _cardType.text = _card.CardType.ToString();
            _basicCost.text = _card.BasicManaCost.ToString();
            _fireCost.text = _card.FireManaCost.ToString();
            _iceCost.text = _card.IceManaCost.ToString();
            _cardText.text = _card.CardDescription;
        }

        private void ActivateRarityBackground()
        {
            switch (_card.Rarity)
            {
                case CardRarity.Basic:
                    break;
                case CardRarity.Common:
                    _commonBackground.SetActive(true);
                    break;
                case CardRarity.Rare:
                    _rareBackground.SetActive(true);
                    break;
                case CardRarity.Epic:
                    _epicBackground.SetActive(true);
                    break;
                case CardRarity.Legendary:
                    _legendaryBackground.SetActive(true);
                    break;
            }
        }

        private void HideUnusedElements()
        {
            if (_card.FireManaCost == 0)
            {
                _fireBubble.SetActive(false);
            }

            if (_card.IceManaCost == 0)
            {
                _iceBubble.SetActive(false);
            }

            if (!_card.ExhaustCard)
            {
                _exhaustBackground.SetActive(false);
            }
        }

        #endregion
    }
}
