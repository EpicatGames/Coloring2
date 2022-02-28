using System;
using System.Collections;
using Coloring2.DataServices;
using Coloring2.PagesSelector;
using Coloring2.PaintingPage.SystemControls;
using Coloring2.PaintingPage.Tools;
using PaintCraft.Canvas;
using UnityEngine;

namespace Coloring2.PaintingPage
{
    public class PaintingPageController : MonoBehaviour
    {
        [SerializeField] private PaintPageUIPanel _uiPanel;
        
        private ScenesSwapScreen _scenesSwapScreen;
        private PlayerInteractionActionsService _playerInteraction;
        private Coroutine _timeout;

        private void Awake()
        {
            if (StartApplication.Initialized == false)
            {
                ScenesManager.LoadScene(ScenesManager.Scenes.StartAppScene);
                return;
            }

            _scenesSwapScreen = ServicesManager.GetService<ProjectContextService>().ScenesSwapScreen;
            _playerInteraction = ServicesManager.GetService<PlayerInteractionActionsService>();
            
            _playerInteraction.CanvasTouchBegan += OnCanvasTouchBegan;
            _playerInteraction.CanvasTouchEnd += OnCanvasTouchEnd;
        }

        private void OnDestroy()
        {
            if(_timeout != null)
                StopCoroutine(_timeout);
            
            if(StartApplication.Initialized == false)
                return;
            _playerInteraction.CanvasTouchBegan -= OnCanvasTouchBegan;
            _playerInteraction.CanvasTouchEnd -= OnCanvasTouchEnd;
        }

        private void Start()
        {
            _scenesSwapScreen.FadeOut();
        }
        
        private void OnCanvasTouchBegan()
        {
            if(_timeout != null)
                StopCoroutine(_timeout);
            
            if(_uiPanel.Opened) _uiPanel.Hide();
        }

        private void OnCanvasTouchEnd()
        {
            if(_uiPanel.Opened) 
                return;
            
            _timeout = StartCoroutine(DoTimeout());
        }
        
        private IEnumerator DoTimeout()
        {
            yield return new WaitForSeconds(1);
            _uiPanel.Show();
        }
    }
}