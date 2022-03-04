using System;
using DG.Tweening;
using PaintCraft.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2
{
    [RequireComponent(typeof(CanvasGroup), typeof(Canvas), typeof(GraphicRaycaster))]
    [RequireComponent(typeof(CanvasScaler))]
    public class ScenesSwapScreen : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Show() => _canvasGroup.alpha = 1;

        public void Hide() => _canvasGroup.alpha = 0;
        
        public void FadeIn(Action complete = null)
        {
            _canvasGroup.DOFade(1, 0.4f).OnComplete(() => complete?.Invoke());
        }
        
        public void FadeOut(Action complete = null)
        {
            _canvasGroup.DOFade(0, 0.4f).OnComplete(() => complete?.Invoke());
        }
    }
}