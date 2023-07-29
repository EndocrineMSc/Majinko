using UnityEngine;
using TMPro;
using EnumCollection;
using UnityEngine.UI;
using DG.Tweening;
using ManaManagement;

namespace Cards
{
    internal class CardUIDisplayer : MonoBehaviour
    {
        #region Fields

        private Card _card;
        [SerializeField] private Image _highlight;
        [SerializeField] private Image _cardImage;
        [SerializeField] private GameObject _basicCardBackground;
        [SerializeField] private GameObject _iceCardBackground;
        [SerializeField] private GameObject _fireCardBackground;
        [SerializeField] private GameObject _lightningCardBackground;
        [SerializeField] private GameObject _commonBackground;
        [SerializeField] private GameObject _rareBackground;
        [SerializeField] private GameObject _epicBackground;
        [SerializeField] private GameObject _legendaryBackground;
        [SerializeField] private TextMeshProUGUI _cardType;
        [SerializeField] private GameObject _basicBubble;
        [SerializeField] private TextMeshProUGUI _basicCost;
        [SerializeField] private GameObject _iceBubbleUpper;
        [SerializeField] private TextMeshProUGUI _iceCostUpper;
        [SerializeField] private GameObject _iceBubbleLower;
        [SerializeField] private TextMeshProUGUI _iceCostLower;
        [SerializeField] private GameObject _fireBubbleUpper;
        [SerializeField] private TextMeshProUGUI _fireCostUpper;
        [SerializeField] private GameObject _fireBubbleLower;
        [SerializeField] private TextMeshProUGUI _fireCostLower;
        [SerializeField] private TextMeshProUGUI _cardText;
        [SerializeField] private GameObject _exhaustBackground;
        [SerializeField] private TextMeshProUGUI _cardName;
        private readonly string INSTANT_EFFECT_TYPE = "Instant";
        private readonly string ORBSHIFTER_EFFECT_TYPE = "Orbshifter";
        private readonly string SPHERESHIFTER_EFFECT_TYPE = "Sphereshifter";

        #endregion

        #region Functions

        private void OnValidate()
        {
            _card = GetComponent<Card>();
            var scriptableCard = _card.ScriptableCard;
            SetCardTexts(scriptableCard);
            ActivateRarityBackground();
            ActivateCardFrameAndCostBubbles();
        }

        private void Start()
        {
            _card = GetComponent<Card>();
            var scriptableCard = _card.ScriptableCard;
            SetCardTexts(scriptableCard);
            ActivateRarityBackground();
            ActivateCardFrameAndCostBubbles();
        }

        private void SetCardTexts(ScriptableCard scriptableCard)
        {
            if (scriptableCard != null)
            {
                _cardName.text = scriptableCard.CardName;
                SetCardEffectTypeText();
                _basicCost.text = scriptableCard.BasicManaCost.ToString();
                _fireCostUpper.text = scriptableCard.FireManaCost.ToString();
                _fireCostLower.text = scriptableCard.FireManaCost.ToString();
                _iceCostUpper.text = scriptableCard.IceManaCost.ToString();
                _iceCostLower.text = scriptableCard.IceManaCost.ToString();
                _cardText.text = scriptableCard.CardDescription;
            }
        }

        private void SetCardEffectTypeText()
        {
            switch (_card.EffectType)
            {
                case CardEffectType.Instant:
                    _cardType.text = (INSTANT_EFFECT_TYPE);
                    break;
                case CardEffectType.Orbshifter:
                    OrbShifterCard card = (OrbShifterCard)_card;
                    _cardType.text = (ORBSHIFTER_EFFECT_TYPE + " - " + card.OrbAmount); 
                    break;
                case CardEffectType.Sphereshifter:
                    _cardType.text = (SPHERESHIFTER_EFFECT_TYPE);
                    break;
            }      
        }

        private void ActivateRarityBackground()
        {
            DeactivateRarityBackgrounds();
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

        private void ActivateCardFrameAndCostBubbles()
        {
            DeactivateCardFrames();
            DeactivateCostBubbles();
            DeactivateHighlight();
            switch (_card.Element)
            {               
                case CardElement.None:
                    _basicCardBackground.SetActive(true);
                    _basicBubble.SetActive(true);
                    break;
                case CardElement.Fire:
                    _fireCardBackground.SetActive(true);
                    _fireBubbleUpper.SetActive(true);
                    break;
                case CardElement.Ice:
                    _iceCardBackground.SetActive(true);
                    _iceBubbleUpper.SetActive(true);
                    break;
                case CardElement.Lightning:
                    _lightningCardBackground.SetActive(true);
                    _iceBubbleUpper.SetActive(true);
                    _fireBubbleLower.SetActive(true);
                    break;
            }
        }

        private void Update()
        {
            if (ManaPool.Instance != null)
            {
                bool enoughBasicMana = _card.BasicManaCost <= ManaPool.Instance.BasicMana.Count / ManaPool.Instance.ManaCostMultiplier;
                bool enoughFireMana = _card.FireManaCost <= ManaPool.Instance.FireMana.Count / ManaPool.Instance.ManaCostMultiplier;
                bool enoughIceMana = _card.IceManaCost <= ManaPool.Instance.IceMana.Count / ManaPool.Instance.ManaCostMultiplier;

                bool highlightActive = enoughBasicMana && enoughFireMana && enoughIceMana;

                if (highlightActive)
                    ActivateHighlight();
                else
                    DeactivateHighlight();
            }
        }

        private void DeactivateCostBubbles()
        {
            _basicBubble.SetActive(false);
            _fireBubbleUpper.SetActive(false);
            _fireBubbleLower.SetActive(false);
            _iceBubbleUpper.SetActive(false);
            _iceBubbleLower.SetActive(false);
        }

        private void DeactivateCardFrames()
        {
            _basicCardBackground.SetActive(false);
            _fireCardBackground.SetActive(false);
            _iceCardBackground.SetActive(false);
            _lightningCardBackground.SetActive(false);
        }

        private void DeactivateRarityBackgrounds()
        {
            _commonBackground.SetActive(false);
            _rareBackground.SetActive(false);
            _epicBackground.SetActive(false);
            _legendaryBackground.SetActive(false);
        }

        private void DeactivateHighlight()
        {
            if (_highlight != null)
                _highlight.DOFade(0, 0.01f);
        }

        private void ActivateHighlight()
        {
            if (_highlight != null)
                _highlight.DOFade(1, 0.01f);
        }

        #endregion
    }
}
