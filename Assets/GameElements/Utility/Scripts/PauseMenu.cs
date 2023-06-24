using Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    internal class PauseMenu : MonoBehaviour
    {
        #region Fields and Properties

        private Canvas _settingsCanvas;
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _musicSlilder;
        [SerializeField] private Slider _volumeSlider;

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            _settingsCanvas = GetComponent<Canvas>();
            _settingsCanvas.enabled = false;
            AudioManager audioManager = AudioManager.Instance;
            _masterSlider.onValueChanged.AddListener(audioManager.SetMasterVolume);
            _musicSlilder.onValueChanged.AddListener(audioManager.SetMusicVolume);
            _volumeSlider.onValueChanged.AddListener(audioManager.SetEffectsVolume);

            PauseControl.Instance.PauseAndUnpauseGame?.AddListener(OpenAndCloseCanvas);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                PauseControl.Instance.PauseAndUnpauseGame?.Invoke();
            }
        }

        void OpenAndCloseCanvas()
        {
            if (_settingsCanvas.isActiveAndEnabled)
            {
                _settingsCanvas.enabled = false;
            }
            else
            {
                _settingsCanvas.enabled = true;
            }
        }

        private void OnDisable()
        {
            PauseControl.Instance.PauseAndUnpauseGame?.RemoveListener(OpenAndCloseCanvas);
        }
    }
}
