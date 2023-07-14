using Utility;

namespace Characters
{
    internal class PlayerConditionTracker
    {
        internal static int PlayerHealth { get; private set; } = 100;
        internal static int MaxPlayerHealth { get; private set; } = 100;
        internal static int TemporaryBasicMana { get; private set; } = 0;


        internal static void SetPlayerHealth(int health)
        {
            PlayerHealth = health;
        }

        internal static void HealPlayer(int amount)
        {
            PlayerHealth += amount;

            if (PlayerHealth > MaxPlayerHealth)
                PlayerHealth = MaxPlayerHealth;
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

        internal static void AddTemporaryBasicMana(int amount = 1)
        {
            TemporaryBasicMana += amount;
        }

        internal static void ResetTemporaryBasicMana()
        { 
            TemporaryBasicMana = 0;
        }

        internal static void OnGameReset()
        {
            PlayerHealth = 100;
            MaxPlayerHealth = 100;
            ResetTemporaryBasicMana();
        }
    }
}
