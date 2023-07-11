using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    internal class AbandonConfirmButton : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(LoadWrap);
        }

        private void LoadWrap()
        {
            if (PauseControl.Instance.GameIsPaused)
                PauseControl.Instance.PauseAndUnpauseGame?.Invoke();

            UtilityEvents.RaiseGameReset();
            ES3.DeleteKey("GlobalDeck");
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.MainMenu);
        }
    }
}
