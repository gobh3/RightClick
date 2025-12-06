using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

public class IAPButtonView : MonoBehaviour, IStoreController
{
    IAP IAP;
    public ProductCollection products => IAP.m_StoreController.products;
    public UnityEvent<Product> OnFetched;

    public void ConfirmPendingPurchase(Product product)
    {
        //throw new NotImplementedException();
    }

    public void FetchAdditionalProducts(HashSet<ProductDefinition> additionalProducts, Action successCallback, Action<InitializationFailureReason> failCallback)
    {
        //throw new NotImplementedException();
    }

    public void FetchAdditionalProducts(HashSet<ProductDefinition> additionalProducts, Action successCallback, Action<InitializationFailureReason, string> failCallback)
    {
        //throw new NotImplementedException();
    }

    public void InitiatePurchase(Product product, string payload)
    {
        //throw new NotImplementedException();
        OnFetched?.Invoke(product);
    }

    public void InitiatePurchase(string productId, string payload)
    {
        //throw new NotImplementedException();
        OnFetched?.Invoke(products.WithID(productId));
    }

    public void InitiatePurchase(Product product)
    {
        //throw new NotImplementedException();
        OnFetched?.Invoke(product);
    }

    public void InitiatePurchase(string productId)
    {
        OnFetched?.Invoke(products.WithID(productId));
    }

}
