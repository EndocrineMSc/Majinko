using PeggleWars.ScrollDisplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PeggleWars.ScrollDisplay
{
    internal class ScrollDisplayer : MonoBehaviour, IDisplayOnScroll
    {
        private void OnMouseEnter()
        {
            DisplayOnScroll();                     
        }

        private void OnMouseExit()
        {
            StopDisplayOnScroll();                    
        }

        public void DisplayOnScroll()
        {
            ScrollEvents.Instance.ScrollDisplayEvent?.Invoke(gameObject);
        }

        public void StopDisplayOnScroll()
        {
            ScrollEvents.Instance.StopDisplayingEvent?.Invoke();
        }

    }
}
