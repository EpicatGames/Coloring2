using System;
using Coloring2.DataServices;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2.MainMenu.Settings
{
    [RequireComponent(typeof(Button))]
    public class SettingsButton : MonoBehaviour
    {
        private PlayerInteractionActionsService _playerInteractionService;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnDestroy() => _button.onClick.RemoveListener(OnTap);

        private void Start()
        {
            _playerInteractionService = ServicesManager.GetService<PlayerInteractionActionsService>();
            _button.onClick.AddListener(OnTap);
        }
        
        private void OnTap()
        {
            _playerInteractionService.SettingsButtonTapped?.Invoke();
        }
    }
}