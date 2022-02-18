using Coloring2.Localization;
using Coloring2.Popups;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2.MainMenu.Settings
{
    public class SettingsPopup : AbstractModalPopup
    {
        [SerializeField] private Button _rateUsBtn;
        [SerializeField] private Button _contactUsBtn;
        [SerializeField] private Button _restorePurchasesBtn;
        [SerializeField] private Button _fullVersionBtn;
        [SerializeField] private Button _privacyPolicyBtn;
        [SerializeField] private Button _ourWebsiteBtn;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _rateUsBtn.onClick.RemoveListener(OnRateUsBtnTap);
            _contactUsBtn.onClick.RemoveListener(OnContactUsBtnTap);
            _restorePurchasesBtn.onClick.RemoveListener(OnRestorePurchasesBtnTap);
            _fullVersionBtn.onClick.RemoveListener(OnFullVersionBtnTap);
            _privacyPolicyBtn.onClick.RemoveListener(OnPrivacyPolicyBtnTap);
            _ourWebsiteBtn.onClick.RemoveListener(OnOurWebsiteBtnTap);
        }

        public override void Show()
        {
            base.Show();
            
            _rateUsBtn
                .GetComponentInChildren<TextMeshProUGUI>()
                .text = LocalizationManager.GetLocalization(LocalizationManager.Keys.RateGame);
            _rateUsBtn.onClick.AddListener(OnRateUsBtnTap);
            
            _contactUsBtn
                .GetComponentInChildren<TextMeshProUGUI>()
                .text = LocalizationManager.GetLocalization(LocalizationManager.Keys.Contact);
            _contactUsBtn.onClick.AddListener(OnContactUsBtnTap);
            
            _restorePurchasesBtn
                .GetComponentInChildren<TextMeshProUGUI>()
                .text = LocalizationManager.GetLocalization(LocalizationManager.Keys.RestorePurchases);
            _restorePurchasesBtn.onClick.AddListener(OnRestorePurchasesBtnTap);
            
            _fullVersionBtn
                .GetComponentInChildren<TextMeshProUGUI>()
                .text = LocalizationManager.GetLocalization(LocalizationManager.Keys.FullVersion);
            _fullVersionBtn.onClick.AddListener(OnFullVersionBtnTap);
            
            _privacyPolicyBtn
                .GetComponentInChildren<TextMeshProUGUI>()
                .text = LocalizationManager.GetLocalization(LocalizationManager.Keys.PrivacyPolicy);
            _privacyPolicyBtn.onClick.AddListener(OnPrivacyPolicyBtnTap);
            
            _ourWebsiteBtn
                .GetComponentInChildren<TextMeshProUGUI>()
                .text = LocalizationManager.GetLocalization(LocalizationManager.Keys.OurWebsite);
            _ourWebsiteBtn.onClick.AddListener(OnOurWebsiteBtnTap);
        }

        private void OnOurWebsiteBtnTap()
        {
            SoundsManager.PlayClick();
            Application.OpenURL("http://epicat.fun/");
        }

        private void OnPrivacyPolicyBtnTap()
        {
            SoundsManager.PlayClick();
            Application.OpenURL("http://epicat.fun/privacy/");
        }

        private void OnFullVersionBtnTap()
        {
            SoundsManager.PlayClick();
            //Close();
        }

        private void OnRestorePurchasesBtnTap()
        {
            SoundsManager.PlayClick();
        }

        private void OnContactUsBtnTap()
        {
            SoundsManager.PlayClick();
            Application.OpenURL("mailto:" + "kids@epicat.fun" + "?subject:" + "KidsPaintGame");
        }

        private void OnRateUsBtnTap()
        {
            SoundsManager.PlayClick();
            string rateUsURL = null; 
#if UNITY_IOS
            rateUsURL = "https://itunes.apple.com/app/id1546739987?action=write-review";
#elif UNITY_ANDROID
            rateUsURL = "https://play.google.com/store/apps/details?id=com.epicat.coloring";
#else
            rateUsURL = "http://epicat.fun/";
#endif
            Application.OpenURL(rateUsURL);
        }
    }
}