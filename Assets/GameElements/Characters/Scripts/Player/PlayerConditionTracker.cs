using Utility;

namespace Characters
{
    internal class PlayerConditionTracker
    {
        internal static int PlayerHealth { get; private set; } = 100;
        internal static int MaxPlayerHealth { get; private set; } = 100;


        internal static void SetPlayerHealth(int health)
        {
            PlayerHealth = health;
        }

        internal static void SetMaxHealth(int amount)
        {
            MaxPlayerHealth = amount;

            if (PlayerHealth > MaxPlayerHealth)
                PlayerHealth = MaxPlayerHealth;
        }

        internal static void DecreaseMaxHealth(int amount)
        {
            MaxPlayerHealth -= amount;

            if (PlayerHealth > MaxPlayerHealth)
                PlayerHealth = MaxPlayerHealth;
        }

        internal static void IncreaseMaxHealth(int amount)
        {
            MaxPlayerHealth += amount;
            PlayerHealth += amount;
        }

        public static void OnGameReset()
        {
            PlayerHealth = 100;
            MaxPlayerHealth = 100;
        }
    }
}
