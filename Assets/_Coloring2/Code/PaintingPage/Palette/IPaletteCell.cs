using UnityEngine;

namespace Coloring2.PaintingPage.Palette
{
    public interface IPaletteCell
    {
        Color Color { get;}
        Texture2D Texture { get;}
        Texture2D GlitterTexture { get;}
        void Select();
        void Deselect();
    }
}