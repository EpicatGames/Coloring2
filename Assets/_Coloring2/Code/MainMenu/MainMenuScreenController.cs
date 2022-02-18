using System;
using System.Collections.Generic;
using Coloring2.Configs;
using Coloring2.DataServices;
using Coloring2.MainMenu.Categories;
using Coloring2.MainMenu.Settings;
using Coloring2.Popups;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Coloring2.MainMenu
{
    public class MainMenuScreenController : MonoBehaviour
    {
        [SerializeField] private CategoriesInteractionsController _categoriesInteractionsController;
        [SerializeField] private List<MenuCategory> _categories;
        [Space(10)]
        [SerializeField] private AnimationCurve _itemScaleCurve;
        [SerializeField] private AnimationCurve _itemAlphaCurve;

        private PlayerPurchasesService _purchaseService;
        private PlayerInteractionActionsService _playerInteractionsService;
        private AppConfig _appConfig;

        private void OnDestroy()
        {
            ServicesManager.Unregister(ServicesManager.GetService<PlayerInteractionActionsService>());
            _categoriesInteractionsController.TryPurchaseCategory -= OnTryPurchaseCategory;
            if(_playerInteractionsService != null)
                _playerInteractionsService.SettingsButtonTapped -= OnSettingsButtonTap;
        }

        private void Start()
        {
            _appConfig = ServicesManager.GetService<ConfigsService>()
                .GetConfig<AppConfig>();
            _playerInteractionsService = ServicesManager.GetService<PlayerInteractionActionsService>();

            foreach (var cat in _categories)
                cat.Init(_itemScaleCurve, _itemAlphaCurve);
            
            SoundsManager.PlayBackgroundMusic();
            _categoriesInteractionsController.Init(_categories);
            
            _playerInteractionsService.SettingsButtonTapped += OnSettingsButtonTap;
            _categoriesInteractionsController.TryPurchaseCategory += OnTryPurchaseCategory;
        }

        private void OnTryPurchaseCategory(CategoryConfig config)
        {
            var popup = OpenEnterBirthdayPopup();
            popup.Success += OnPurchaseEnterBirthdaySuccess;
        }

        private void OnPurchaseEnterBirthdaySuccess(EnterBirthdayPopup popup)
        {
            popup.Success -= OnPurchaseEnterBirthdaySuccess;
            popup.Close();
        }

        private void OnSettingsButtonTap()
        {
            SoundsManager.PlayClick();
            var popup = OpenEnterBirthdayPopup();
            popup.Success += OnSettingsEnterBirthdaySuccess;
        }

        private void OnSettingsEnterBirthdaySuccess(EnterBirthdayPopup popup)
        {
            popup.Success -= OnSettingsEnterBirthdaySuccess;
            SoundsManager.PlayCorrectBirthYear();
            
            Debug.Log($"SettingsEnterBirthdaySuccess!");
            
            ModalPopupsManager.ShowPopup(_appConfig.SettingsPopupRef);
            ModalPopupsManager.Current.Closed += OnPopupClose;
        }

        private EnterBirthdayPopup OpenEnterBirthdayPopup()
        {
            var popup = ModalPopupsManager.ShowPopup<EnterBirthdayPopup>(_appConfig.EnterBirthdayPopupRef);
            popup.Closed += OnPopupClose;
            return popup;
        }

        private void OnPopupClose(IPopup popup)
        {
            popup.Closed -= OnPopupClose;
            ModalPopupsManager.RemovePopup();
        }
    }
}