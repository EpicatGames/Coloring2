using System;
using Coloring2.CommonComponents.PagesSwiper;
using Coloring2.Configs;
using UnityEngine;

namespace Coloring2.CommonComponents.PagesSwiper
{
    [RequireComponent(typeof(PageLayoutElement))]
    public class Page : MonoBehaviour, IPage
    {
        private PageLayoutElement _layoutElement;
        private RectTransform _rectTransform;
        public Vector2 Size => _layoutElement.Size;

        private void Awake()
        {
            _layoutElement = GetComponent<PageLayoutElement>();
        }

        public int Id { get; private set; }

        public void Init(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"[{gameObject.name}, id: {Id}], Size: {Size}";
        }
    }
}