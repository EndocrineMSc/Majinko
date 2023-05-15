using UnityEngine;

namespace Cards
{
    internal class CardZoomEventMovement : MonoBehaviour
    {
        private readonly float _moveDistance = 75f;
        private readonly string _leftOfZoomCard = "Left";
        private readonly string _rightOfZoomCard = "Right";
        private string _leftOrRightOfZoomCard;
        private CardZoomEventHandler _otherCardIsBeingZoomed;

        private void Start()
        {
            _otherCardIsBeingZoomed = Hand.Instance.GetComponent<CardZoomEventHandler>();
            _otherCardIsBeingZoomed.CardZoomIn.AddListener(OnOtherCardZoomIn);
            _otherCardIsBeingZoomed.CardZoomOut.AddListener(OnOtherCardZoomOut);            
        }

        private void OnDisable()
        {
            Hand.Instance.GetComponent<CardZoomEventHandler>().CardZoomIn?.RemoveListener(OnOtherCardZoomIn);
            Hand.Instance.GetComponent<CardZoomEventHandler>().CardZoomOut?.RemoveListener(OnOtherCardZoomOut);
        }

        private void OnOtherCardZoomIn(Vector3 otherCardPosition)
        {            
            DetermineMovementDirection(otherCardPosition);

            if (_leftOrRightOfZoomCard == _leftOfZoomCard)
            {
                MoveLeft();
            }
            else if (_leftOrRightOfZoomCard == _rightOfZoomCard)
            {
                MoveRight();
            }
            else
            {
                //Don't move you're being zoomed
            }
        }
        
        private void OnOtherCardZoomOut(Vector3 otherCardPosition)
        {
            if (_leftOrRightOfZoomCard == _leftOfZoomCard)
            {
                MoveRight();
            }
            else if (_leftOrRightOfZoomCard == _rightOfZoomCard)
            {
                MoveLeft();
            }
            else
            {
                //Don't move you're being zoomed
            }
            _leftOrRightOfZoomCard = "";
        }

        private void DetermineMovementDirection(Vector3 otherCardPosition)
        {
            float deltaXTransform = otherCardPosition.x - transform.position.x;

            if (deltaXTransform > 0f)
            {
                _leftOrRightOfZoomCard = _leftOfZoomCard;
            }
            else if (deltaXTransform == 0)
            {
                //this card is the one being zoomed!
            }
            else
            {
                _leftOrRightOfZoomCard = _rightOfZoomCard;
            }
        }

        private void MoveLeft()
        {
            transform.position = new Vector3(transform.position.x - _moveDistance, transform.position.y, transform.position.z);
        }

        private void MoveRight()
        {
            transform.position = new Vector3(transform.position.x + _moveDistance, transform.position.y, transform.position.z);
        }

    }
}
