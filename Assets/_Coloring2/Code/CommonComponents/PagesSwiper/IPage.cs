using UnityEngine;

namespace Coloring2.CommonComponents.PagesSwiper
{
    public interface IPage
    {
        Transform transform { get; }
        int Id { get; }
        void Init(int id);
        Vector2 Size { get; }
    }
}