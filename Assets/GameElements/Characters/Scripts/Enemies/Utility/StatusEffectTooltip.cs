using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Characters
{
    internal class StatusEffectTooltip : MonoBehaviour
    {
        #region Fields and Properties

        private Image _statusImage;
        private TextMeshProUGUI _statusDescription;

        [SerializeField] private Sprite _burningSprite;
        [SerializeField] private Sprite _freezingSprite;
        [SerializeField] private Sprite _frozenSprite;
        [SerializeField] private Sprite _intangibleSprite;
        [SerializeField] private Sprite _temperatureSicknessSprite;

        private readonly string _burningDescription = "The enemy takes 1 point of damage for each stack at the start of its turn";
        private readonly string _freezingDescription = "If enemy health drops below the amount of stacks, it dies instantly";
        private readonly string _frozenDescription = "While frozen, the enemy cannot act";
        private readonly string _intangibleDescription = "While intangible, the enemy cannot be hit by attacks";
        private readonly string _temperatureSicknessDescription = "For each stack, the enemy will take an additional 5% damage from the next attack";


        #endregion

        private void Awake()
        {
            Transform backgroundContainer = transform.GetChild(0);
            _statusImage = backgroundContainer.transform.GetChild(0).GetComponent<Image>();
            _statusDescription = backgroundContainer.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        internal void SetUp(StatusEffects statusEffect)
        {
            switch (statusEffect)
            {
                case StatusEffects.Burning:
                    _statusImage.sprite = _burningSprite;
                    _statusDescription.text = _burningDescription;
                    break;
                case StatusEffects.Freezing:
                    _statusImage.sprite = _freezingSprite;
                    _statusDescription.text = _freezingDescription;
                    break;
                case StatusEffects.Frozen:
                    _statusImage.sprite = _frozenSprite;
                    _statusDescription.text = _frozenDescription;
                    break;
                case StatusEffects.Intangible:
                    _statusImage.sprite = _intangibleSprite;
                    _statusDescription.text = _intangibleDescription;
                    break;
                case StatusEffects.TemperatureSickness:
                    _statusImage.sprite = _temperatureSicknessSprite;
                    _statusDescription.text = _temperatureSicknessDescription;
                    break;
            }
        }
    }

    internal enum StatusEffects
    {
        Burning,
        Freezing,
        Frozen,
        Intangible,
        TemperatureSickness,
    }
}
