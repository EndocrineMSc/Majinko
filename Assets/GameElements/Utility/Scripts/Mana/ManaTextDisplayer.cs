using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ManaManagement
{
    internal class ManaTextDisplayer : MonoBehaviour
    {
        #region Fields and Properties

        private ManaPool _manaPool;
        [SerializeField] private bool _isBasicMana;
        [SerializeField] private bool _isFireMana;
        [SerializeField] private bool _isIceMana;
        private TextMeshProUGUI _manaTextField;

        #endregion

        #region Functions

        // Start is called before the first frame update
        void Start()
        {
            _manaPool = ManaPool.Instance != null ? ManaPool.Instance : null;
            _manaTextField = GetComponent<TextMeshProUGUI>();
           
            if (_isBasicMana && _isFireMana
                || _isBasicMana && _isIceMana
                || _isFireMana && _isIceMana)
            {
                Debug.Log("Wrong mana assignment. Assign only on type of mana to this script. Defaulted to BasicMana");
                _isBasicMana = true;
                _isFireMana = false;
                _isIceMana = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_manaPool != null)
            {
                int basicManaAmount = _manaPool.BasicMana.Count / _manaPool.ManaCostMultiplier;
                int fireManaAmount = _manaPool.FireMana.Count / _manaPool.ManaCostMultiplier;
                int iceManaAmount = _manaPool.FireMana.Count / _manaPool.ManaCostMultiplier;

                if (_isBasicMana)
                    _manaTextField.text = basicManaAmount.ToString();
                else if (_isFireMana)
                    _manaTextField.text = fireManaAmount.ToString();
                else
                    _manaTextField.text = iceManaAmount.ToString();
            }
        }

        #endregion
    }
}
