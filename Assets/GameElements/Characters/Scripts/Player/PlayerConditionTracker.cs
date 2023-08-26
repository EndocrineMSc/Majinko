using UnityEngine;
using Utility;

namespace Characters
{
    internal class PlayerConditionTracker
    {
        #region Fields and Properties

        #region Health

        internal static int PlayerHealth { get; private set; } = 100;
        internal static int MaxPlayerHealth { get; private set; } = 100;

        #endregion

        #region Buffs and Debuffs
        //All Buffs and Debuffs in here should be aimed for level scale and temporary (triggered by cards and overworld events)
        //Permanent Buffs should be handled by relics

        internal static int TemporaryBasicMana { get; private set; } = 0;
        internal static int SicknessStacks { get; private set; } = 0;
        internal static int FastHandStacks { get; private set; } = 0;
        internal static bool HasShieldBeetle { get; private set; } = false;
        internal static bool HasBubbleWand { get; private set; } = false;
        internal static bool HasOrbInlayedGauntlets { get; private set; } = false;
        internal static bool HasWardingRune { get; private set; } = false;

        #endregion

        #endregion

        #region Functions

        #region Health

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

        internal static void DamagePlayer(int amount)
        {
            PlayerHealth -= amount;
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

        #endregion

        #region Buffs and Debuffs

        internal static void AddTemporaryBasicMana(int amount = 1)
        {
            TemporaryBasicMana += amount;
        }

        internal static void ResetTemporaryBasicMana()
        {
            TemporaryBasicMana = 0;
        }

        internal static void AddFastHandsStacks(int amount = 1)
        {
            FastHandStacks += amount;
        }

        internal static void ResetFastHandsStacks()
        {
            FastHandStacks = 0;
        }

        internal static void AddSicknessStacks(int amount = 1)
        {
            SicknessStacks += amount;
        }

        internal static void ResetSicknessStacks()
        {
            SicknessStacks = 0;
        }

        internal static void ActivateShieldBeetle()
        {
            HasShieldBeetle = true;
        }

        internal static void ActivateBubbleWand()
        {
            HasBubbleWand = true;
        }

        internal static void ActivateOrbInlayedGauntlets()
        {
            HasOrbInlayedGauntlets = true;
        }

        internal static void ActivateWardingRune()
        {
            HasWardingRune = true;
        }

        #endregion

        #region Utility

        internal static void ResetBuffsAndDebuffs()
        {
            TemporaryBasicMana = 0;
            SicknessStacks = 0;
            FastHandStacks = 0;
            HasShieldBeetle = false;
            HasOrbInlayedGauntlets = false;
            HasBubbleWand = false;
            HasWardingRune = false;
        }

        internal static void OnGameReset()
        {
            PlayerHealth = 100;
            MaxPlayerHealth = 100;
            ResetTemporaryBasicMana();
        }

        #endregion

        #endregion
    }
}
