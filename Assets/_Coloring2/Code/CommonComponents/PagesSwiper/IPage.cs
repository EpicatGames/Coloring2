using UnityEngine;

namespace Coloring2.CommonComponents.PagesSwiper
{
    public interface IPage
    {
        RectTransform RectTransform { get; }
        GameObject gameObject { get; }
        int Id { get; }
        void Init(int id);
        Vector2 GetSize();
    }
}