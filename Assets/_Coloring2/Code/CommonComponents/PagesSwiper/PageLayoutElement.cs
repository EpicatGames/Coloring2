using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Coloring2.CommonComponents.PagesSwiper
{
    [ExecuteInEditMode]
    public class PageLayoutElement : MonoBehaviour
    {
        public Vector2 Size { get; private set; }
        
        private void Start()
        {
            var rootRectTransform = GetComponentInParent<Canvas>()?.GetComponent<RectTransform>();
            if (rootRectTransform == null)
                throw new Exception("missing canvas component in root");

            var rect = GetComponent<RectTransform>();
            Size = new Vector2(rootRectTransform.sizeDelta.x, rootRectTransform.sizeDelta.y);
            rect.sizeDelta = Size;
        }
    }
}