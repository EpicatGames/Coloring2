﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
//using Mycom.Tracker.Unity;

//public class PurchaseManager : MonoBehaviour, IStoreListener
//{
    /*private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    public string GetPriceString(ProductCatalogItem productCatalog)
    {
        if (m_StoreController != null)
        {
            Product product = m_StoreController.products.WithID(productCatalog.id);
            if (product != null)
            {
                string priceString = "";
                string currency = product.metadata.isoCurrencyCode;
                string price = product.metadata.localizedPrice.ToString("G29");

                switch (currency)
                {
                    case ("USD"):
                        priceString += "$" + price;
                        break;
                    case ("EUR"):
                        priceString += price + " €";
                        break;
                    case ("GBP"):
                        priceString += "£" + price;
                        break;
                    case ("RUB"):
                        priceString += price + " ₽";
                        break;
                    default:
                        priceString += price + " " + currency;
                        break;
                }

                return priceString;
            }
        }
        return "";
    }

    private void Awake()
    {
        var module = StandardPurchasingModule.Instance();

        // The FakeStore supports: no-ui (always succeeding), basic ui (purchase pass/fail), and
        // developer ui (initialization, purchase, failure code setting). These correspond to
        // the FakeStoreUIMode Enum values passed into StandardPurchasingModule.useFakeStoreUIMode.
        module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

        var builder = ConfigurationBuilder.Instance(module);
        var catalog = ProductCatalog.LoadDefaultCatalog();

        foreach (var product in catalog.allProducts)
        {
            if (product.allStoreIDs.Count > 0)
            {
                var ids = new IDs();
                foreach (var storeID in product.allStoreIDs)
                {
                    ids.Add(storeID.id, storeID.store);
                }
                builder.AddProduct(product.id, product.type, ids);
            }
            else
            {
                builder.AddProduct(product.id, product.type);
            }
        }

        UnityPurchasing.Initialize(this, builder);
    }

    public void InitializePurchasing()
    {

        if (IsInitialized())
        {
            return;
        }

    }

    public bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        PurchaseProcessingResult result;
        IAPListener[] activeListeners = FindObjectsOfType<IAPListener>();
        bool consumePurchase = false;
        bool resultProcessed = false;

        foreach (IAPListener listener in activeListeners)
        {
            result = listener.ProcessPurchase(e);

            if (result == PurchaseProcessingResult.Complete)
            {
                consumePurchase = true;
                //MyTracker.TrackPurchaseEvent(e.purchasedProduct);
            }

            resultProcessed = true;
        }

        // we expect at least one receiver to get this message
        if (!resultProcessed)
        {

            Debug.LogError("Purchase not correctly processed for product \"" +
                             e.purchasedProduct.definition.id +
                             "\". Add an active IAPButton to process this purchase, or add an IAPListener to receive any unhandled purchase events.");

        }


        return (consumePurchase) ? PurchaseProcessingResult.Complete : PurchaseProcessingResult.Pending;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void Purchase(string productId)
    {
        if (IsInitialized())
        {
            m_StoreController.InitiatePurchase(productId);
        }
        else OnInitializeFailed(UnityEngine.Purchasing.InitializationFailureReason.PurchasingUnavailable);
    }*/
    
//}

