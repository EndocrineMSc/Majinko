using System;

namespace Characters.Enemies
{
    internal class EnemyEvents
    {
        #region Fields and Properties
        
        internal static event Action OnEnemyDied;
        internal static event Action OnEnemyFinishedMoving;
        internal static event Action OnEnemiesFinishedAttacking;

        #endregion

        #region Functions
        
        internal static void RaiseOnEnemyDeath()
        {
            OnEnemyDied?.Invoke();
        }

        internal static void RaiseOnEnemyFinishedMoving()
        {
            OnEnemyFinishedMoving?.Invoke();
        }

        internal static void RaiseOnEnemiesFinishedAttacking()
        {
            OnEnemiesFinishedAttacking?.Invoke();
        }

        #endregion
    }
}
