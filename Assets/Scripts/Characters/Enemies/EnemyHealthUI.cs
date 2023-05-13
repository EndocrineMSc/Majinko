using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PeggleWars.Characters;

namespace PeggleWars.Enemies
{
    internal class EnemyHealthUI : MonoBehaviour
    {
        #region Fields and Properties

        private Enemy _parentEnemy;
        private ICanBeIntangible _intangibleEnemy;
        private TextMeshProUGUI _healthPoints;
        private Image _heart;
        private Image _burningStatus;
        private Image _freezingStatus;
        private Image _frozenStatus;
        private Image _intangibleStatus;
        private int _lastUpdateHealth;
        private int _lastUpdateFireStacks;
        private int _lastUpdateIceStacks;
        private int _lastUpdateFrozenStacks;
        private int _lastUpdateIntangibleStacks;
        private bool _isBurning;
        private bool _isFreezing;
        private bool _isFrozen;
        private bool _enemyCanBeIntangible;
        private bool _isIntangible;

        [SerializeField] private GameObject _burningPrefab;
        [SerializeField] private GameObject _freezingPrefab;
        [SerializeField] private GameObject _frozenPrefab;
        [SerializeField] private GameObject _intangiblePrefab;
        [SerializeField] private GameObject _layoutGroup;

        #endregion

        #region Functions

        private void Awake()
        {
            _parentEnemy = GetComponentInParent<Enemy>();
            _healthPoints = GetComponentInChildren<TextMeshProUGUI>();
            _heart = GetComponentInChildren<Image>();
        }

        private void Start()
        {
            _lastUpdateHealth = _parentEnemy.Health;
            _lastUpdateFireStacks = _parentEnemy.FireStacks;
            GetComponent<Canvas>().worldCamera = Camera.main;
            _enemyCanBeIntangible = _parentEnemy.TryGetComponent<ICanBeIntangible>(out _);
           
            if (_enemyCanBeIntangible)
                _intangibleEnemy = _parentEnemy.GetComponent<ICanBeIntangible>();
        }

        void Update()
        {
            UpdateHealth();
            UpdateBurning();
            UpdateFreezing();
            UpdateFrozen();

            if (_enemyCanBeIntangible)
                UpdateIntangible();
        }

        private void UpdateHealth()
        {
            if (_lastUpdateHealth != _parentEnemy.Health)
            {
                _heart.rectTransform.DOPunchScale(_heart.rectTransform.localScale * 1.1f, 0.2f, 1, 1);
                _healthPoints.text = _parentEnemy.Health >= 0 ? _parentEnemy.Health.ToString() : "0";

                if (_parentEnemy.Health <= 0)
                {
                    _heart.DOFade(0, 0.5f);
                    _healthPoints.DOFade(0, 0.5f);
                }

                _lastUpdateHealth = _parentEnemy.Health;
            }
        }

        private void UpdateBurning()
        {
            if (_parentEnemy.FireStacks > 0)
            {
                if (_isBurning)
                {
                    if (_lastUpdateFireStacks != _parentEnemy.FireStacks)
                    {
                        UpdateFireStacks();
                    }
                }
                else
                {
                    _isBurning = true;
                    _burningStatus = Instantiate(_burningPrefab, _layoutGroup.transform).GetComponent<Image>();
                    _burningStatus.rectTransform.SetParent(_layoutGroup.GetComponent<RectTransform>());
                    UpdateFireStacks();
                }
            }

            if (_parentEnemy.FireStacks <= 0 && _isBurning)
            {
                _isBurning = false;
                Destroy(_burningStatus.gameObject);
            }
        }

        private void UpdateFreezing()
        {
            if (_parentEnemy.IceStacks > 0)
            {
                if (_isFreezing)
                {
                    if (_lastUpdateIceStacks != _parentEnemy.IceStacks)
                    {
                        UpdateIceStacks();
                    }
                }
                else
                {
                    _isFreezing = true;
                    _freezingStatus = Instantiate(_freezingPrefab, _layoutGroup.transform).GetComponent<Image>();
                    _freezingStatus.rectTransform.SetParent(_layoutGroup.GetComponent<RectTransform>());
                    UpdateIceStacks();
                }
            }

            if (_parentEnemy.IceStacks <= 0 && _isFreezing)
            {
                _isFreezing = false;
                Destroy(_freezingStatus.gameObject);
            }
        }

        private void UpdateFrozen()
        {
            if (_parentEnemy.FrozenForTurns > 0)
            {
                if (_isFrozen)
                {
                    if (_lastUpdateFrozenStacks != _parentEnemy.FrozenForTurns)
                    {
                        UpdateFrozenStacks();
                    }
                }
                else
                {
                    _isFrozen = true;
                    _frozenStatus = Instantiate(_frozenPrefab, _layoutGroup.transform).GetComponent<Image>();
                    _frozenStatus.rectTransform.SetParent(_layoutGroup.GetComponent<RectTransform>());
                    UpdateFrozenStacks();
                }
            }

            if (_parentEnemy.FrozenForTurns <= 0 && _isFrozen)
            {
                _isFrozen = false;
                Destroy(_frozenStatus.gameObject);
            }
        }

        private void UpdateIntangible()
        {
            if (_intangibleEnemy.IntangibleStacks > 0)
            {
                if (_isIntangible)
                {
                    if (_lastUpdateIntangibleStacks != _intangibleEnemy.IntangibleStacks)
                    {
                        UpdateIntangibleStacks();
                    }
                }
                else
                {
                    _isIntangible = true;
                    _intangibleStatus = Instantiate(_intangiblePrefab, _layoutGroup.transform).GetComponent<Image>();
                    _intangibleStatus.rectTransform.SetParent(_layoutGroup.GetComponent<RectTransform>());
                    UpdateIntangibleStacks();
                }
            }

            if (_intangibleEnemy.IntangibleStacks <= 0 && _isIntangible)
            {
                _isIntangible = false;
                Destroy(_intangibleStatus.gameObject);
            }
        }
        
        private void UpdateFireStacks()
        {
            _burningStatus.rectTransform.DOPunchScale(_burningStatus.rectTransform.localScale * 1.1f, 0.2f, 1, 1);
            _burningStatus.GetComponentInChildren<TextMeshProUGUI>().text = _parentEnemy.FireStacks.ToString();
            _lastUpdateFireStacks = _parentEnemy.FireStacks;
        }

        private void UpdateIceStacks()
        {
            _freezingStatus.rectTransform.DOPunchScale(_freezingStatus.rectTransform.localScale * 1.1f, 0.2f, 1, 1);
            _freezingStatus.GetComponentInChildren<TextMeshProUGUI>().text = _parentEnemy.IceStacks.ToString();
            _lastUpdateIceStacks = _parentEnemy.IceStacks;
        }

        private void UpdateFrozenStacks()
        {
            _frozenStatus.rectTransform.DOPunchScale(_frozenStatus.rectTransform.localScale * 1.1f, 0.2f, 1, 1);
            _frozenStatus.GetComponentInChildren<TextMeshProUGUI>().text = _parentEnemy.FrozenForTurns.ToString();
            _lastUpdateFrozenStacks = _parentEnemy.FrozenForTurns;
        }

        private void UpdateIntangibleStacks()
        {
            _intangibleStatus.rectTransform.DOPunchScale(_intangibleStatus.rectTransform.localScale * 1.1f, 0.2f, 1, 1);
            _intangibleStatus.GetComponentInChildren<TextMeshProUGUI>().text = _intangibleEnemy.IntangibleStacks.ToString();
            _lastUpdateIntangibleStacks = _intangibleEnemy.IntangibleStacks;
        }

        #endregion

    }
}
