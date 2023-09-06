using System;

namespace Characters.Enemies
{
    public class EnemyEvents
    {
        #region Fields and Properties
        
        public static event Action OnEnemyDied;
        public static event Action OnEnemyFinishedMoving;
        public static event Action OnEnemiesFinishedAttacking;
        public static event Action<int> OnIntangibleTriggered;
        public static event Action<Enemy> OnAppliedBurning;
        public static event Action<Enemy> OnAppliedFreezing;
        public static event Action<Enemy> OnAppliedFrozen;
        public static event Action<Enemy> OnAppliedTemperatureSickness;

        #endregion

        #region Functions

        public static void RaiseOnEnemyDeath()
        {
            OnEnemyDied?.Invoke();
        }

        public static void RaiseOnEnemyFinishedMoving()
        {
            OnEnemyFinishedMoving?.Invoke();
        }

        public static void RaiseOnEnemiesFinishedAttacking()
        {
            OnEnemiesFinishedAttacking?.Invoke();
        }

        public static void RaiseAppliedBurning(Enemy enemy)
        {
            OnAppliedBurning?.Invoke(enemy);
        }

        public static void RaiseAppliedFreezing(Enemy enemy)
        {
            OnAppliedFreezing?.Invoke(enemy);
        }

        public static void RaiseAppliedFrozen(Enemy enemy)
        {
            OnAppliedFrozen?.Invoke(enemy);
        }

        public static void RaiseAppliedTemperatureSickness(Enemy enemy)
        {
            OnAppliedTemperatureSickness?.Invoke(enemy);
        }

        public static void RaiseIntangibleTriggered(int amount = 1)
        {
            OnIntangibleTriggered?.Invoke(amount);
        }

        #endregion
    }
}
