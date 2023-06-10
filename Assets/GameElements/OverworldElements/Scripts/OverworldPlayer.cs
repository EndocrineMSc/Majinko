using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Overworld
{
    internal class OverworldPlayer : MonoBehaviour
    {
        #region Fields and Properties

        internal static OverworldPlayer Instance { get; private set; }
        internal Button CurrentOverworldElementPosition { get; private set; }

        private readonly float _tweenDuration = 1f;
        private List<Button> _sortedButtons;
        private RectTransform _playerTransform;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            List<Button> allButtons = GameObject.FindObjectsOfType<Button>().ToList();
            List<Button> overworldElementButtons = new();

            foreach (Button button in allButtons)
            {
                if (button.TryGetComponent<OverworldElement>(out _))
                    overworldElementButtons.Add(button);
            }

            _sortedButtons = overworldElementButtons.OrderBy(button => button.GetComponent<RectTransform>().anchoredPosition.x).ToList();

            CurrentOverworldElementPosition = _sortedButtons[CurrentPlayerWorldPosition.OverworldPlayerButtonIndex];
            _playerTransform = GetComponent<RectTransform>();
            _playerTransform.anchoredPosition = CurrentOverworldElementPosition.GetComponent<RectTransform>().anchoredPosition;
        }

        internal IEnumerator MoveToNextElement(Button overworldButton)
        {
            Vector3 targetPosition = overworldButton.GetComponent<RectTransform>().position;
            _playerTransform.DOMove(targetPosition, _tweenDuration).SetEase(Ease.InBack);

            SetNewOverworldPosition(overworldButton);
            yield return new WaitForSeconds(_tweenDuration);

            OverworldElement newElement = overworldButton.GetComponent<OverworldElement>();

            if (newElement != null)
            {
                newElement.TriggerSceneTransition();
            }
            else
            {
                Debug.Log("No overworld element assigned to button!");
                throw new NotImplementedException();
            }
        }

        private void SetNewOverworldPosition(Button newButton)
        {
            int newButtonIndex = -1;
            CurrentOverworldElementPosition = newButton;

            for (int i = 0; i < _sortedButtons.Count; i++)
            {
                if (_sortedButtons[i] == newButton)
                {
                    newButtonIndex = i;
                    break;
                }
            }

            if (newButtonIndex >= 0)
            {
                CurrentPlayerWorldPosition.SetPlayerButtonIndex(newButtonIndex);
            }
            else
            {
                Debug.Log("Correct button was not found in element button list");
            }
        }

        #endregion
    }
}
