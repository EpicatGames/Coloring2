using System;
using Coloring2.DataServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Coloring2.MainMenu.Categories
{
    public class CategoryInteractor : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private PlayerInteractionActionsService _playerActionsService;
        private MenuCategory _owner;

        public void Init(MenuCategory owner)
        {
            _owner = owner;
            _playerActionsService = ServicesManager.GetService<PlayerInteractionActionsService>();
        }

        public void OnPointerDown(PointerEventData eventData) { }

        public void OnPointerUp(PointerEventData eventData)
        {
            var delta = (eventData.pressPosition - eventData.position).magnitude;
            //Debug.Log($"OnPointerUp: {delta}");
            if(delta > 5)
                return;
            
            _playerActionsService.MenuCategoryTapped?.Invoke(_owner);
        }
    }
}