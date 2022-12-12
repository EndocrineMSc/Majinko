using PeggleMana;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using Cards.DragDrop;


namespace Cards
{
    [RequireComponent(typeof(CardDragDrop))]
    public class Card : MonoBehaviour
    {
        #region Fields

        protected string _cardName;
        protected string _cardText;
        protected int _baseManaCost = 20;
        protected ManaType _manaType = ManaType.BaseMana;
        private ManaPoolManager _instance;
        protected bool _enoughMana;
        private Vector3 _startPosition;

        #endregion

        #region Public Virtual Functions

        public virtual void CardDropEffect()
        {
            CheckForMana();
            if (_enoughMana)
            {
                SubtractManaCost();
                CardEffect();
                //ToDo: put Card into discard pile list
                StartCoroutine(DestroyCard());
            }
            else
            {
                //ToDo: Player Feedback for not enough mana
                gameObject.transform.position = _startPosition;
            }
        }

        #endregion

        #region Protected Virtual Functions

        protected void Start()
        {
            _startPosition = gameObject.transform.position;
            _instance = ManaPoolManager.Instance;
        }

        protected virtual void SubtractManaCost()
        {
            _instance.SpendMana(ManaType.BaseMana, _baseManaCost);
        }

        protected virtual void CardEffect()
        {
            Debug.Log("Test effect.");
        }

        protected virtual void CheckForMana()
        {
            if (_instance.BasicMana.Count >= _baseManaCost)
            {
                _enoughMana = true;
            }
        }

        #endregion

        #region IEnumerators

        private IEnumerator DestroyCard()
        {
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }

        #endregion
    }
}
