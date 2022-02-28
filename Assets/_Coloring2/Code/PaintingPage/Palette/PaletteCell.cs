using System;
using Coloring2.PaintingPage.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2.PaintingPage.Palette
{
    [RequireComponent(typeof(Button))]
    public class PaletteCell : MonoBehaviour, IPaletteCell
    {
        [SerializeField]private Texture2D _texture;
        [SerializeField]private Texture2D _glitterTexture;

        [Space(10)]
        [SerializeField]private RawImage _display;

        [Space(10)]
        [SerializeField]private Image _lock;
        [SerializeField]private Image _selectIcon;

        public ToolTypes CurrentState { get; private set; }
        public Color Color { get; private set; }
        public Texture2D Texture => _texture;
        public Texture2D GlitterTexture => _glitterTexture;

        private bool _selected;
        
        public bool IsLocked;

        private Action<PaletteCell> _tapAction;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnTap);
            Color = _display.color;
        }

        public void Select()
        {
            _selectIcon.enabled = _selected = true;
        }
        
        public void Deselect()
        {
            _selectIcon.enabled = _selected = false;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnTap);
        }

        public void Init(Action<PaletteCell> tapAction, bool locked)
        {
            _tapAction = tapAction;
            IsLocked = locked;
            _lock.enabled = IsLocked;
        }
        
        public void UpdateState(ToolTypes toolType)
        {
            switch (toolType)
            {
                case ToolTypes.Color: UpdateInternal(toolType, Color, null);
                    break;
                case ToolTypes.Texture: UpdateInternal(toolType, Color.white, _texture);
                    break;
                case ToolTypes.Glitter: UpdateInternal(toolType, Color.white, _glitterTexture);
                    break;
                case ToolTypes.Eraser:
                    _display.enabled = false;
                    if (_selected) _selectIcon.enabled = false;
                        break;
            }
        }

        private void UpdateInternal(ToolTypes toolType, Color color, Texture texture)
        {
            if (_selected) _selectIcon.enabled = true;
            _display.enabled = true;
            _display.color = color;
            _display.texture = texture;
            CurrentState = toolType;
        }
        
        private void OnTap()
        {
            _tapAction?.Invoke(this);
        }
    }
}