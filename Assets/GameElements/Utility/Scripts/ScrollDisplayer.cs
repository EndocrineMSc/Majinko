using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utility
{
    internal class ScrollDisplayer : MonoBehaviour, IDisplayOnScroll, IPointerEnterHandler, IPointerExitHandler
    {
        public string DisplayDescription { get; set; } = "Not implemented";
        public int DisplayScale { get; set; }

        private void OnMouseEnter()
        {
            DisplayOnScroll();                     
        }

        private void OnMouseExit()
        {
            StopDisplayOnScroll();                    
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            DisplayOnScroll();
        }

        public void OnPointerExit(PointerEventData eventData)
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
            UtilityEvents.RaiseStopScrollDisplay();
        }

        private IEnumerator PolishDisplayTimer()
        {
            yield return new WaitForSeconds(0.2f);
            UtilityEvents.RaiseDisplayOnScroll(gameObject);
        }
    }
}
