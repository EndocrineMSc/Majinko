using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PeggleWars.ScrollDisplay
{
    internal class Scroll : MonoBehaviour
    {
        #region Fields and Properties

        private Animator _scrollAnimator;
        private GameObject _scrollDisplay;
        private SpriteRenderer _scrollRenderer;
        private TextMeshProUGUI _scrollDescriptionBox;

        #endregion

        #region Functions

        private void Start()
        {
            _scrollDisplay = transform.GetChild(0).gameObject;
            _scrollDescriptionBox = transform.GetComponentInChildren<TextMeshProUGUI>();
            _scrollRenderer = _scrollDisplay.GetComponent<SpriteRenderer>();
            _scrollAnimator = _scrollDisplay.GetComponent<Animator>();
            _scrollRenderer.enabled = false;
            _scrollDescriptionBox.enabled = false;

            ScrollEvents.Instance.ScrollDisplayEvent?.AddListener(DisplayOnScroll);
            ScrollEvents.Instance.StopDisplayingEvent?.AddListener(StopDisplaying);
        }

        private void DisplayOnScroll(GameObject displayObject)
        {
            _scrollRenderer.enabled = true;
            Animator gameobjectAnimator = displayObject.GetComponent<Animator>();
            if (gameobjectAnimator == null)
            {
                gameobjectAnimator = displayObject.GetComponentInChildren<Animator>();
            }

            string displayText = displayObject.GetComponentInChildren<IDisplayOnScroll>()?.DisplayDescription;
            int scrollDisplayScale = displayObject.GetComponentInChildren<IDisplayOnScroll>().DisplayScale;

            if(gameobjectAnimator != null)
            {
                _scrollAnimator.enabled = true;
                _scrollAnimator.runtimeAnimatorController = gameobjectAnimator.runtimeAnimatorController;
            }
            else
            {
                _scrollAnimator.enabled = false;
                if(displayObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
                {
                    _scrollRenderer.sprite = spriteRenderer.sprite;
                }
                else
                {
                    GameObject displayObjectChild = displayObject.transform.GetChild(0).gameObject;
                    if(displayObjectChild.TryGetComponent<SpriteRenderer>(out SpriteRenderer childSpriteRenderer))
                    {
                        _scrollRenderer.sprite = childSpriteRenderer.sprite;
                    }
                }
                
            }

            _scrollDisplay.transform.localScale = displayObject.transform.localScale * scrollDisplayScale;
            _scrollDescriptionBox.enabled = true;
            _scrollDescriptionBox.text = displayText;
        }

        private void StopDisplaying()
        {
            _scrollRenderer.GetComponent<SpriteRenderer>().enabled = false;
            _scrollDescriptionBox.enabled = false;
        }



        #endregion

    }
}
