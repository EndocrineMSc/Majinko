using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PeggleWars.ManaManagement
{
    internal class FireMana : Mana
    {
        void Start()
        {
            GetComponent<Rigidbody2D>().velocity = (Vector2.up * 5);
        }
    }
}