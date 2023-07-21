using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

namespace Overworld
{
    internal class RandomEventElement : OverworldElement
    {
        private SceneName _sceneToLoad;

        protected override void Start()
        {
            base.Start();
            var eventScenes = new List<SceneName>();
            var enumList = Enum.GetValues(typeof(SceneName)).Cast<SceneName>().ToList();
            
            eventScenes.AddRange(from SceneName scene in enumList
                                  where scene.ToString().Contains("Event")
                                  select scene);

            int randomIndex = UnityEngine.Random.Range(0, eventScenes.Count);
            _sceneToLoad = eventScenes[randomIndex];
        }

        protected override void LoadScene()
        {
            LoadHelper.LoadSceneWithLoadingScreen(_sceneToLoad);
        }
    }
}
