using Coloring2.Localization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Coloring2.Configs
{
    public enum Categories
    {
        category_animals,
        category_princesses,
        category_underwater,
        category_transport,
        category_dinosaurs,
        category_picnic,
        category_garden,
        category_aliens,
        category_houses,
        full_version,
        colors
    }
    
    [CreateAssetMenu(fileName = "CategoryConfig", menuName = "Coloring2/CategoryConfig", order = 0)]
    public class CategoryConfig : ScriptableObject, ISelectable
    {
        public Categories Category;
        public bool PurchasedByDefault;

        [Space(5)]
        public Color Color;
        public Sprite PageBground;
        public Material ParticlesMaterial;
    }
}