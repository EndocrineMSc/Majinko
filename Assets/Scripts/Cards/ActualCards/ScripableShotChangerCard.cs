using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards.ScriptableCards
{
    [CreateAssetMenu]
    public class ScripableShotChangerCard : ScriptableCard
    {
        [SerializeField] private ShotType _spawnShot;
        public ShotType ShotToBeSpawned { get { return _spawnShot; } }
    }
}
