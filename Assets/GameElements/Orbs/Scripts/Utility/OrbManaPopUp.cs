using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace Orbs
{
    internal class OrbManaPopUp : MonoBehaviour
    {
        #region Fields and Properties

        private TextMeshPro _manaAmountText;
        private readonly float _timeOfExistance = 1f;
        private readonly float _moveDistanceFactor = 0.6f;
        private Vector2 _targetPosition;
        private float _destructionTimer;
        private readonly float _jumpPower = 0.25f;

        #endregion

        #region Functions

        private void Awake()
        {
            _manaAmountText = GetComponent<TextMeshPro>();
            _destructionTimer = _timeOfExistance;
            _targetPosition = new(transform.position.x + _moveDistanceFactor, transform.position.y + _moveDistanceFactor);
        }

        private void Start()
        {
            if (_manaAmountText.text == "0")
                _manaAmountText.color = Color.clear;

            transform.DOLocalJump(_targetPosition, _jumpPower, 1, _timeOfExistance);
            _manaAmountText.DOFade(0, _timeOfExistance);
        }

        private void Update()
        {
            _destructionTimer -= Time.deltaTime;

            if (_destructionTimer <= 0 )
                Destroy(gameObject);
        }

        internal void SetPopUpColor(Color color)
        {
            _manaAmountText.color = color;
        }

        internal void SetPopUpValue(float value)
        {
            _manaAmountText.text = value.ToString();
        }

        #endregion
    }
}
