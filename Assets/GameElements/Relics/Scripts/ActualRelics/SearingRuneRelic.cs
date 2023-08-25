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
    internal class SearingRuneRelic : MonoBehaviour, IRelic
    {

        internal static SearingRuneRelic Instance { get; private set; }

        public Relic RelicEnum { get; private set; } = Relic.SearingRune;

        public string Description { get; } = "<size=120%><b>Searing Rune</b><size=20%>\n\n<size=100%>" +
            "A mysterious stone, bearing the carving of a fire rune." +
            " It is hot to the touch.<size=20%>\n\n<size=100%>" +
            "Every time <b>Burning</b> is applied to an enemy, apply <b>1 additional Burning</b>.";

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void OnEnable()
        {
            EnemyEvents.OnAppliedBurning += ApplyExtraBurning;
        }

        private void OnDisable()
        {
            EnemyEvents.OnAppliedBurning -= ApplyExtraBurning;
        }

        void Start()
        {
            Image image = GetComponent<Image>();
            image.enabled = (SceneManager.GetActiveScene().name.Contains("Combat"));

            ScrollDisplayer displayer = GetComponent<ScrollDisplayer>();
            displayer.DisplayDescription = Description;
            displayer.DisplayScale = ScrollDisplayScales.RelicDisplayScale;
        }

        private void ApplyExtraBurning(Enemy enemy)
        {
            //if sourceIsRelic, will not invoke AppliedBurningEvent, preventing infinte recursion
            enemy.ApplyBurning(1, true);
            transform.DOPunchScale(new(1.1f, 1.1f), 0.2f, 1, 1);
        }
    }
}
