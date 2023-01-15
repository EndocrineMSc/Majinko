using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PeggleWars.ManaManagement
{
    /// <summary>
    /// This class handles the the usage of mana. Stores available mana in lists.
    /// </summary>
    public class ManaPool : MonoBehaviour
    {
        #region

        public List<Mana> BasicMana = new();
        public List<Mana> FireMana = new();
        public List<Mana> IceMana = new();
        public List<Mana> LightningMana = new();
        public List<Mana> DarkMana = new();
        public List<Mana> LightMana = new();

        public static ManaPool Instance { get; private set; }

        #endregion

        #region Public Functions

        public void SpendMana(ManaType manaType, int amount)
        {
            switch (manaType)
            {
                case ManaType.BaseMana:
                    SpendManaByList(BasicMana, amount);
                    break;

                case ManaType.FireMana:
                    SpendManaByList(FireMana, amount);
                    break;

                case ManaType.IceMana:
                    SpendManaByList(IceMana, amount);
                    break;

                case ManaType.LightningMana:
                    SpendManaByList(LightningMana, amount);
                    break;

                case ManaType.DarkMana:
                    SpendManaByList(DarkMana, amount);
                    break;

                case ManaType.LightMana:
                    SpendManaByList(LightMana, amount);
                    break;
            }
        }

        public bool CheckForManaAmount(ManaType manaType, int amount)
        {
            bool enoughMana = false;

            switch (manaType)
            {
                case ManaType.BaseMana:
                    enoughMana = CheckIfEnoughManaByList(BasicMana, amount);
                    break;

                case ManaType.FireMana:
                    enoughMana = CheckIfEnoughManaByList(FireMana, amount);
                    break;

                case ManaType.IceMana:
                    enoughMana = CheckIfEnoughManaByList(IceMana, amount);
                    break;

                case ManaType.LightningMana:
                    enoughMana = CheckIfEnoughManaByList(LightningMana, amount);
                    break;

                case ManaType.DarkMana:
                    enoughMana = CheckIfEnoughManaByList(DarkMana, amount);
                    break;

                case ManaType.LightMana:
                    enoughMana = CheckIfEnoughManaByList(LightMana, amount);
                    break;
            }
            return enoughMana;
        }

        #endregion

        #region Private Functions

        private void SpendManaByList(List<Mana> manaList, int amount)
        {
            if (manaList.Count < amount)
            {
                Debug.Log("Not enough Mana");
                return;
            }

            for (int i = 0; i < amount; i++)
            { 
                Destroy(manaList[0].gameObject);
                manaList.RemoveAt(0);
            }   
        }

        private bool CheckIfEnoughManaByList(List<Mana> manaList, int amount)
        {
            if (manaList.Count > amount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        #endregion
    }
}



