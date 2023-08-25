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
    internal class HoarfrostDaggerRelic : MonoBehaviour, IRelic
    {

        internal static HoarfrostDaggerRelic Instance { get; private set; }

        public Relic RelicEnum { get; private set; } = Relic.HoarfrostDaggerRelic;

        public string Description { get; } = "<size=120%><b>Hoarfrost Dagger</b><size=20%>\n\n<size=100%>" +
            "An icy dagger. Condensating mist swirls around it.<size=20%>\n\n<size=100%>" +
            "Every time <b>Freezing</b> is applied to an enemy, apply <b>1 additional Freezing</b>.";

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void OnEnable()
        {
            EnemyEvents.OnAppliedFreezing += ApplyExtraFreezing;
        }

        private void OnDisable()
        {
            EnemyEvents.OnAppliedFreezing -= ApplyExtraFreezing;
        }

        void Start()
        {
            Image image = GetComponent<Image>();
            image.enabled = (SceneManager.GetActiveScene().name.Contains("Combat"));

            ScrollDisplayer displayer = GetComponent<ScrollDisplayer>();
            displayer.DisplayDescription = Description;
            displayer.DisplayScale = ScrollDisplayScales.RelicDisplayScale;
        }

        private void ApplyExtraFreezing(Enemy enemy)
        {
            //if sourceIsRelic, will not invoke event, preventing infinte recursion
            enemy.ApplyFreezing(1, true);
            transform.DOPunchScale(new(1.1f, 1.1f), 0.2f, 1, 1);
        }
    }
}
