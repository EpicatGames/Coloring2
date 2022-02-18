using System;
using Coloring2.MainMenu.Categories;

namespace Coloring2.DataServices
{
    public class PlayerInteractionActionsService : AbstractService
    {
        public Action<MenuCategory> MenuCategoryTapped;
        public Action SettingsButtonTapped;
    }
}