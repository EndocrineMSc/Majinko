using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace Utility
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

        private void OnEnable()
        {
            UtilityEvents.OnDisplayOnScrollTrigger += DisplayOnScroll;
            UtilityEvents.OnStopScrollDisplayTrigger += StopDisplaying;
        }

        private void OnDisable()
        {
            UtilityEvents.OnDisplayOnScrollTrigger -= DisplayOnScroll;
            UtilityEvents.OnStopScrollDisplayTrigger -= StopDisplaying;
        }

        private void Start()
        {
            _scrollDisplay = transform.GetChild(0).gameObject;
            _scrollDescriptionBox = transform.GetComponentInChildren<TextMeshProUGUI>();
            _scrollRenderer = _scrollDisplay.GetComponent<SpriteRenderer>();
            _scrollAnimator = _scrollDisplay.GetComponent<Animator>();
            _scrollRenderer.enabled = false;
            _scrollDescriptionBox.enabled = false;
        }

        private void DisplayOnScroll(GameObject displayObject)
        {
            Animator animator = CheckIfObjectHasAnimator(displayObject);
            SpriteRenderer spriteRenderer = CheckIfObjectHasSpriteRenderer(displayObject);
            Image image = CheckIfObjectHasImage(displayObject);
            IDisplayOnScroll display = GetScrollDisplayer(displayObject);

            _scrollRenderer.enabled = true;

            var displayText = display.DisplayDescription;
            var displayScale = display.DisplayScale;

            if (animator != null)
            {
                _scrollAnimator.enabled = true;
                _scrollAnimator.runtimeAnimatorController = animator.runtimeAnimatorController;
            }
            else if (spriteRenderer != null)
            {
                _scrollRenderer.sprite = spriteRenderer.sprite;
            }
            else
            {
                _scrollRenderer.sprite = image.sprite;
            }

            _scrollDisplay.transform.localScale = displayObject.transform.localScale * displayScale;
            _scrollDescriptionBox.enabled = true;
            _scrollDescriptionBox.text = displayText;
        }

        private Animator CheckIfObjectHasAnimator(GameObject displayObject)
        {
            Animator gameobjectAnimator = displayObject.GetComponent<Animator>();

            if(gameobjectAnimator == null) 
                gameobjectAnimator = displayObject.GetComponentInChildren<Animator>();

            return gameobjectAnimator;
        }

        private SpriteRenderer CheckIfObjectHasSpriteRenderer(GameObject displayObject)
        {
            SpriteRenderer spriteRenderer = displayObject.GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
                spriteRenderer = displayObject.GetComponentInChildren<SpriteRenderer>();

            return spriteRenderer;
        }

        private Image CheckIfObjectHasImage(GameObject displayObject)
        {
            Image image = displayObject.GetComponent<Image>();

            if (image == null)
                image = displayObject.GetComponentInChildren<Image>();

            return image;
        }

        private IDisplayOnScroll GetScrollDisplayer(GameObject displayObject)
        {
            IDisplayOnScroll displayer = displayObject.GetComponent<IDisplayOnScroll>();

            if (displayer == null)
                displayer = displayObject.GetComponentInChildren<IDisplayOnScroll>();

            return displayer;
        }

        private void StopDisplaying()
        {
            _scrollRenderer.GetComponent<SpriteRenderer>().enabled = false;
            _scrollDescriptionBox.enabled = false;
            _scrollAnimator.enabled = false;
        }
        #endregion

    }
}
