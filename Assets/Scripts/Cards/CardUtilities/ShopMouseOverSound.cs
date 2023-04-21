using PeggleWars.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PeggleWars.Cards
{
    internal class ShopMouseOverSound : MonoBehaviour, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(EnumCollection.SFX._0002_MouseOverCard);
        }
    }
}
