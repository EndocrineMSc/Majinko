using UnityEngine;
using EnumCollection;
using PeggleWars.Orbs;

namespace Cards
{
    internal class OrbShifterCard : Card
    {
        //will be set in the inspector of the respective Card Prefab
        [SerializeField] protected OrbType _orbType;
        [SerializeField] protected int _amountOrbs;
        public int AmountOrbs { get => _amountOrbs; set { _amountOrbs = value; } }

        protected override void CardEffect()
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(GetComponent<RectTransform>().transform.position);            
            OrbManager.Instance.SwitchOrbs(_orbType, worldPosition, _amountOrbs);
        }
    }
}
