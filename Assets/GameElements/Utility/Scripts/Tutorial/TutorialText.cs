using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Spheres;
using Utility.TurnManagement;


namespace Utility
{
    internal class TutorialText : MonoBehaviour
    {
        #region Fields and Properties

        private TextMeshProUGUI _textField;
        [SerializeField, TextArea] private string[] _tutorialTexts;
        [SerializeField] private Image _scrollImage;
        [SerializeField] private GameObject _fieldDescriptionLines;
        [SerializeField] private Image _previewCard;
        [SerializeField] private Image _previewCardArrow;
        [SerializeField] private Image _previewCardBox;

        private int _index = 0;
        private bool _isStopped;
        
        internal bool NextTextIsBlocked { get; set; }

        #endregion

        #region Functions

        private void Awake()
        {
            _textField = GetComponent<TextMeshProUGUI>();
            _textField.text = _tutorialTexts[0];
        }

        private void OnEnable()
        {
            SphereEvents.OnSphereDestruction += OnSphereDestruction;
            LevelPhaseEvents.OnStartEnemyPhase += OnStartEnemyPhase;
        }

        private void OnDisable()
        {
            SphereEvents.OnSphereDestruction -= OnSphereDestruction;
            LevelPhaseEvents.OnStartEnemyPhase -= OnStartEnemyPhase;
        }

        private void DisplayNextText(int index)
        {
            if (index < _tutorialTexts.Length)
                _textField.text = _tutorialTexts[index];
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !NextTextIsBlocked)
            {
                HandleMouseClick();          
                DisplayNextText(_index);
            }
        }

        private void HandleMouseClick()
        {
            switch (_index)
            {
                case 2:
                    _scrollImage.color = Color.clear;
                    _textField.color = Color.clear;
                    _fieldDescriptionLines.SetActive(true);
                    _index++;
                    break;

                case 3:
                    if (!_isStopped)
                    {
                        _isStopped = true;
                        _scrollImage.color = Color.white;
                        _textField.color = Color.black;
                        _fieldDescriptionLines.SetActive(false);
                    }
                    else
                    {
                        _isStopped = false;
                        _index++;
                    }
                    break;

                case 5:
                    _previewCard.color = Color.white;
                    _previewCardArrow.color = Color.white;
                    _index++;
                    break;

                case 6:
                    _previewCardArrow.color = Color.clear;
                    _previewCardBox.color = Color.white;
                    _index++;
                    break;

                case 9:
                    _previewCard.color = Color.clear;
                    _previewCardBox.color = Color.clear;
                    _index++;
                    break;

                case 13:                    
                    NextTextIsBlocked = true;
                    _textField.color = Color.clear;
                    _scrollImage.color = Color.clear;
                    Time.timeScale = 1;
                    _index++;
                    break;

                case 15:
                    _textField.color = Color.clear;
                    _scrollImage.color = Color.clear;
                    Time.timeScale = 1;
                    _index++;
                    break;

                case 20:
                    NextTextIsBlocked = true;
                    Time.timeScale = 1;
                    _textField.color = Color.clear;
                    _scrollImage.color = Color.clear;
                    break;

                default:
                    _index++;
                    break;
            }
        }

        private void OnSphereDestruction()
        {
            Time.timeScale = 0;
            NextTextIsBlocked = false;
            _textField.color = Color.black;
            _scrollImage.color = Color.white;
            SphereEvents.OnSphereDestruction -= OnSphereDestruction;
        }

        private void OnStartEnemyPhase()
        {
            Time.timeScale = 0;
            _textField.color = Color.black;
            _scrollImage.color = Color.white;
            LevelPhaseEvents.OnStartEnemyPhase -= OnStartEnemyPhase;
        }

        #endregion
    }
}
