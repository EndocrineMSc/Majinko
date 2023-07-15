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
        private OverworldPlayer _player;

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
            _player = OverworldPlayer.Instance;
        }

        private void OnButtonClick()
        {
            if (CheckIfPlayerInViablePosition())
                StartCoroutine(OverworldPlayer.Instance.MoveToNextElement(_elementButton));          
        }

        internal abstract void TriggerSceneTransition();

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

        #endregion
    }
}
