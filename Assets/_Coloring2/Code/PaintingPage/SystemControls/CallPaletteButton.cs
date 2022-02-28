using System;
using Coloring2.DataServices;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2.PaintingPage.SystemControls
{
    [RequireComponent(typeof(Button))]
    public class CallPaletteButton : MonoBehaviour
    {
        public event Action<bool> Tapped;
        public Action PaletteOutsideClosed;

        [SerializeField] private RawImage _display;
        [SerializeField] private Image _handIcon;

        private bool _isOn;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnTap);
            PaletteOutsideClosed += OnPaletteOutsideClosed;
        }
        
        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnTap);
            PaletteOutsideClosed -= OnPaletteOutsideClosed;
        }

        private void OnPaletteOutsideClosed() => _isOn = false;

        public void UpdateColor(Color color)
        {
            if (!_display.enabled)
                _display.enabled = _handIcon.enabled = true;
            _display.color = color;
            _display.texture = null;
        }

        public void UpdateTexture(Texture2D tex)
        {
            if (!_display.enabled)
                _display.enabled = _handIcon.enabled = true;

            _display.color = Color.white;
            _display.texture = tex;
        }
        
        public void ResetDisplay()
        {
            _display.enabled = _handIcon.enabled = false;
        }

        private void OnTap()
        {
            SoundsManager.PlayClick();
            _isOn = !_isOn;
            Tapped?.Invoke(_isOn);
        }
    }
}