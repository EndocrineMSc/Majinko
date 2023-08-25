using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

namespace Characters.Enemies
{
    internal class PlayerHealthUI : MonoBehaviour
    {
        #region Fields and Properties

        private Player _player;
        private Canvas _canvas;
        private TextMeshProUGUI _healthPoints;
        private Image _heart;
        private Image _fastHandsStatus;
        private Image _sicknessStatus;
        private Image _shieldBeetleBuff;
        private List<GameObject> _statusObjects;
        private int _lastUpdateHealth;
        private int _lastUpdateFastHandsStacks;
        private int _lastUpdateSicknessStacks;
        private bool _hasFastHands;
        private bool _hasShieldBeetle;
        private bool _isSick;
        private readonly float _punchScale = 1.1f;
        private readonly float _punchTime = 0.2f;

        [SerializeField] private GameObject _fastHandsPrefab;
        [SerializeField] private GameObject _shieldBeetlePrefab;
        [SerializeField] private GameObject _sicknessPrefab;
        [SerializeField] private GameObject _layoutGroup;


        #endregion

        #region Functions

        private void Awake()
        {
            _player = GetComponentInParent<Player>();
            _healthPoints = GetComponentInChildren<TextMeshProUGUI>();
            _heart = GetComponentInChildren<Image>();
            _canvas = GetComponent<Canvas>();
            _statusObjects ??= new();
            _lastUpdateHealth = _player.Health;
        }

        void Update()
        {
            UpdateHealth();
            UpdateFastHands();
            UpdateSickness();
            UpdateShieldBeetle();
        }

        private void UpdateHealth()
        {
            if (_lastUpdateHealth != _player.Health)
            {
                _heart.rectTransform.DOPunchScale(_heart.rectTransform.localScale * _punchScale, _punchTime, 1, 1);
                _healthPoints.text = _player.Health >= 0 ? _player.Health.ToString() : "0";

                if (_player.Health <= 0)
                {
                    _heart.DOFade(0, 0.5f);
                    _healthPoints.DOFade(0, 0.5f);
                }

                _lastUpdateHealth = _player.Health;
            }
        }

        private void UpdateFastHands()
        {
            if (PlayerConditionTracker.FastHandStacks > 0)
            {
                if (_hasFastHands)
                {
                    if (_lastUpdateFastHandsStacks != PlayerConditionTracker.FastHandStacks)
                    {
                        UpdateFastHandsStacks();
                    }
                }
                else
                {
                    _hasFastHands = true;
                    _fastHandsStatus = Instantiate(_fastHandsPrefab, _layoutGroup.transform).GetComponent<Image>();
                    _fastHandsStatus.rectTransform.SetParent(_layoutGroup.GetComponent<RectTransform>());
                    _statusObjects.Add(_fastHandsStatus.gameObject);
                    UpdateFastHandsStacks();
                }
            }

            if (PlayerConditionTracker.FastHandStacks <= 0 && _hasFastHands)
            {
                _hasFastHands = false;
                _statusObjects.Remove(_fastHandsStatus.gameObject);
                Destroy(_fastHandsStatus.gameObject);
            }
        }

        private void UpdateSickness()
        {
            if (PlayerConditionTracker.SicknessStacks > 0)
            {
                if (_isSick)
                {
                    if (_lastUpdateSicknessStacks != PlayerConditionTracker.SicknessStacks)
                    {
                        UpdateSicknessStacks();
                    }
                }
                else
                {
                    _isSick = true;
                    _sicknessStatus = Instantiate(_sicknessPrefab, _layoutGroup.transform).GetComponent<Image>();
                    _sicknessStatus.rectTransform.SetParent(_layoutGroup.GetComponent<RectTransform>());
                    _statusObjects.Add(_sicknessStatus.gameObject);
                    UpdateSicknessStacks();
                }
            }

            if (PlayerConditionTracker.SicknessStacks <= 0 && _isSick)
            {
                _isSick = false;
                _statusObjects.Remove(_sicknessStatus.gameObject);
                Destroy(_sicknessStatus.gameObject);
            }
        }

        private void UpdateShieldBeetle()
        {
            if (PlayerConditionTracker.HasShieldBeetle && !_hasShieldBeetle)
            {
                _hasShieldBeetle = true;
                _shieldBeetleBuff = Instantiate(_shieldBeetlePrefab, _layoutGroup.transform).GetComponent<Image>();
                _shieldBeetleBuff.rectTransform.SetParent(_layoutGroup.GetComponent<RectTransform>());
                _statusObjects.Add(_shieldBeetleBuff.gameObject);
                _shieldBeetleBuff.rectTransform.DOPunchScale(_shieldBeetleBuff.rectTransform.localScale * _punchScale, _punchTime, 1, 1);
            }
        }

        private void UpdateFastHandsStacks()
        {
            _fastHandsStatus.rectTransform.DOPunchScale(_fastHandsStatus.rectTransform.localScale * _punchScale, _punchTime, 1, 1);
            _fastHandsStatus.GetComponentInChildren<TextMeshProUGUI>().text = PlayerConditionTracker.FastHandStacks.ToString();
            _lastUpdateFastHandsStacks = PlayerConditionTracker.FastHandStacks;
        }

        private void UpdateSicknessStacks()
        {
            _sicknessStatus.rectTransform.DOPunchScale(_sicknessStatus.rectTransform.localScale * _punchScale, _punchTime, 1, 1);
            _sicknessStatus.GetComponentInChildren<TextMeshProUGUI>().text = PlayerConditionTracker.SicknessStacks.ToString();
            _lastUpdateSicknessStacks = PlayerConditionTracker.SicknessStacks;
        }

        #endregion
    }
}
