using System;
using Coloring2.Configs;
using Coloring2.DataServices;
using Coloring2.Localization;
using Coloring2.Popups;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2.Popups
{
    public class ColorsPurchasePopup : AbstractModalPopup
    {
        [Space(15)]
        [SerializeField] private Button _openFullVersionBtn;
        [SerializeField] private Button _openColorsBtn;
        
        [Space(5)]
        [SerializeField] private TextMeshProUGUI _fullVersionPriceField;
        [SerializeField] private TextMeshProUGUI _colorsPriceField;
        
        [Space(5)]
        [SerializeField] private TextMeshProUGUI _fullVersionLabelField;
        [SerializeField] private TextMeshProUGUI _colorsLabelField;
        
        private PlayerPurchasesService _purchaseService;

        protected override void Awake()
        {
            base.Awake();

            _purchaseService = ServicesManager.GetService<PlayerPurchasesService>();
            _fullVersionLabelField.text = LocalizationManager.GetLocalization(LocalizationManager.Keys.PurchaseFull);
            _colorsLabelField.text = LocalizationManager.GetLocalization(LocalizationManager.Keys.AllToolsAndColors);

            _fullVersionPriceField.text = PlayerPurchasesService.GetPriceString(Categories.full_version);
            _colorsPriceField.text = PlayerPurchasesService.GetPriceString(Categories.colors);
            
            var openLabel = LocalizationManager.GetLocalization(LocalizationManager.Keys.Unlock);
            _openFullVersionBtn.GetComponentInChildren<TextMeshProUGUI>().text = openLabel;
            _openColorsBtn.GetComponentInChildren<TextMeshProUGUI>().text = openLabel;
            
            _openFullVersionBtn.onClick.AddListener(OnFullVersionOpenTap);
            _openColorsBtn.onClick.AddListener(OnColorsTapTap);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _openFullVersionBtn.onClick.RemoveListener(OnFullVersionOpenTap);
            _openColorsBtn.onClick.RemoveListener(OnColorsTapTap);
        }

        private void OnColorsTapTap()
        {
            AddPurchaseListeners();
            _purchaseService.Purchase(Categories.colors);
        }

        private void OnFullVersionOpenTap()
        {
            AddPurchaseListeners();
            _purchaseService.Purchase(Categories.full_version);
        }
        
        private void AddPurchaseListeners()
        {
            _purchaseService.Purchased += OnCategoryPurchased;
            _purchaseService.PurchaseFailed += OnPurchaseFailed;
        }
        
        private void RemovePurchaseListeners()
        {
            _purchaseService.Purchased -= OnCategoryPurchased;
            _purchaseService.PurchaseFailed -= OnPurchaseFailed;
        }
        
        private void OnCategoryPurchased(Categories category)
        {
            RemovePurchaseListeners();
            Close();
        }

        private void OnPurchaseFailed(string productId)
        {
            RemovePurchaseListeners();
            Close();
        }
    }
}