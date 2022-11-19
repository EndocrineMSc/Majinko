using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

public class LiquidBlood : MonoBehaviour
{
    void Start()
    {
        //Ignore the collisions between layer 0 (default) and layer 8 (custom layer you set in Inspector window)
        Physics.IgnoreLayerCollision(6, 7);
    }
}