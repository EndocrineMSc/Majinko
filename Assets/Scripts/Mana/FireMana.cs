using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PeggleWars.ManaManagement
{
    internal class FireMana : Mana
    {
        bool _hasRested;

        void Start()
        {
            GetComponent<Rigidbody2D>().velocity = (Vector2.up * 10);
        }

        private void Update()
        {
            if(!_hasRested && GetComponent<Rigidbody2D>().velocity == Vector2.zero)
            {
                _hasRested = true;
                GetComponent<Rigidbody2D>().mass = 5;
            }
        }
    }
}