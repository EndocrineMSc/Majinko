using PeggleWars.ScrollDisplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PeggleWars.ScrollDisplay
{
    internal class ScrollDisplayer : MonoBehaviour, IDisplayOnScroll
    {
        public string DisplayDescription { get; set; } = "Not implemented";

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
            StartCoroutine(PolishDisplayTimer());
        }

        public void StopDisplayOnScroll()
        {
            StopAllCoroutines();
            ScrollEvents.Instance.StopDisplayingEvent?.Invoke();
        }

        private IEnumerator PolishDisplayTimer()
        {
            yield return new WaitForSeconds(0.2f);
            ScrollEvents.Instance.ScrollDisplayEvent?.Invoke(gameObject);
        }
    }
}
