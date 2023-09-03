using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Characters
{
    public class AttackClockPolish : MonoBehaviour
    {
        private TextMeshProUGUI _timer;
        private bool _timerIsZero;
        private RectTransform _rectTransform;
        private Image _attackClock;
        private readonly float _pulseTime = 0.3f;

        // Update is called once per frame
        void Update()
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();

            if (_attackClock == null)
                _attackClock = GetComponent<Image>();

            if (_timer == null)
                _timer = GetComponentInChildren<TextMeshProUGUI>();

            if (!_timerIsZero && _timer != null && _timer.text == "0")
            {
                _timerIsZero = true;
                TweenPunch();
            }
        }

        private void TweenPunch()
        {
            if (_timer.text != "0")
                _timerIsZero = false;

            if (_timerIsZero)
            {
                _rectTransform.DOPunchScale(_rectTransform.localScale * 1.05f, _pulseTime, 1, 1).OnComplete(TweenPunch);
                _attackClock.DOColor(Color.red, _pulseTime / 2).OnComplete(RevertColor);
            }
        }

        private void RevertColor()
        {
            _attackClock.DOColor(Color.white, _pulseTime / 2);
        }
    }
}
