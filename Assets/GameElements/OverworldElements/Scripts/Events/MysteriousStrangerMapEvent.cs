using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;
using UnityEngine.UI;
using Utility;

internal class MysteriousStrangerMapEvent : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] private Button _tradeButton;
    [SerializeField] private Button _leaveButton;
    [SerializeField] private DeckLibrary _deckLibrary;

    #endregion

    #region Functions

    void Start()
    {
        if (GlobalDeckManager.Instance != null && GlobalDeckManager.Instance.GlobalDeck.Count == 0)
            _tradeButton.interactable = false;

        _tradeButton.onClick.AddListener(TradeCard);
        _tradeButton.onClick.AddListener(DisableAllButtons);

        _leaveButton.onClick.AddListener(Leave);
        _leaveButton.onClick.AddListener(DisableAllButtons);
    }

    private void TradeCard()
    {
        _deckLibrary.SetUpCurrentDeckLibrary("Select card to exchange", DeckLibraryAction.ExchangeCard);
    }

    private void Leave()
    {
        LoadHelper.LoadSceneWithLoadingScreen(SceneName.WorldOne);
    }

    private void DisableAllButtons()
    {
        _tradeButton.interactable = false;
        _leaveButton.interactable = false;
    }

    #endregion
}
