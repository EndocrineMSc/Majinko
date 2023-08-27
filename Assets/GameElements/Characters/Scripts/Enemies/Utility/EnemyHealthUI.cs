using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PeggleWars.Characters;
using System.Collections.Generic;
using Unity.VisualScripting;
using ES3Types;

namespace Characters.Enemies
{
    internal class EnemyHealthUI : MonoBehaviour
    {
        #region Fields and Properties

        private Enemy _parentEnemy;
        private Canvas _canvas;
        private ICanBeIntangible _intangibleEnemy;
        private TextMeshProUGUI _healthPoints;
        private Image _heart;
        private Image _burningStatus;
        private Image _freezingStatus;
        private Image _frozenStatus;
        private Image _intangibleStatus;
        private Image _temperatureSicknessStatus;
        private Image _enragedStatus;
        private List<GameObject> _statusObjects;
        private int _lastUpdateHealth;
        private int _lastUpdateFireStacks;
        private int _lastUpdateIceStacks;
        private int _lastUpdateFrozenStacks;
        private int _lastUpdateIntangibleStacks;
        private int _lastUpdateTemperatureSicknessStacks;
        private int _lastUpdateEnragedStacks;
        private bool _isBurning;
        private bool _isFreezing;
        private bool _isFrozen;
        private bool _enemyCanBeIntangible;
        private bool _isIntangible;
        private bool _isTemperatureSick;
        private bool _isEnraged;

        [SerializeField] private GameObject _burningPrefab;
        [SerializeField] private GameObject _freezingPrefab;
        [SerializeField] private GameObject _frozenPrefab;
        [SerializeField] private GameObject _intangiblePrefab;
        [SerializeField] private GameObject _temperatureSicknessPrefab;
        [SerializeField] private GameObject _enragedPrefab;
        [SerializeField] private GameObject _layoutGroup;


        #endregion

        #region Functions

        private void Awake()
        {
            _parentEnemy = GetComponentInParent<Enemy>();
            _healthPoints = GetComponentInChildren<TextMeshProUGUI>();
            _heart = GetComponentInChildren<Image>();
            _canvas = GetComponent<Canvas>();
            _statusObjects ??= new();
        }

        private void OnEnable()
        {
            EnemyEvents.OnEnemyFinishedMoving += UpdateCanvasLayerOrder;
        }

        private void OnDisable()
        {
            EnemyEvents.OnEnemyFinishedMoving -= UpdateCanvasLayerOrder;
            transform.DOKill();
        }

        private void Start()
        {
            _lastUpdateHealth = _parentEnemy.Health;
            _healthPoints.text = _parentEnemy.Health >= 0 ? _parentEnemy.Health.ToString() : "0";
            _lastUpdateFireStacks = _parentEnemy.BurningStacks;
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
            UpdateTemperatureSickness();
            UpdateEnraged();

            if (_enemyCanBeIntangible)
                UpdateIntangible();

            FadeOutStatusObjects(); //only fades if health <= 0
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
            if (_parentEnemy.BurningStacks > 0)
            {
                if (_isBurning)
                {
                    if (_lastUpdateFireStacks != _parentEnemy.BurningStacks)
                    {
                        UpdateFireStacks();
                    }
                }
                else
                {
                    _isBurning = true;
                    _burningStatus = Instantiate(_burningPrefab, _layoutGroup.transform).GetComponent<Image>();
                    _burningStatus.rectTransform.SetParent(_layoutGroup.GetComponent<RectTransform>());
                    _statusObjects.Add(_burningStatus.gameObject);
                    UpdateFireStacks();
                }
            }

            if (_parentEnemy.BurningStacks <= 0 && _isBurning)
            {
                _isBurning = false;
                _statusObjects.Remove(_burningStatus.gameObject);
                Destroy(_burningStatus.gameObject);
            }
        }

        private void UpdateFreezing()
        {
            if (_parentEnemy.FreezingStacks > 0)
            {
                if (_isFreezing)
                {
                    if (_lastUpdateIceStacks != _parentEnemy.FreezingStacks)
                    {
                        UpdateIceStacks();
                    }
                }
                else
                {
                    _isFreezing = true;
                    _freezingStatus = Instantiate(_freezingPrefab, _layoutGroup.transform).GetComponent<Image>();
                    _freezingStatus.rectTransform.SetParent(_layoutGroup.GetComponent<RectTransform>());
                    _statusObjects.Add(_freezingStatus.gameObject);
                    UpdateIceStacks();
                }
            }

            if (_parentEnemy.FreezingStacks <= 0 && _isFreezing)
            {
                _isFreezing = false;
                _statusObjects.Remove(_freezingStatus.gameObject);
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
                    _statusObjects.Add(_frozenStatus.gameObject);
                    UpdateFrozenStacks();
                }
            }

            if (_parentEnemy.FrozenForTurns <= 0 && _isFrozen)
            {
                _isFrozen = false;
                _statusObjects.Remove(_frozenStatus.gameObject);
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
                    _statusObjects.Add(_intangibleStatus.gameObject);
                    UpdateIntangibleStacks();
                }
            }

            if (_intangibleEnemy.IntangibleStacks <= 0 && _isIntangible)
            {
                _isIntangible = false;
                _statusObjects.Remove(_intangibleStatus.gameObject);
                Destroy(_intangibleStatus.gameObject);
            }
        }

        private void UpdateTemperatureSickness()
        {
            if (_parentEnemy.TemperatureSicknessStacks > 0)
            {
                if (_isTemperatureSick)
                {
                    if (_lastUpdateTemperatureSicknessStacks != _parentEnemy.TemperatureSicknessStacks)
                    {
                        UpdateTemperatureSicknessStacks();
                    }
                }
                else
                {
                    _isTemperatureSick = true;
                    _temperatureSicknessStatus = Instantiate(_temperatureSicknessPrefab, _layoutGroup.transform).GetComponent<Image>();
                    _temperatureSicknessStatus.rectTransform.SetParent(_layoutGroup.GetComponent<RectTransform>());
                    _statusObjects.Add(_temperatureSicknessStatus.gameObject);
                    UpdateTemperatureSicknessStacks();
                }
            }

            if (_parentEnemy.TemperatureSicknessStacks <= 0 && _isTemperatureSick)
            {
                _isTemperatureSick = false;
                _statusObjects.Remove(_temperatureSicknessStatus.gameObject);
                Destroy(_temperatureSicknessStatus.gameObject);
            }
        }

        private void UpdateEnraged()
        {
            if (_parentEnemy.EnragedStacks > 0)
            {
                if (_isEnraged)
                {
                    if (_lastUpdateEnragedStacks != _parentEnemy.EnragedStacks)
                    {
                        UpdateEnragedStacks();
                    }
                }
                else
                {
                    _isEnraged = true;
                    _enragedStatus = Instantiate(_enragedPrefab, _layoutGroup.transform).GetComponent<Image>();
                    _enragedStatus.rectTransform.SetParent(_layoutGroup.GetComponent<RectTransform>());
                    _statusObjects.Add(_enragedStatus.gameObject);
                    UpdateEnragedStacks();
                }
            }

            if (_parentEnemy.EnragedStacks <= 0 && _isEnraged)
            {
                _isEnraged = false;
                _statusObjects.Remove(_enragedStatus.gameObject);
                Destroy(_enragedStatus.gameObject);
            }
        }

        private void UpdateFireStacks()
        {
            _burningStatus.rectTransform.DOPunchScale(_burningStatus.rectTransform.localScale * 1.1f, 0.2f, 1, 1);
            _burningStatus.GetComponentInChildren<TextMeshProUGUI>().text = _parentEnemy.BurningStacks.ToString();
            _lastUpdateFireStacks = _parentEnemy.BurningStacks;
        }

        private void UpdateIceStacks()
        {
            _freezingStatus.rectTransform.DOPunchScale(_freezingStatus.rectTransform.localScale * 1.1f, 0.2f, 1, 1);
            _freezingStatus.GetComponentInChildren<TextMeshProUGUI>().text = _parentEnemy.FreezingStacks.ToString();
            _lastUpdateIceStacks = _parentEnemy.FreezingStacks;
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

        private void UpdateTemperatureSicknessStacks()
        {
            _temperatureSicknessStatus.rectTransform.DOPunchScale(_temperatureSicknessStatus.rectTransform.localScale * 1.1f, 0.2f, 1, 1);
            _temperatureSicknessStatus.GetComponentInChildren<TextMeshProUGUI>().text = _parentEnemy.TemperatureSicknessStacks.ToString();
            _lastUpdateTemperatureSicknessStacks = _parentEnemy.TemperatureSicknessStacks;
        }

        private void UpdateEnragedStacks()
        {
            _enragedStatus.rectTransform.DOPunchScale(_enragedStatus.rectTransform.localScale * 1.1f, 0.2f, 1, 1);
            _enragedStatus.GetComponentInChildren<TextMeshProUGUI>().text = _parentEnemy.EnragedStacks.ToString();
            _lastUpdateEnragedStacks = _parentEnemy.EnragedStacks;
        }

        private void UpdateCanvasLayerOrder()
        {
            _canvas.sortingOrder++;
        }

        private void FadeOutStatusObjects()
        {
            if (_parentEnemy.Health <= 0)
            {
                foreach (var item in _statusObjects)
                {
                    item.GetComponent<Image>().DOFade(0, 0.5f);
                    item.GetComponentInChildren<TextMeshProUGUI>().DOFade(0, 0.5f);
                }
            }
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        #endregion
    }
}
