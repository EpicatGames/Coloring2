using System;
using System.Collections.Generic;
using System.Linq;
using Coloring2.Configs;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Coloring2.DataServices
{
    public class PlayerPurchasesService : AbstractService, IStoreListener
    {
        #region Static
        private static IStoreController _storeController;
        private static IExtensionProvider _storeExtensionProvider;

        private static bool IsIAPInitialized() => _storeController != null && _storeExtensionProvider != null;
        
        public static string GetPriceString(Categories category)
        {
            if(IsIAPInitialized() == false)
                return string.Empty;
            
            var product = _storeController.products.WithID(category.ToString());
            return product == null ? string.Empty : product.metadata.localizedPriceString;
        }
        #endregion



        public Action<Categories> Purchased;
        public Action<string> PurchaseFailed;

        public List<Categories> PurchasedCategories { get; } = new List<Categories>();
        private List<CategoryConfig> _categoryConfigs;

        public PlayerPurchasesService(List<CategoryConfig> categoryConfigs)
        {
            InitializePurchasing();
            _categoryConfigs = categoryConfigs;
            CheckOnStart();
        }

        private void InitializePurchasing()
        {
            var module = StandardPurchasingModule.Instance();
            var builder = ConfigurationBuilder.Instance(module);
            //module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
            
            builder.AddProduct(Categories.category_aliens.ToString(), ProductType.NonConsumable);
            builder.AddProduct(Categories.category_dinosaurs.ToString(), ProductType.NonConsumable);
            builder.AddProduct(Categories.category_garden.ToString(), ProductType.NonConsumable);
            builder.AddProduct(Categories.category_houses.ToString(), ProductType.NonConsumable);
            builder.AddProduct(Categories.category_picnic.ToString(), ProductType.NonConsumable);
            builder.AddProduct(Categories.category_princesses.ToString(), ProductType.NonConsumable);
            builder.AddProduct(Categories.category_transport.ToString(), ProductType.NonConsumable);
            builder.AddProduct(Categories.category_underwater.ToString(), ProductType.NonConsumable);
            builder.AddProduct(Categories.full_version.ToString(), ProductType.NonConsumable);
            builder.AddProduct(Categories.colors.ToString(), ProductType.NonConsumable);

            UnityPurchasing.Initialize(this, builder);
        }

        public bool HasCategoryPurchased(Categories category) => PurchasedCategories.Contains(category);
        public bool HasAllColorsPurchased() => PlayerPrefs.GetInt(Categories.colors.ToString(), 0) > 0;

        private void CheckOnStart()
        {
            foreach (var cat in _categoryConfigs)
            {
                var savedValue = PlayerPrefs.GetInt(cat.Category.ToString(), 0);
                if (savedValue > 0 || cat.PurchasedByDefault)
                {
                    if(!PurchasedCategories.Contains(cat.Category))
                        PurchasedCategories.Add(cat.Category);
                }
            }
        }
        
        public void Purchase(Categories category)
        {
            if (IsIAPInitialized())
                _storeController.InitiatePurchase(category.ToString());
            else OnInitializeFailed(UnityEngine.Purchasing.InitializationFailureReason.PurchasingUnavailable);
        }


        #region Unity IAP
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("In-App Purchasing successfully initialized");
            _storeController = controller;
            _storeExtensionProvider = extensions;
        }
        
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log($"In-App Purchasing initialize failed: {error}");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            var product = e.purchasedProduct;
            var productKey = product.definition.id;
            
            if (Enum.TryParse(productKey, out Categories cat))
            {
                switch (cat)
                {
                    case Categories.full_version:
                        foreach (var c in _categoryConfigs)
                        {
                            if (!PurchasedCategories.Contains(c.Category))
                                PurchasedCategories.Add(c.Category);
                            PlayerPrefs.SetInt(c.Category.ToString(), 1);
                        }
                        break;
                    default:
                        if(!PurchasedCategories.Contains(cat))
                            PurchasedCategories.Add(cat);
                        break;
                }
                
                PlayerPrefs.SetInt(productKey, 1);
                PlayerPrefs.Save();
                
                Debug.Log($"Purchase Complete - Product: {productKey}");
                Purchased?.Invoke(cat);
            }
            else
            {
                Debug.LogError($"{product.definition.id} does not match existing category");
            }

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
            PurchaseFailed?.Invoke(product.definition.id);
        }
        #endregion
    }
}