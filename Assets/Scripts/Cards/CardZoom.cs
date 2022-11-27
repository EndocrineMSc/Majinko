using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards.Zoom
{
    public class CardZoom : MonoBehaviour
    {
        #region Fields

        private Card _card;
        private SpriteRenderer _renderer;

        #endregion

        #region Private Functions

        private void Start()
        {
            _card = this.GetComponent<Card>();

        }

        private void OnPointerEnter(PointerEventData eventData)
        {

        }
        #endregion

        #region Public Functions
        public void TriggerCost()
        {
            //ToDo: Make respective Mana fly to player if enough mana is present
        }

        #endregion

        #region IEnumerators

        #endregion
    }
}
