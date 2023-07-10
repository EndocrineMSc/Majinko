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
        private ManaType _orbManaType;

        #endregion

        #region Functions

        private void Start()
        {
            Orb orb = GetComponent<Orb>();
            _orbManaType = orb.SpawnManaType;

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

                switch(_orbManaType)
                {
                    case ManaType.BasicMana:
                        popUp.SetPopUpValue((float)System.Math.Round(OrbManager.Instance.GatheredBasicManaAmountTurn, 1));
                        break;
                    case ManaType.FireMana:
                        popUp.SetPopUpValue((float)System.Math.Round(OrbManager.Instance.GatheredFireManaAmountTurn, 1));
                        break;
                    case ManaType.IceMana:
                        popUp.SetPopUpValue((float)System.Math.Round(OrbManager.Instance.GatheredIceManaAmountTurn, 1));
                        break;
                    case ManaType.RottedMana:
                        popUp.SetPopUpValue((float)System.Math.Round(OrbManager.Instance.GatheredBasicManaAmountTurn, 1));
                        break;
                }
            }
        }

        #endregion




    }
}
