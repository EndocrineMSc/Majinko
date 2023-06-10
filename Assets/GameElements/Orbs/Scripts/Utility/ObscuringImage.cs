using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility.TurnManagement;

namespace Orbs
{
    internal class ObscuringImage : MonoBehaviour
    {
        #region Fields and Properties

        private Image _obscuringImage;
        private bool _willBeFaded;
        private float _fadeSpeed = 1f;

        #endregion

        #region Functions

        private void OnEnable()
        {
            ObscuringImageHandler.OnObscuringImageFade += OnFadeTriggered;
            LevelPhaseEvents.OnStartShootingPhase += OnFadeStart;
            LevelPhaseEvents.OnStartEnemyPhase += OnFadeEnd;
        }

        private void OnDisable()
        { 
            ObscuringImageHandler.OnObscuringImageFade -= OnFadeTriggered;
            LevelPhaseEvents.OnStartShootingPhase -= OnFadeStart;
            LevelPhaseEvents.OnStartEnemyPhase -= OnFadeEnd;
        }

        // Start is called before the first frame update
        void Start()
        {
            _obscuringImage = GetComponent<Image>();
            _obscuringImage.DOFade(0, 0.1f);
        }

        private void OnFadeTriggered()
        {
            _willBeFaded = true;
        }

        private void OnFadeStart()
        {
            if (_willBeFaded)
            {
                _obscuringImage.DOFade(1, _fadeSpeed);
                _willBeFaded = false;
            }
        }

        private void OnFadeEnd()
        {
            _obscuringImage.DOFade(0, _fadeSpeed);
        }

        #endregion
    }
}
