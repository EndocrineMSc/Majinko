using Audio;
using Characters;
using Characters.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Overworld
{
    internal class MagicalSpringMapEvent : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private Button _healButton;
        [SerializeField] private Button _manaButton;
        [SerializeField] private Button _leaveButton;
        [SerializeField] private GameObject _basicMana;

        #endregion

        void Start()
        {
            _healButton.onClick.AddListener(HealPlayer);
            _manaButton.onClick.AddListener(AddManaForNextCombat);
            _leaveButton.onClick.AddListener(Leave);
        }

        private void HealPlayer()
        {
            PlayButtonClick();
            DisableButtons();

            var maxHealth = PlayerConditionTracker.MaxPlayerHealth;
            var amountToHeal = (int)(maxHealth * 0.2f);
            PlayerConditionTracker.HealPlayer(amountToHeal);
          
            var popUpSpawner = Player.Instance.GetComponent<PopUpSpawner>();
            popUpSpawner.SpawnPopUp(amountToHeal, "#0B6612"); //green Color

            StartCoroutine(LoadSceneAfterDelay());
        }

        private void AddManaForNextCombat()
        {
            PlayButtonClick();
            DisableButtons();
            PlayerConditionTracker.AddTemporaryBasicMana(2);
            StartCoroutine(LoadSceneAfterDelay());
        }

        private void Leave()
        {
            PlayButtonClick();
            DisableButtons();
            StartCoroutine(LoadSceneAfterDelay());
        }

        private void DisableButtons()
        {
            _healButton.interactable = false;
            _manaButton.interactable = false;
            _leaveButton.interactable = false;
        }

        private void PlayButtonClick()
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySoundEffectOnce(SFX._0001_ButtonClick);
        }

        private IEnumerator LoadSceneAfterDelay()
        {
            if (FadeCanvas.Instance != null)
                FadeCanvas.Instance.FadeToBlack();

            yield return new WaitForSeconds(LoadHelper.LoadDuration);
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.WorldOne);
        }
    }
}
