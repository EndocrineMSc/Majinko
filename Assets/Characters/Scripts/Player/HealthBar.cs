using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Characters.UI
{
    public class HealthBar : MonoBehaviour
    {
        #region Fields

        private Player _player;
        [SerializeField] private Image _heart;
        [SerializeField] private Image _shield;
        [SerializeField] private Slider _healthFill;
        [SerializeField] private Slider _shieldFill;

        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private TextMeshProUGUI _shieldText;

        #endregion

        #region Private Function

        // Start is called before the first frame update
        void Start()
        {
            _player = Player.Instance;

            _healthFill.maxValue = _player.MaxHealth;
            _shieldFill.maxValue = _player.MaxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            _healthText.text = _player.Health.ToString() + " / " + _player.MaxHealth.ToString();
            _healthFill.value = _player.Health;
            _shieldFill.value = _player.Shield;

            if (_player.Shield > 0)
            {
                _shield.enabled = true;
                _heart.enabled = false;
                _shieldText.enabled = true;

                _shieldText.text = _player.Shield.ToString();
            }
            else
            {
                _shield.enabled = false;
                _heart.enabled = true;
                _shieldText.enabled = false;
            }
        }

        #endregion
    }
}
