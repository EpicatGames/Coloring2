using Coloring2.PaintingPage.Palette;
using PaintCraft.Tools;

namespace Coloring2.PaintingPage.Tools
{
    public interface IPaintTool
    {
        void Select();
        void Deselect();
        ToolTypes Type { get; }
        Brush Brush { get; }
        void UpdateState(IPaletteCell cell);
    }
}