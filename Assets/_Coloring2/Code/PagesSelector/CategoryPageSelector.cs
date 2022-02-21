using Coloring2.CommonComponents.PagesSwiper;
using Coloring2.Configs;
using UnityEngine;

namespace Coloring2.PagesSelector
{
    [RequireComponent(typeof(IPage))]
    public class CategoryPageSelector : MonoBehaviour
    {
        [SerializeField] private CategoryConfig _config;
        public CategoryConfig Config => _config;
    }
}