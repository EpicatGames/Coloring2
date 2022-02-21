using System;
using System.Collections.Generic;
using System.Linq;
using Coloring2.Configs;
using JetBrains.Annotations;
using UnityEngine;

namespace Coloring2.DataServices
{
    public class PlayerPurchasesService : AbstractService
    {
        private const string boughtSomethingKey = "bought_something";
        private const string successfulCrossPromoKey = "successful_cross_promo";

        // private static readonly Categories[] categories = {
        //     Categories.category_animals,
        //     Categories.category_princesses,
        //     Categories.category_underwater,
        //     Categories.category_transport,
        //     Categories.category_dinosaurs,
        //     Categories.category_picnic,
        //     Categories.category_garden,
        //     Categories.category_aliens,
        //     Categories.category_houses,
        //     Categories.category_full
        // };
        
        public Action<string> Purchased;
        public Action SuccessfulCrossPromo;

        public List<Categories> PurchasedCategories { get; } = new List<Categories>();
        private List<CategoryConfig> _categoryConfigs;

        public bool BoughtSomething { get; private set; }
        public bool IsCrossPromoSuccessful { get; private set; }

        public PlayerPurchasesService(List<CategoryConfig> categoryConfigs)
        {
            _categoryConfigs = categoryConfigs;
            CheckOnStart();
            
            Purchased += OnPurchased;
            SuccessfulCrossPromo += OnSuccessfulCrossPromo;
        }
        
        public override void Dispose()
        {
            Purchased -= OnPurchased;
            SuccessfulCrossPromo -= OnSuccessfulCrossPromo;
        }

        public bool HasCategoryPurchased(Categories category) => PurchasedCategories.Contains(category);

        private void CheckOnStart()
        {
            IsCrossPromoSuccessful = PlayerPrefs.GetInt(successfulCrossPromoKey) > 0;
            foreach (var cat in _categoryConfigs)
            {
                if(PlayerPrefs.GetInt(cat.ToString()) > 0 || cat.PurchasedByDefault)
                    PurchasedCategories.Add(cat.Category);
            }
            
            ////////////////////// TEMP /////////////////////
            PurchasedCategories.Add(Categories.category_aliens);
            PurchasedCategories.Add(Categories.category_dinosaurs);
            PurchasedCategories.Add(Categories.category_garden);
            ////////////////////////////////////////////////

            if (PurchasedCategories.Count > 0)
                SetBoughtSomething();
        }

        private void OnSuccessfulCrossPromo()
        {
            if(PlayerPrefs.GetInt(successfulCrossPromoKey) > 0)
                return;
            PlayerPrefs.SetInt(successfulCrossPromoKey, 1);
            IsCrossPromoSuccessful = true;
            PlayerPrefs.Save();
        }

        private void SetBoughtSomething()
        {
            PlayerPrefs.SetInt(boughtSomethingKey, 1);
            BoughtSomething = true;
        }

        private void OnPurchased(string productKey)
        {
            if (PlayerPrefs.GetInt(boughtSomethingKey) <= 0)
                SetBoughtSomething();
            
            if(PlayerPrefs.GetInt(productKey) > 0)
                return;
            
            PlayerPrefs.SetInt(productKey, 1);
            PlayerPrefs.Save();
        }
    }
}