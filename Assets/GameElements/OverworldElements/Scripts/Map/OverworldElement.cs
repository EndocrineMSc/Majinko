using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        protected void Start()
        {
            _elementButton = GetComponent<Button>();
            _elementButton.onClick.AddListener(OnButtonClick);
            _player = OverworldPlayer.Instance;
        }

        private void OnButtonClick()
        {
            foreach (Button button in _validEntryPositions)
            {
                if (button == _player.CurrentOverworldElementPosition)
                {
                    StartCoroutine(OverworldPlayer.Instance.MoveToNextElement(_elementButton));
                    return;
                }
            }
        }

        internal abstract void TriggerSceneTransition();

        #endregion
    }
}
