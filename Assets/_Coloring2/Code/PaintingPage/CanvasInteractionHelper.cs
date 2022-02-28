using System;
using Coloring2.DataServices;
using PaintCraft.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Coloring2.PaintingPage
{
    public class CanvasInteractionHelper : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private PlayerInteractionActionsService _playerInteraction;

        private void Awake()
        {
            if (StartApplication.Initialized == false)
                return;
            
            _playerInteraction =ServicesManager.GetService<PlayerInteractionActionsService>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _playerInteraction.CanvasTouchBegan?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _playerInteraction.CanvasTouchEnd?.Invoke();
        }
    }
}