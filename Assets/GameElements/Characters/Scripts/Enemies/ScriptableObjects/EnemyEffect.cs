using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Enemies
{
    public class EnemyEffect : ScriptableObject
    {
        protected Enemy _enemy;

        public virtual void InitializeObject(Enemy enemy)
        {
            _enemy = enemy;
        }
    }
}
