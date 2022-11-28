using PeggleMana;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using EnumCollection;

namespace Cards
{
    public class Card : MonoBehaviour
    {
        #region Fields

        protected string _cardName;
        protected string _cardText;
        protected int _baseManaCost = 20;

        #endregion

        #region Private Virtual Functions

        public virtual void CauseEffect()
        {
            Debug.Log("Test effect.");
            ManaPoolManager.Instance.SpendMana(ManaType.BaseMana, _baseManaCost);
        }

        protected virtual void SubtractManaCost()
        {

        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #endregion
    }
}
