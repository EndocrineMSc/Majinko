using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(menuName = "Cards/AllCardsCollection")]
    internal class AllCardsCollection : ScriptableObject
    {
        [SerializeField] internal Card[] AllCards;
    }
}
