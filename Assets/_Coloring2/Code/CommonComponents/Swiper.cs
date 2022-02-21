using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Coloring2.CommonComponents
{
    public class Swiper : ScrollRect
    {
        public const float MinDeltaValueForSwipe = 70;

        public struct SwiperEventData
        {
            public Directions Direction;
            public float Delta;
            public Vector2 PositionOnBeginSwipe;

            public override string ToString()
            {
                return $"[Direction: {Direction}, Delta: {Delta}, PressPoint: {PositionOnBeginSwipe}]";
            }
        }
        
        public enum Directions
        {
            Left,
            Right
        }
        
        public Action<SwiperEventData> Swipe;

        private Vector2 _beginDragContentPosition;
        
        public void SetContentPosition(Vector2 pos)
        {
            content.anchoredPosition = pos;
        }
        
        public void AnimateContentHorizontalTo(float x, float time)
        {
            content.DOAnchorPosX(x, time);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            _beginDragContentPosition = ((RectTransform) content.transform).anchoredPosition;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            var delta = eventData.pressPosition - eventData.position;
            var dir = delta.x > 0 ? Directions.Left : Directions.Right;
            var data = new SwiperEventData
            {
                Direction = dir,
                Delta = Math.Abs(delta.x),
                PositionOnBeginSwipe = _beginDragContentPosition
            };
            Swipe?.Invoke(data);
        }
    }
}