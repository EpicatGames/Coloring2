using System;
using Coloring2.DataServices;
using Coloring2.PaintingPage.Palette;
using PaintCraft.Tools;
using PaintCraft.UIControllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Coloring2.PaintingPage.Tools
{
    public class PaintToolsController : MonoBehaviour
    {
        public Action<IPaintTool> ToolSelected;
        
        [SerializeField] private AbstractPaintTool[] _tools;
        [SerializeField] private AbstractPaintTool _defaultSelected;
        [SerializeField] protected LineConfig _lineConfig;

        private PaletteController _palette;

        public IPaintTool Selected { get; private set; }

        private void Awake()
        {
            _palette = GetComponent<PaletteController>();
            Selected = _defaultSelected;
        }

        private void OnDestroy()
        {
            ToolSelected -= OnToolSelected;
            _palette.CellSelected -= OnPaletteCellSelected;
        }

        public void Init()
        {
            ToolSelected += OnToolSelected;
            _palette.CellSelected += OnPaletteCellSelected;

            foreach (var tool in _tools)
                tool.Init(ToolSelected, _palette, _lineConfig);
            
            OnToolSelected(_defaultSelected);
        }

        private void OnPaletteCellSelected(IPaletteCell cell)
        {
            Selected.UpdateState(cell);
        }

        private void OnToolSelected(IPaintTool tool)
        {
            Selected?.Deselect();
            tool.Select();
            Selected = tool;
        }
    }
}