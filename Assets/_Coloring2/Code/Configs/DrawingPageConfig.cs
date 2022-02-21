using UnityEngine;

namespace Coloring2.Configs
{
    [CreateAssetMenu(fileName = "DrawingPageConfig", menuName = "Coloring2/DrawingPageConfig", order = 0)]
    public class DrawingPageConfig : ScriptableObject
    {
        public Sprite MiniaturImage;
        public Sprite RegionsImage;
        public Sprite ColorizedImage;
    }
}