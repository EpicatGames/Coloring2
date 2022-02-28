using Coloring2.Localization;
using Coloring2.Popups;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2.PaintingPage
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

        protected override void Awake()
        {
            base.Awake();
            
            _fullVersionLabelField.text = LocalizationManager.GetLocalization(LocalizationManager.Keys.PurchaseFull);
            _colorsLabelField.text = LocalizationManager.GetLocalization(LocalizationManager.Keys.AllToolsAndColors);
            
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
            Debug.Log($"OnColorsTapTap");
            
        }

        private void OnFullVersionOpenTap()
        {
            Debug.Log($"OnFullVersionOpenTap");
        }
    }
}