using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Overworld
{
    internal class FadeCanvas : MonoBehaviour
    {
        internal static FadeCanvas Instance { get; private set; }

        [SerializeField] private Image _fadeImage;
        internal Image FadeImage { get { return _fadeImage; } }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            _fadeImage.enabled = true;
            _fadeImage.DOFade(0, LoadHelper.LoadDuration);
        }
    }
}
