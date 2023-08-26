using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Orbs;
using DG.Tweening;

namespace Characters
{
    internal class BubbleWandTextCounter : MonoBehaviour
    {
        private TextMeshProUGUI _remainingHitsText;
        private int _remainingHits = 20;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _remainingHitsText = GetComponent<TextMeshProUGUI>();
            _remainingHitsText.text = _remainingHits.ToString();
            _rectTransform = GetComponentInParent<RectTransform>();
        }

        private void OnEnable()
        {
            OrbEvents.OnOrbHit += OrbWasHit;
        }

        private void OnDisable()
        {
             OrbEvents.OnOrbHit -= OrbWasHit;
        }

        private void OrbWasHit()
        {
            _remainingHits--;

            if (_remainingHits == 0)
            {
                _remainingHits = 20;
                _rectTransform.DOPunchScale(new(1.1f, 1.1f), 0.2f, 1, 1);
            }

            _remainingHitsText.text = _remainingHits.ToString();
        }
    }
}
