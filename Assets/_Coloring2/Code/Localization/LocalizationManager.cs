using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Coloring2.Localization
{
    public static class LocalizationManager
    {
        public class Keys
        {
            public const string Animals = "category_animals";
            public const string Princesses = "category_princesses";
            public const string Underwater = "category_underwater";
            public const string Transport = "category_transport";
            public const string Dinosaurs = "category_dinosaurs";
            public const string Picnic = "category_picnic";
            public const string Garden = "category_garden";
            public const string Aliens = "category_aliens";
            public const string Houses = "category_houses";
            public const string FullVersion = "full_version";
            public const string EnterBirthYear = "enter_year";
            public const string RateGame = "settings_rate";
            public const string Contact = "settings_contact";
            public const string RestorePurchases = "settings_restore";
            public const string SettingsFullVersion = "settings_fullversion";
            public const string PrivacyPolicy = "settings_privacy";
            public const string OurWebsite = "settings_website";
            public const string AllToolsAndColors = "purchase_tools";
            public const string PurchaseFull = "purchase_full";
            public const string Unlock = "purchase_unlock_button";
        }
        
        private static string[] _systemLanguages = Enum.GetNames(typeof(SystemLanguage));
        private static Dictionary<string, LangData> _languages = new Dictionary<string, LangData>();

        public static async UniTask<bool> Initialize(string url)
        {
            var request = Resources.LoadAsync<TextAsset>(url);
            await UniTask.WaitUntil(() => request.isDone);
            var dictList = CSVReader.Read(request.asset as TextAsset);
            
            for (var i = 0; i < dictList.Count; i++)
                Add(dictList[i]);
            
            return true;
        }

        private static void Add(Dictionary<string, object> category)
        {
            var lang = new LangData {Languages = new Dictionary<string, string>()};
            foreach (var kvp in category)
            {
                if (_systemLanguages.Contains(kvp.Key))
                    lang.Languages.Add(kvp.Key, kvp.Value.ToString());
            }
            _languages.Add(category.First().Value.ToString(), lang);
        }

        public static string GetLocalization(string id)
        {
            var lang = _languages[id];
            return lang.Languages[Application.systemLanguage.ToString()];
        }
        
        private struct LangData
        {
            public Dictionary<string, string> Languages;
        }
    }
}