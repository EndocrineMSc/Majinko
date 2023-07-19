using System;

namespace Characters.Enemies
{
    internal class EnemyEvents
    {
        #region Fields and Properties
        
        internal static event Action OnEnemyDied;
        internal static event Action OnEnemyFinishedMoving;
        internal static event Action OnEnemiesFinishedAttacking;
        internal static event Action OnAppliedBurning;
        internal static event Action OnAppliedFreezing;
        internal static event Action OnAppliedFrozen;

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

        internal static void RaiseAppliedBurning()
        {
            OnAppliedBurning?.Invoke();
        }

        internal static void RaiseAppliedFreezing()
        {
            OnAppliedFreezing?.Invoke();
        }

        internal static void RaiseAppliedFrozen()
        {
            OnAppliedFrozen?.Invoke();
        }

        #endregion
    }
}
