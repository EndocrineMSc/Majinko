using DG.Tweening;
using Overworld;
using System.Collections;
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

            if (FadeCanvas.Instance != null)
                FadeCanvas.Instance.FadeToBlack();

            StartCoroutine(LoadAfterDelay());
        }

        private IEnumerator LoadAfterDelay()
        {
            yield return new WaitForSeconds(LoadHelper.LoadDuration);
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.MainMenu);
        }
    }
}
