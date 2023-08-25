using Characters.Enemies;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Characters
{
    internal class PlayerStatusTooltipSpawner : MonoBehaviour
    {
        #region Fields and Properties

        private Player _player;
        [SerializeField] private VerticalLayoutGroup _tooltipGroup;
        [SerializeField] private StatusEffectTooltip _tooltip;
        

        private List<StatusEffectTooltip> _activeTooltips;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            _player = GetComponent<Player>();
            _activeTooltips = new();
        }

        private void OnMouseEnter()
        {
            if (PlayerConditionTracker.FastHandStacks > 0)
            {
                StatusEffectTooltip fastHands = InstantiateTooltip(StatusEffects.FastHands);
                _activeTooltips.Add(fastHands);
            }

            if (PlayerConditionTracker.HasShieldBeetle)
            {
                StatusEffectTooltip shieldBeetle = InstantiateTooltip(StatusEffects.ShieldBeetle);
                _activeTooltips.Add(shieldBeetle);
            }

            if (PlayerConditionTracker.SicknessStacks > 0)
            {
                StatusEffectTooltip sickness = InstantiateTooltip(StatusEffects.Sickness);
                _activeTooltips.Add(sickness);
            }
        }

        private void OnMouseExit()
        {
            foreach (var item in _activeTooltips)
                Destroy(item.gameObject);

            _activeTooltips.Clear();
        }

        private StatusEffectTooltip InstantiateTooltip(StatusEffects statusEffect)
        {
            StatusEffectTooltip tooltip = Instantiate(_tooltip, _tooltipGroup.transform.position, Quaternion.identity).GetComponent<StatusEffectTooltip>();
            tooltip.gameObject.transform.SetParent(_tooltipGroup.transform);
            tooltip.SetUp(statusEffect);
            return tooltip;
        }
    }
}
