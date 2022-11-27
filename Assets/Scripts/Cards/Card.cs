using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace Cards
{
    public class Card : MonoBehaviour
    {
        #region Fields

        protected string _cardName;
        protected string _cardText;
        protected string _baseManaCost;

        #endregion

        #region Private Virtual Functions

        protected virtual void CauseEffect()
        {
            Debug.Log("Effect not implemented yet.");
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
