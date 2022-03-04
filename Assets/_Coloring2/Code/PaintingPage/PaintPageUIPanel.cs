using System;
using Coloring2.PaintingPage.Palette;
using DG.Tweening;
using UnityEngine;

namespace Coloring2.PaintingPage
{
    public class PaintPageUIPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        
        private PaletteController _palette;
        private RectTransform _rectTransform;
        private float _defaultXPos;
        private float _hideXPos;

        private void Start()
        {
            _palette = GetComponent<PaletteController>();
            _rectTransform = GetComponent<RectTransform>();
            _defaultXPos = _rectTransform.sizeDelta.x;
            _hideXPos = _rect.rect.width * 2.5f;
        }

        public bool Opened { get; private set; } = true;

        public void Show()
        {
            _rectTransform.DOAnchorPosX(_defaultXPos, .4f);
            Opened = true;
        }

        public void Hide()
        {
            _rectTransform.DOAnchorPosX(_hideXPos, .4f);
            _palette.Hide();
            Opened = false;
        }
    }
}