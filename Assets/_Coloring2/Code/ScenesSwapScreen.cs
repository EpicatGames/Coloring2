using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2
{
    [RequireComponent(typeof(CanvasGroup), typeof(Canvas), typeof(GraphicRaycaster))]
    [RequireComponent(typeof(CanvasScaler))]
    public class ScenesSwapScreen : MonoBehaviour
    {
        public event Action FadeInComplete;
        public event Action FadeOutComplete;
        
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Show() => _canvasGroup.alpha = 1;

        public void Hide() => _canvasGroup.alpha = 0;
        
        public void FadeIn()
        {
            _canvasGroup.DOFade(1, 0.6f).OnComplete(() => FadeInComplete?.Invoke());
        }
        
        public void FadeOut()
        {
            _canvasGroup.DOFade(0, 0.6f).OnComplete(() => FadeOutComplete?.Invoke());
        }
    }
}