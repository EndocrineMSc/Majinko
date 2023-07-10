using UnityEngine;
using EnumCollection;
using Spheres;
using ManaManagement;
using Utility.TurnManagement;

namespace Orbs
{
    internal class OrbManaPopUpDisplayer : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private GameObject _orbPopUpPrefab;
        private Color _popUpColor;
        private Orb _orb;
        private float _currentManaPopUpAmount = 0;
        private ManaType _orbManaType;

        #endregion

        #region Functions

        private void OnEnable()
        {
            OrbEvents.SpawnMana += OnManaSpawn;
            LevelPhaseEvents.OnStartEnemyPhase += OnStartEnemyPhase;
        }

        private void OnDisable()
        {
            OrbEvents.SpawnMana -= OnManaSpawn;
            LevelPhaseEvents.OnStartEnemyPhase -= OnStartEnemyPhase;
        }

        private void Start()
        {
            _orb = GetComponent<Orb>();
            _orbManaType = _orb.SpawnManaType;

            switch (_orbManaType)
            {
                case ManaType.BasicMana:
                    ColorUtility.TryParseHtmlString("#56689F", out Color basicColor);
                    _popUpColor = basicColor;
                    break;
                case ManaType.FireMana:
                    ColorUtility.TryParseHtmlString("#BE1C23", out Color fireColor);
                    _popUpColor = fireColor;
                    break;
                case ManaType.IceMana:
                    ColorUtility.TryParseHtmlString("#258EB6", out Color iceColor);
                    _popUpColor = iceColor; 
                    break;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Sphere>(out _))
            {
                OrbManaPopUp popUp = Instantiate(_orbPopUpPrefab, transform.position, Quaternion.identity).GetComponent<OrbManaPopUp>();
                popUp.SetPopUpColor(_popUpColor);
                popUp.SetPopUpValue(_currentManaPopUpAmount);
            }
        }

        private void OnManaSpawn(ManaType manaType, int amount)
        {
            if (amount != 0 && _orbManaType == manaType)
                _currentManaPopUpAmount += (float)amount / ManaPool.Instance.ManaCostMultiplier;
        }

        private void OnStartEnemyPhase()
        {
            _currentManaPopUpAmount = 0;
        }

        #endregion




    }
}
