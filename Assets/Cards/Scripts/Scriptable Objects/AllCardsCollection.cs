using UnityEngine;

namespace Cards
{
    [CreateAssetMenu]
    internal class AllCardsCollection : ScriptableObject
    {
        [SerializeField] internal Card[] AllCards;
    }
}
