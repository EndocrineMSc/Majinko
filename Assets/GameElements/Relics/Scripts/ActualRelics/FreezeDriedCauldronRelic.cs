using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;
using UnityEngine.UI;
using Characters.Enemies;
using DG.Tweening;

namespace Relics
{
    internal class FreezeDriedCauldronRelic : MonoBehaviour, IRelic
    {

        internal static FreezeDriedCauldronRelic Instance { get; private set; }

        public Relic RelicEnum { get; private set; } = Relic.FreezeDriedCauldronRelic;

        public string Description { get; } = "<size=120%><b>Freeze-dried Cauldron</b><size=20%>\n\n<size=100%>" +
            "The dried remains of a cauldron, reconstitute with water. " +
            "Avoid breathing in the powder.<size=20%>\n\n<size=100%>" +
            "Every time <b>Temperature Sickness</b> is applied to an enemy, apply <b>1 additional Temperature Sickness</b>.";

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void OnEnable()
        {
            EnemyEvents.OnAppliedTemperatureSickness += ApplyExtraSickness;
        }

        private void OnDisable()
        {
            EnemyEvents.OnAppliedTemperatureSickness -= ApplyExtraSickness;
        }

        void Start()
        {
            Image image = GetComponent<Image>();
            image.enabled = (SceneManager.GetActiveScene().name.Contains("Combat"));

            ScrollDisplayer displayer = GetComponent<ScrollDisplayer>();
            displayer.DisplayDescription = Description;
            displayer.DisplayScale = ScrollDisplayScales.RelicDisplayScale;
        }

        private void ApplyExtraSickness(Enemy enemy)
        {
            //if sourceIsRelic, will not invoke event, preventing infinte recursion
            enemy.ApplyTemperatureSickness(1, true);
            transform.DOPunchScale(new(1.1f, 1.1f), 0.2f, 1, 1);
        }
    }
}
