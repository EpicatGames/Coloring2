using System;
using UnityEngine;

namespace Coloring2.CommonComponents.PagesSwiper
{
    public class Page : MonoBehaviour, IPage
    {
        public RectTransform RectTransform => GetComponent<RectTransform>();

        public int Id { get; private set; }

        public void Init(int id)
        {
            Id = id;
        }

        public Vector2 GetSize()
        {
            var rect = RectTransform.rect;
            return new Vector2(rect.width, rect.height);
        }

        public override string ToString()
        {
            return $"[{gameObject.name}, id: {Id}], Size: {GetSize()}, pos: {RectTransform.anchoredPosition}";
        }
    }
}