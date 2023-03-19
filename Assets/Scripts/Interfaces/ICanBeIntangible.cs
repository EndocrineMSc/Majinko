using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleWars.Characters
{
    public interface ICanBeIntangible
    {
        int IntangibleStacks { get; }
        void SetIntangible(int intangibleStacks = 1);
        void RemoveIntangible();
        void HandleIntangibleStacks();
    }
}
