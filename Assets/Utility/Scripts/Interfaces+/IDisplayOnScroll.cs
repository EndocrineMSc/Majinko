using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleWars.ScrollDisplay
{
    public interface IDisplayOnScroll
    {
        public void DisplayOnScroll();
        public void StopDisplayOnScroll();

        public string DisplayDescription { get; set; }

        public int DisplayScale { get; set; }
    }
}
