using Cinemachine;
using UnityEngine;

namespace Utility
{
    internal class ScreenShaker : MonoBehaviour
    {
        #region Fields and Properties

        internal static ScreenShaker Instance { get; private set; }
        private CinemachineVirtualCamera _virtualCamera;
        private float _timer = 0;
        private bool _isShaking;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);

            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Start()
        {
            StopShake();
        }

        internal void ShakeCamera(float amplitude = 5f, float shakeTime = 0.2f)
        {
            if (!_isShaking)
            {
                _isShaking = true;
                var perlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = amplitude;
                _timer = shakeTime;
            }
        }

        private void StopShake()
        {
            var perlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = 0f;
            _timer = 0;
            _isShaking = false;
        }

        private void Update()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;

                if (_timer <= 0)
                    StopShake();
            }
        }

        #endregion
    }
}
