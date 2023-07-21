using DG.Tweening;
using UnityEngine;

namespace Cards
{
    internal class CardZoomMovement : MonoBehaviour
    {
        private readonly float _moveDistance = 85f;

        private void OnEnable()
        {
            CardEvents.OnCardZoomIn += OnCardZoomIn;         
        }

        private void OnDisable()
        {
            CardEvents.OnCardZoomIn -= OnCardZoomIn;
        }

        private void OnCardZoomIn(Vector3 otherCardPosition)
        {
            var deltaXTransform = otherCardPosition.x - transform.position.x;

            if (deltaXTransform > 0)
                transform.DOBlendableMoveBy(new(-_moveDistance, 0, 0), 0.1f);
            else if (deltaXTransform < 0)
                transform.DOBlendableMoveBy(new(_moveDistance, 0, 0), 0.1f);
        }
    }
}
