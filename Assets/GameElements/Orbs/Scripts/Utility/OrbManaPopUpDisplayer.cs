using UnityEngine;
using EnumCollection;
using Spheres;
using ManaManagement;

namespace Orbs
{
    internal class OrbManaPopUpDisplayer : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private GameObject _orbPopUpPrefab;
        private Color _popUpColor;
        private Orb _orb;

        #endregion

        #region Functions

        private void Start()
        {
            _orb = GetComponent<Orb>();

            switch (_orb.SpawnManaType)
            {
                case ManaType.BasicMana:
                    ColorUtility.TryParseHtmlString("#688AD1", out Color basicColor);
                    _popUpColor = basicColor;
                    break;
                case ManaType.FireMana:
                    ColorUtility.TryParseHtmlString("#DE4C52", out Color fireColor);
                    _popUpColor = fireColor;
                    break;
                case ManaType.IceMana:
                    ColorUtility.TryParseHtmlString("#DE4C52", out Color iceColor);
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
                popUp.SetPopUpValue((float)_orb.ManaAmount / ManaPool.Instance.ManaCostMultiplier);
            }
        }

        #endregion




    }
}
