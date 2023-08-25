using System;

namespace Characters.Enemies
{
    internal class EnemyEvents
    {
        #region Fields and Properties
        
        internal static event Action OnEnemyDied;
        internal static event Action OnEnemyFinishedMoving;
        internal static event Action OnEnemiesFinishedAttacking;
        internal static event Action<Enemy> OnAppliedBurning;
        internal static event Action<Enemy> OnAppliedFreezing;
        internal static event Action<Enemy> OnAppliedFrozen;
        internal static event Action<Enemy> OnAppliedTemperatureSickness;

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

        internal static void RaiseAppliedBurning(Enemy enemy)
        {
            OnAppliedBurning?.Invoke(enemy);
        }

        internal static void RaiseAppliedFreezing(Enemy enemy)
        {
            OnAppliedFreezing?.Invoke(enemy);
        }

        internal static void RaiseAppliedFrozen(Enemy enemy)
        {
            OnAppliedFrozen?.Invoke(enemy);
        }

        internal static void RaiseAppliedTemperatureSickness(Enemy enemy)
        {
            OnAppliedTemperatureSickness?.Invoke(enemy);
        }

        #endregion
    }
}
