using PeggleMana;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using Cards.DragDrop;
using Cards.ScriptableCards;

namespace Cards
{
    [RequireComponent(typeof(CardDragDrop))]
    public abstract class Card : MonoBehaviour
    {
        #region Fields

        private string _cardName;
        private string _cardDescription;
        private int _manaCost;
        private ManaType _manaType;
        private ManaPoolManager _instance;
        private bool _enoughMana;
        private Vector3 _startPosition;
        private CardType _cardType;

        [SerializeField] protected ScriptableCard ScriptableCard;

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

        protected virtual void Start()
        {
            _startPosition = gameObject.transform.position;
            _instance = ManaPoolManager.Instance;

            _cardName = ScriptableCard.CardName;
            _cardDescription = ScriptableCard.CardDescription;
            _manaCost = ScriptableCard.ManaCost;
            _manaType = ScriptableCard.ManaType;
            _cardType = ScriptableCard.CardType;
        }

        protected virtual void SubtractManaCost()
        {
            _instance.SpendMana(ManaType.BaseMana, _manaCost);
        }

        protected virtual void CardEffect()
        {
            Debug.Log("Test effect.");
        }

        protected virtual void CheckForMana()
        {
            if (_instance.BasicMana.Count >= _manaCost)
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
