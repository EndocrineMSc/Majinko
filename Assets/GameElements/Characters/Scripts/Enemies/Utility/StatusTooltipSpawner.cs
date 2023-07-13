using Characters.Enemies;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Characters
{
    internal class StatusTooltipSpawner : MonoBehaviour
    {
        #region Fields and Properties

        private Enemy _enemy;
        [SerializeField] private VerticalLayoutGroup _tooltipGroup;
        [SerializeField] private StatusEffectTooltip _tooltip;
        

        private List<StatusEffectTooltip> _activeTooltips;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            _enemy = GetComponent<Enemy>();
            _activeTooltips = new();
        }

        private void OnMouseEnter()
        {
            if (_enemy.BurningStacks > 0)
            {
                StatusEffectTooltip burning = InstantiateTooltip(StatusEffects.Burning);
                _activeTooltips.Add(burning);
            }

            if (_enemy.FreezingStacks > 0)
            {
                StatusEffectTooltip freezing = InstantiateTooltip(StatusEffects.Freezing);
                _activeTooltips.Add(freezing);
            }

            if (_enemy.FrozenForTurns > 0)
            {
                StatusEffectTooltip frozen = InstantiateTooltip(StatusEffects.Frozen);
                _activeTooltips.Add(frozen);
            }

            if (_enemy.TemperatureSicknessStacks > 0)
            {
                StatusEffectTooltip temperatureSick = InstantiateTooltip(StatusEffects.TemperatureSickness);
                _activeTooltips.Add(temperatureSick);
            }

            if (_enemy.TryGetComponent<ICanBeIntangible>(out var intangibleEnemy) && intangibleEnemy.IntangibleStacks > 0) 
            {
                StatusEffectTooltip intangible = InstantiateTooltip(StatusEffects.Intangible);
                _activeTooltips.Add(intangible);
            }
        }

        private void OnMouseExit()
        {
            foreach (var item in _activeTooltips)
            {
                Destroy(item.gameObject);
            }

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
