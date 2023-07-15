using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;
using UnityEngine.UI;
using Utility;

namespace Overworld
{
    internal class MirrorTreeMapEvent : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private Button _copyButton;
        [SerializeField] private Button _leaveButton;
        [SerializeField] private DeckLibrary _deckLibrary;

        #endregion

        #region Functions

        private void Start()
        {
            if (GlobalDeckManager.Instance != null && GlobalDeckManager.Instance.GlobalDeck.Count == 0)
                _copyButton.interactable = false;

            _copyButton.onClick.AddListener(CopyCard);
            _copyButton.onClick.AddListener(DisableAllButtons);

            _leaveButton.onClick.AddListener(Leave);
            _leaveButton.onClick.AddListener(DisableAllButtons);
        }

        private void CopyCard()
        {
            _deckLibrary.SetUpCurrentDeckLibrary("Choose card to copy", DeckLibraryAction.AddCard);
        }

        private void Leave()
        {
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.WorldOne);
        }

        private void DisableAllButtons()
        {
            _copyButton.interactable = false;
            _leaveButton.interactable = false;
        }

        #endregion;
    }
}
