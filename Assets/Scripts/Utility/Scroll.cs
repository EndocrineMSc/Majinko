using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleWars.ScrollDisplay
{
    internal class Scroll : MonoBehaviour
    {
        #region Fields and Properties

        private Animator _scrollAnimator;
        private GameObject _scrollDisplay;
        private SpriteRenderer _scrollRenderer;

        #endregion

        #region Functions

        private void Start()
        {
            _scrollDisplay = transform.GetChild(0).gameObject;            
            _scrollRenderer = _scrollDisplay.GetComponent<SpriteRenderer>();
            _scrollAnimator = _scrollDisplay.GetComponent<Animator>();
            _scrollRenderer.enabled = false;

            ScrollEvents.Instance.ScrollDisplayEvent?.AddListener(DisplayOnScroll);
            ScrollEvents.Instance.StopDisplayingEvent?.AddListener(StopDisplaying);
        }

        private void DisplayOnScroll(GameObject displayObject)
        {
            Debug.Log("I'm in the function");
            _scrollRenderer.enabled = true;

            Animator gameobjectAnimator = displayObject.GetComponent<Animator>();

            if(gameobjectAnimator != null)
            {
                _scrollAnimator.enabled = true;
                _scrollAnimator.runtimeAnimatorController = gameobjectAnimator.runtimeAnimatorController;
            }
            else
            {
                _scrollAnimator.enabled = false;
                _scrollRenderer.sprite = displayObject.GetComponent<SpriteRenderer>().sprite;
            }

            _scrollDisplay.transform.localScale = displayObject.transform.localScale * 4;
        }

        private void StopDisplaying()
        {
            _scrollRenderer.GetComponent<SpriteRenderer>().enabled = false;
        }



        #endregion

    }
}
