using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Utility
{
    internal class TutorialText : MonoBehaviour
    {
        #region Fields and Properties

        private TextMeshProUGUI _textField;
        [SerializeField, TextArea] private string[] _tutorialTexts;

        private int _testIndex = 0;

        #endregion

        #region Functions

        private void Awake()
        {
            _textField = GetComponent<TextMeshProUGUI>();
            _textField.text = _tutorialTexts[0];
        }

        private void OnEnable()
        {
            TutorialEvents.OnCurrentActionCompleted += DisplayNextText;
        }

        private void OnDisable()
        {
            TutorialEvents.OnCurrentActionCompleted -= DisplayNextText;
        }

        private void DisplayNextText(int index)
        {
            if (index < _tutorialTexts.Length)
                _textField.text = _tutorialTexts[index];
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _testIndex++;
                DisplayNextText(_testIndex);
            }
        }

        #endregion
    }
}
