using Characters.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    internal class IntangibleController : ICanBeIntangible
    {
        #region Fields and Properties

        public int IntangibleStacks { get; set; }
        private Enemy _enemy;
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider;
        private ICanBeIntangible _intangibleEnemy;

        #endregion

        #region Constructor

        internal IntangibleController(Enemy enemy) 
        { 
            _enemy = enemy;
            _spriteRenderer = _enemy.GetComponentInChildren<SpriteRenderer>();
            _collider = _enemy.GetComponent<Collider2D>();
            _intangibleEnemy = _enemy.GetComponent<ICanBeIntangible>();
        }

        #endregion

        #region Functions

        public void HandleIntangibleStacks()
        {
            if (IntangibleStacks > 0)
            {
                IntangibleStacks--;
                _intangibleEnemy.IntangibleStacks = this.IntangibleStacks;
            }
            if (IntangibleStacks <= 0)
            {
                IntangibleStacks = 0;
                _intangibleEnemy.IntangibleStacks = this.IntangibleStacks;
                RemoveIntangible();
            }
        }

        public void RemoveIntangible()
        {
            Color alpha = _spriteRenderer.color;
            alpha.a = 1f;
            _spriteRenderer.color = alpha;

            _collider.enabled = true;
        }

        public void SetIntangible(int intangibleStacks = 1)
        {
            IntangibleStacks += intangibleStacks;
            _intangibleEnemy.IntangibleStacks = this.IntangibleStacks;

            Color alpha = _spriteRenderer.color;
            alpha.a = 0.5f;
            _spriteRenderer.color = alpha;

            _collider.enabled = false;
        }

        #endregion
    }
}
