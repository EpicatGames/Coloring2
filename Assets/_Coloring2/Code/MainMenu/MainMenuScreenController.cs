﻿using System;
using System.Collections.Generic;
using Coloring2.Configs;
using Coloring2.DataServices;
using Coloring2.MainMenu.Categories;
using Coloring2.MainMenu.Settings;
using Coloring2.Popups;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Coloring2.MainMenu
{
    public class MainMenuScreenController : MonoBehaviour
    {
        private static bool isFirstOpening = true;
        
        [SerializeField] private CategoriesInteractionsController _categoriesInteractionsController;
        [SerializeField] private List<MenuCategory> _categories;
        [Space(10)]
        [SerializeField] private AnimationCurve _itemScaleCurve;
        [SerializeField] private AnimationCurve _itemAlphaCurve;

        private PlayerPurchasesService _purchaseService;
        private PlayerInteractionActionsService _playerInteractionsService;
        private SelectedItemService _selectedItemService;
        private ScenesSwapScreen _scenesSwapScreen;
        private AppConfig _appConfig;

        private void OnDestroy()
        {
            _categoriesInteractionsController.CategorySelected -= OnCategorySelected;
            if(_playerInteractionsService != null)
                _playerInteractionsService.SettingsButtonTapped -= OnSettingsButtonTap;
        }

        private async void Start()
        {
            _selectedItemService = ServicesManager.GetService<SelectedItemService>();
            _purchaseService = ServicesManager.GetService<PlayerPurchasesService>();
            _scenesSwapScreen = ServicesManager.GetService<ProjectContextService>().ScenesSwapScreen;
            
            if (isFirstOpening)
            {
                _scenesSwapScreen.Show();
                isFirstOpening = false;
            }
            
            _appConfig = ServicesManager.GetService<ConfigsService>()
                .GetConfig<AppConfig>();
            _playerInteractionsService = ServicesManager.GetService<PlayerInteractionActionsService>();

            foreach (var cat in _categories)
                cat.Init(_itemScaleCurve, _itemAlphaCurve);
            
            _categoriesInteractionsController.Init(_categories);
            
            _playerInteractionsService.SettingsButtonTapped += OnSettingsButtonTap;
            _categoriesInteractionsController.CategorySelected += OnCategorySelected;

            await UniTask.DelayFrame(1);
            _scenesSwapScreen.FadeOut();
        }

        private void OnCategorySelected(CategoryConfig config)
        {
            if (config.PurchasedByDefault || _purchaseService.HasCategoryPurchased(config.Category))
            {
                _selectedItemService.Set(config);
                _scenesSwapScreen.FadeInComplete += LoadSelectPageScene;
                _scenesSwapScreen.FadeIn();
                return;
            }
            
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
        
        private void LoadSelectPageScene()
        {
            _scenesSwapScreen.FadeInComplete -= LoadSelectPageScene;
            ScenesManager.LoadScene(ScenesManager.Scenes.SelectPageScene);
        }
    }
}