using System;
using Coloring2.Configs;
using Coloring2.MainMenu.Categories;
using PaintCraft.Canvas.Configs;

namespace Coloring2.DataServices
{
    public class PlayerInteractionActionsService : AbstractService
    {
        public Action<MenuCategory> MenuCategoryTapped;
        public Action SettingsButtonTapped;
        public Action<PageConfigSO> PagePreviewSelected;
        public Action CanvasTouchBegan;
        public Action CanvasTouchEnd;
    }
}