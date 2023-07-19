using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Relics;
using System;
using System.Linq;
using UnityEngine.UI;

namespace Utility
{
    internal class AddRandomRelicButton : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(AddRandomRelic);
        }

        private void AddRandomRelic()
        {
            List<Relic> allRelicEnums = Enum.GetValues(typeof(Relic)).Cast<Relic>().ToList();
            int randomIndex = UnityEngine.Random.Range(0, allRelicEnums.Count);
            Relic relic = allRelicEnums[randomIndex];
            RelicManager.Instance.AddRelic(relic);
            RelicManager.Instance.InstantiateRelics();
        }
    }
}
