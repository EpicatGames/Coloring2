using System;
using Coloring2.Configs;
using Coloring2.DataServices;
using PaintCraft.Canvas.Configs;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2.PagesSelector
{
    [RequireComponent(typeof(Button))]
    public class PagePreviewItem : MonoBehaviour
    {
        [SerializeField] private PageConfigSO _config;
        [SerializeField] private RawImage _display;

        private PlayerInteractionActionsService _playerInteractionsService;
        private Button _button;

        private void Awake()
        {
            _playerInteractionsService = ServicesManager.GetService<PlayerInteractionActionsService>();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnTap);
            
            if (_config.HasUserOrDefaultIcon(out var icon))
                _display.texture = icon;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnTap);
        }

        private void OnTap()
        {
            SoundsManager.PlayClick();
            _playerInteractionsService.PagePreviewSelected?.Invoke(_config);
        }
    }
}