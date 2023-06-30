using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    internal class LeaveButton : MonoBehaviour
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
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.MainMenu);
        }
    }
}
