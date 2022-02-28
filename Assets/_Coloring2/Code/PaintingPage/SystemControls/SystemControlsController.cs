using System;
using Coloring2;
using Coloring2.DataServices;
using Coloring2.PaintingPage.Palette;
using Coloring2.PaintingPage.Tools;
using PaintCraft.Canvas;
using PaintCraft.Canvas.Configs;
using PaintCraft.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2.PaintingPage.SystemControls
{
    public class SystemControlsController : MonoBehaviour
    {
        [SerializeField] private CallPaletteButton _callPaletteButton;
        [SerializeField] private CanvasController _canvas;
        [SerializeField] private Button _applyButton;
        [SerializeField] private Button _screenshotButton;

        private ScenesSwapScreen _sceneSwapScreen;
        private PaletteController _palette;
        private PaintToolsController _paintTools;

        private void Awake()
        {
            _palette = GetComponent<PaletteController>();
            _paintTools = GetComponent<PaintToolsController>();
            
            _applyButton.onClick.AddListener(OnApplyButton);
            _screenshotButton.onClick.AddListener(OnScreenshotButton);

            if (StartApplication.Initialized)
                _sceneSwapScreen = ServicesManager.GetService<ProjectContextService>().ScenesSwapScreen;
        }

        private void Start()
        {
            _paintTools.ToolSelected += OnToolSelected;
            _palette.CellSelected += OnPaletteCellSelected;
            _callPaletteButton.Tapped += OnCallPaletteTap;
            
            _palette.Init();
            _paintTools.Init();
        }

        private void OnDestroy()
        {
            _callPaletteButton.Tapped -= OnCallPaletteTap;
            _palette.CellSelected -= OnPaletteCellSelected;
            _paintTools.ToolSelected -= OnToolSelected;

            _applyButton.onClick.RemoveListener(OnApplyButton);
            _screenshotButton.onClick.RemoveListener(OnScreenshotButton);
        }

        private void OnToolSelected(IPaintTool tool) => UpdateCallPaletteButton(tool.Type, _palette.SelectedCell);

        private void OnPaletteCellSelected(PaletteCell cell)
        {
            UpdateCallPaletteButton(_paintTools.Selected.Type, cell);
        }

        private void UpdateCallPaletteButton(ToolTypes type, IPaletteCell cell)
        {
            switch (type)
            {
                case ToolTypes.Color: _callPaletteButton.UpdateColor(cell.Color); 
                    break;
                case ToolTypes.Texture: _callPaletteButton.UpdateTexture(cell.Texture); 
                    break;
                case ToolTypes.Glitter: _callPaletteButton.UpdateTexture(cell.GlitterTexture); 
                    break;
                case ToolTypes.Eraser:
                    _callPaletteButton.ResetDisplay();
                    break;
            }
        }

        private void OnScreenshotButton()
        {
            SoundsManager.PlayClick();
            var bytes = _canvas.ExportResultImage().EncodeToPNG();
            //var config = (PageConfigSO)_canvas.PageConfig;
            var fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
            NativeGallery.SaveImageToGallery(bytes, "SavedPictures", fileName);
            SoundsManager.PlayPhoto();
        }

        private void OnApplyButton()
        {
            SoundsManager.PlayClick();
            _sceneSwapScreen.FadeIn(() => ScenesManager.LoadScene(ScenesManager.Scenes.SelectPageScene));
        }

        private void OnCallPaletteTap(bool value)
        {
            SoundsManager.PlayClick();
            if(value)
                _palette.Show();
            else
                _palette.Hide();
        }
    }
}