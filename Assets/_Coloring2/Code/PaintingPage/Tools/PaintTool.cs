using System;
using Coloring2.PaintingPage.Palette;
using PaintCraft.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2.PaintingPage.Tools
{
    public class PaintTool : AbstractPaintTool
    {
        [Space(10)]
        [SerializeField] private RawImage[] _texturableImages;
        [SerializeField] private Image[] _colorableImages;

        protected override void Awake()
        {
            base.Awake();
            _button.onClick.AddListener(OnTap);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _button.onClick.RemoveListener(OnTap);
        }

        public override void UpdateState(IPaletteCell cell)
        {
            switch (Type)
            {
                case ToolTypes.Color: ApplyColor(cell.Color);
                    break;
                case ToolTypes.Texture: ApplyTexture(cell.Texture);
                    break;
                case ToolTypes.Glitter: ApplyTexture(cell.GlitterTexture);
                    break;
                case ToolTypes.Eraser:
                    break;
            }
        }

        public override void Select()
        {
            base.Select();
            UpdateState(_palette.SelectedCell);
        }

        private void ApplyColor(Color color)
        {
            foreach (var img in _colorableImages)
                img.color = color;
            
            var pointColor = PointColor.White;
            pointColor.Color = color;
            _lineConfig.color = pointColor;
        }

        private void ApplyTexture(Texture2D texture)
        {
            foreach (var img in _texturableImages)
            {
                img.texture = texture;
                img.color = Color.white;
            }
            
            _lineConfig.color = PointColor.White;
            _lineConfig.texture = texture;
        }
    }
}