using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Orbs;
using DG.Tweening;
using Utility;

namespace Characters
{
    internal class BubbleWandTextCounter : MonoBehaviour
    {
        private TextMeshProUGUI _remainingHitsText;
        private int _remainingHits = 20;
        [SerializeField] private RectTransform _rectTransform;

        private void Awake()
        {
            _remainingHitsText = GetComponent<TextMeshProUGUI>();
            _remainingHitsText.text = _remainingHits.ToString();
        }

        private void OnEnable()
        {
            OrbEvents.OnOrbHit += OrbWasHit;
        }

        private void OnDisable()
        {
             OrbEvents.OnOrbHit -= OrbWasHit;
        }

        private void OrbWasHit(GameObject orb)
        {
            _remainingHits--;

            if (_remainingHits == 0)
            {
                _remainingHits = 20;
                _rectTransform.DOPunchScale(new(_rectTransform.localScale.x * 1.1f, _rectTransform.localScale.y * 1.1f), 0.2f, 1, 1);

                if (ScreenShaker.Instance != null)
                    ScreenShaker.Instance.ShakeCamera(2, 0.1f);
            }

            _remainingHitsText.text = _remainingHits.ToString();
        }
    }
}
