using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Overworld
{
    internal abstract class OverworldElement : MonoBehaviour
    {
        #region Fields and Properties

        private Button _elementButton;

        [Header("Valid Entry Positions (Buttons)")]
        [SerializeField] private Button[] _validEntryPositions;

        #endregion

        #region Functions

        protected void OnEnable()
        {
            UtilityEvents.OnOverworldPlayerPositionChange += DisableLocationIfNotViable;
        }

        protected void OnDisable()
        {
            UtilityEvents.OnOverworldPlayerPositionChange -= DisableLocationIfNotViable;
        }

        protected virtual void Start()
        {
            _elementButton = GetComponent<Button>();
            _elementButton.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            if (CheckIfPlayerInViablePosition())
                StartCoroutine(OverworldPlayer.Instance.MoveToNextElement(_elementButton));          
        }

        internal virtual void TriggerSceneTransition()
        {
            StartCoroutine(LoadWithFade());
        }

        protected bool CheckIfPlayerInViablePosition()
        {
            foreach (Button button in _validEntryPositions)
            {
                if (OverworldPlayer.Instance.CurrentOverworldElementPosition == button)
                    return true;
            }
            return false;
        }

        protected void DisableLocationIfNotViable()
        {
            GetComponent<Button>().interactable = CheckIfPlayerInViablePosition();
        }

        protected IEnumerator LoadWithFade()
        {
            if (FadeCanvas.Instance != null)
                FadeCanvas.Instance.FadeToBlack();

            yield return new WaitForSeconds(LoadHelper.LoadDuration);
            LoadScene();
        }

        protected abstract void LoadScene();

        #endregion
    }
}
