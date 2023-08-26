using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

namespace Characters.Enemies
{
    internal class PlayerHealthUI : MonoBehaviour
    {
        #region Fields and Properties

        private Player _player;
        private Canvas _canvas;
        private Image _heart;
        private Image _fastHandsStatus;
        private Image _sicknessStatus;
        private List<GameObject> _statusObjects;
        private int _lastUpdateHealth;
        private int _lastUpdateShield;
        private int _lastUpdateFastHandsStacks;
        private int _lastUpdateSicknessStacks;
        private readonly float _punchScale = 1.1f;
        private readonly float _punchTime = 0.2f;

        //Shield and Health
        [SerializeField] private TextMeshProUGUI _healthPoints;
        [SerializeField] private TextMeshProUGUI _shieldPoints;
        [SerializeField] private Animator _shieldAnimator;
        [SerializeField] private Sprite _playerHeartSprite;
        private bool _heartIsTweening;

        //BuffDebuff bools
        private bool _hasBubbleWand;
        private bool _hasFastHands;
        private bool _hasOrbInlayedGauntlet;
        private bool _hasShieldBeetle;
        private bool _hasWardingRune;
        private bool _isSick;

        //BuffDebuff Prefabs
        [SerializeField] private GameObject _bubbleWandPrefab;
        [SerializeField] private GameObject _fastHandsPrefab;
        [SerializeField] private GameObject _orbInlayedGauntletPrefab;
        [SerializeField] private GameObject _shieldBeetlePrefab;
        [SerializeField] private GameObject _sicknessPrefab;
        [SerializeField] private GameObject _wardingRunePrefab;
        [SerializeField] private GameObject _layoutGroup;


        #endregion

        #region Functions

        private void Awake()
        {
            _player = GetComponentInParent<Player>();
            _heart = GetComponentInChildren<Image>();
            _canvas = GetComponent<Canvas>();
            _statusObjects ??= new();
            _lastUpdateHealth = _player.Health;
        }

        void Update()
        {
            //Health and Shield
            UpdateHealth();
            UpdateShield();

            //Buffs
            UpdateBubbleWand();
            UpdateFastHands();
            UpdateOrbInlayedGauntlets();
            UpdateShieldBeetle();
            UpdateWardingRune();

            //Debuffs
            UpdateSickness();
        }

        private void UpdateHealth()
        {
            if (_lastUpdateHealth != _player.Health)
            {
                if (!_heartIsTweening)
                {
                    StartCoroutine(HeartTweenBlock());
                    _heart.rectTransform.DOPunchScale(_heart.rectTransform.localScale * _punchScale, _punchTime, 1, 1);
                }

                _healthPoints.text = _player.Health >= 0 ? _player.Health.ToString() : "0";

                if (_player.Health <= 0)
                {
                    _heart.DOFade(0, 0.5f);
                    _healthPoints.DOFade(0, 0.5f);
                }

                _lastUpdateHealth = _player.Health;
            }
        }

        private void UpdateShield()
        {
            if (_player.Shield > 0 && !_shieldPoints.enabled)
            {
                _shieldPoints.enabled = true;
                _shieldAnimator.enabled = true;
            }

            if (_lastUpdateShield != _player.Shield && _shieldPoints.enabled)
            {
                if(!_heartIsTweening)
                {
                    StartCoroutine(HeartTweenBlock());
                    _heart.rectTransform.DOPunchScale(_heart.rectTransform.localScale * _punchScale, _punchTime, 1, 1);
                }

                _shieldPoints.text = _player.Shield >= 0 ? _player.Shield.ToString() : "0";

                if (_player.Shield <= 0)
                {
                    _shieldAnimator.enabled = false;
                    _shieldPoints.enabled = false;
                    _heart.sprite = _playerHeartSprite;
                }
                
                _lastUpdateShield = _player.Shield;
            }
        }

        private void UpdateSickness()
        {
            if (PlayerConditionTracker.SicknessStacks > 0)
            {
                if (_isSick)
                {
                    if (_lastUpdateSicknessStacks != PlayerConditionTracker.SicknessStacks)
                        UpdateSicknessStacks();
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

        #region BuffUpdates

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

        private void UpdateBubbleWand()
        {
            if (PlayerConditionTracker.HasBubbleWand && !_hasBubbleWand)
            {
                _hasBubbleWand = true;
                Image bubbleWandBuff = Instantiate(_bubbleWandPrefab, _layoutGroup.transform).GetComponent<Image>();
                HandleBuffDebuffImage(bubbleWandBuff);
            }
        }

        private void UpdateOrbInlayedGauntlets()
        {
            if (PlayerConditionTracker.HasOrbInlayedGauntlets && !_hasOrbInlayedGauntlet)
            {
                _hasOrbInlayedGauntlet = true;
                Image orbInlayedGauntletBuff = Instantiate(_orbInlayedGauntletPrefab, _layoutGroup.transform).GetComponent<Image>();
                HandleBuffDebuffImage(orbInlayedGauntletBuff);
            }
        }

        private void UpdateShieldBeetle()
        {
            if (PlayerConditionTracker.HasShieldBeetle && !_hasShieldBeetle)
            {
                _hasShieldBeetle = true;
                Image shieldBeetleBuff = Instantiate(_shieldBeetlePrefab, _layoutGroup.transform).GetComponent<Image>();
                HandleBuffDebuffImage(shieldBeetleBuff);
            }
        }

        private void UpdateWardingRune()
        {
            if (PlayerConditionTracker.HasWardingRune && !_hasWardingRune)
            {
                _hasWardingRune = true;
                Image wardingRuneBuff = Instantiate(_wardingRunePrefab, _layoutGroup.transform).GetComponent<Image>();
                HandleBuffDebuffImage(wardingRuneBuff);
            }
        }

        #endregion

        #region Utility

        private void HandleBuffDebuffImage(Image image)
        {
            image.rectTransform.SetParent(_layoutGroup.GetComponent<RectTransform>());
            _statusObjects.Add(image.gameObject);
            image.rectTransform.DOPunchScale(image.rectTransform.localScale * _punchScale, _punchTime, 1, 1);
        }

        private IEnumerator HeartTweenBlock()
        {
            _heartIsTweening = true;
            yield return new WaitForSeconds(_punchTime);
            _heartIsTweening = false;
        }

        #endregion

        #endregion
    }
}
