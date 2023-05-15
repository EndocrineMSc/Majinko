using Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    internal class ShopMouseOverSound : MonoBehaviour, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0002_MouseOverCard);
        }
    }
}
