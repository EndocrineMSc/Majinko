using DG.Tweening;
using UnityEngine;

namespace Cards
{
    internal class CardZoomMovement : MonoBehaviour
    {
        private readonly float _moveDistance = 125f;
        private float _startZoomXPosition;

        private void OnEnable()
        {
            CardEvents.OnCardZoomIn += OnOtherCardZoomIn;
            CardEvents.OnCardZoomOut += OnOtherCardZoomOut;            
        }

        private void OnDisable()
        {
            CardEvents.OnCardZoomIn -= OnOtherCardZoomIn;
            CardEvents.OnCardZoomOut -= OnOtherCardZoomOut;
        }

        private void OnOtherCardZoomIn(Vector3 otherCardPosition)
        {
            float deltaXTransform = otherCardPosition.x - transform.position.x;
            _startZoomXPosition = transform.position.x;

            if (deltaXTransform > 0)
            {
                float targetXPosition = transform.position.x - _moveDistance;
                MoveCard(targetXPosition);
            }
            else if (deltaXTransform < 0)
            {
                float targetXPosition = transform.position.x + _moveDistance;
                MoveCard(targetXPosition);
            }
        }

        private void OnOtherCardZoomOut()
        {
            MoveCard(_startZoomXPosition);
        }

        private void MoveCard(float targetXPosition)
        {
            transform.DOKill();
            transform.DOMoveX(targetXPosition, 0.1f);
        }
    }
}
