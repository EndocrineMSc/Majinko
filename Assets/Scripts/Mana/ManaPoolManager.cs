using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleMana;
using EnumCollection;

namespace PeggleMana
{

    public class ManaPoolManager : MonoBehaviour
    {
        #region

        public List<Mana> BasicMana = new();
        public List<Mana> FireMana = new();
        public List<Mana> IceMana = new();
        public List<Mana> LightningMana = new();
        public List<Mana> DarkMana = new();
        public List<Mana> LightMana = new();

        public static ManaPoolManager Instance { get; private set; }

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

        #endregion

        #region Private Functions

        private void SpendManaByList(List<Mana> list, int amount)
        {
            if (list.Count < amount)
            {
                Debug.Log("Not enough Mana");
                return;
            }

            for (int i = 0; i < amount; i++)
            { 
                Destroy(list[0].gameObject);
                list.RemoveAt(0);
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
