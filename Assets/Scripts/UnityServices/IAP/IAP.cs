using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.Pool;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Product = UnityEngine.Purchasing.Product;

public class IAP : MonoBehaviour, IDetailedStoreListener
{
    // Required for inntializing Unity gaming Service.
    // (You must initialize gaming service before intiaizing unity purchasing)
    public string environment = "production";
    //Required for unity purchaing
    public IStoreController m_StoreController;
    IGooglePlayStoreExtensions m_GooglePlayStoreExtensions;

    //Your products IDs.
    public string get3HeartsProductId = "com.adadi.rightclick.3hearts";
    public string get8HeartsProductId = "com.adadi.rightclick.8hearts";
    public string getPlus2HeartsRegularProductId = "com.adadi.rightclick.plus2hearts_every_game_regular";
    public string getPlus2HeartsPremiumProductId = "com.adadi.rightclick.plus2hearts_every_game_premium";

    //events
    public UnityEvent OnPurchasing3Hearts;
    public UnityEvent OnPurchasing8Hearts;
    public UnityEvent OnPurchasingPlus2HeartsRegular;
    public UnityEvent OnPurchasingPlus2HeartsPremium;

    public UnityEvent OnPurchasing3HeartsFailed;
    public UnityEvent OnPurchasing8HeartsFailed;

    public UnityEvent<Product> OnProductFetchd;

    //shabat detctor for preventing selling on shabbat
    public ShabbatDetector shabatDetector;
    public WarningPopup warningPopupPf; //warning popup for showing warning messages Prefab
    public GameObject warningPopupFather;
    public string CANT_BUY_ON_SHABAT_WARNING;
    private async void Start()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            var options = new InitializationOptions().SetEnvironmentName(environment);
            try
            {
                await UnityServices.InitializeAsync(options);
            }
            catch (Exception e)
            {
                Debug.LogError("Unity Services failed to initialize: " + e);
                return;
            }
        }
        InitializeIAP();
    }

    private void InitalizeUnityServices()
    {
        var options = new InitializationOptions()
                     .SetEnvironmentName(environment);
        try
        {
            UnityServices.InitializeAsync(options);
        }
        catch (Exception e)
        {
            Debug.Log("Didn't Initilize Unity Services Because: \n" + e);
        }
    }

    public void InitializeIAP()
    {
        // intializing unity purchaing
        try
        {
            StandardPurchasingModule module = StandardPurchasingModule.Instance();
            ProductCatalog catalog = ProductCatalog.LoadDefaultCatalog();
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);
            
            // Manually add products from catalog (IAPConfigurationHelper is deprecated in Unity 6)
            foreach (var product in catalog.allProducts)
            {
                if (product.allStoreIDs.Count > 0)
                {
                    var storeIDs = new StoreSpecificIds();
                    foreach (var storeID in product.allStoreIDs)
                    {
                        storeIDs.Add(storeID.id, storeID.store);
                    }
                    builder.AddProduct(product.id, product.type, storeIDs);
                }
                else
                {
                    builder.AddProduct(product.id, product.type);
                }
            }
            
            UnityPurchasing.Initialize(this, builder);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        //Debug.Log("In-App Purchasing successfully initialized");
        m_StoreController = controller;
        m_GooglePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();
        int i = 0;
        string report = "";
        foreach (Product product in controller.products.all)
        {
            OnProductFetchd?.Invoke(product);
            /*report += ("Product Number: " + i + "\n");
            report += ("Title: " + product.metadata.localizedTitle + "\n");
            report += ("localizedPrice: " + product.metadata.localizedPrice + "\n");
            report += ("isoCurrencyCode: " + product.metadata.isoCurrencyCode + "\n");
            report += ("localizedPriceString: " + product.metadata.localizedPriceString + "\n");
            report += ("localizedDescription:" + product.metadata.localizedDescription + "\n");
            i++;*/
        }
        //print(report);
        foreach (var p in controller.products.all)
        {
            Debug.Log($"[IAP] Product fetched: {p.definition.id} - {p.metadata.localizedTitle}");
        }
        Debug.Log("[IAP] Total products loaded: " + controller.products.all.Length);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        OnInitializeFailed(error, null);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

        if (message != null)
        {
            errorMessage += $" More details: {message}";
        }

        Debug.Log(errorMessage);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
        if (product.definition.id == get3HeartsProductId)
        {
            OnPurchasing3HeartsFailed?.Invoke();
        }
        if (product.definition.id == get8HeartsProductId)
        {
            OnPurchasing8HeartsFailed?.Invoke();
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
                        $" Purchase failure reason: {failureDescription.reason}," +
                        $" Purchase failure details: {failureDescription.message}");
        if (product.definition.id == get3HeartsProductId)
        {
            OnPurchasing3HeartsFailed?.Invoke();
        }
        if (product.definition.id == get8HeartsProductId)
        {
            OnPurchasing8HeartsFailed?.Invoke();
        }
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Product product = args.purchasedProduct;

        Debug.Log($"Processing Purchase: {product.definition.id}");

        //Handle product goods' fullfillment
        if (product.definition.id == get3HeartsProductId)
        {
            OnPurchasing3Hearts?.Invoke();
        }
        if (product.definition.id == get8HeartsProductId)
        {
            OnPurchasing8Hearts?.Invoke();
        }
        if (product.definition.id == getPlus2HeartsRegularProductId)
        {
            OnPurchasingPlus2HeartsRegular?.Invoke();
        }
        if (product.definition.id == getPlus2HeartsPremiumProductId)
        {
            OnPurchasingPlus2HeartsPremium?.Invoke();
        }
        return PurchaseProcessingResult.Complete;
    }


    public void RestorePurchases()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
                //TODO
                break;
            case RuntimePlatform.Android:
                m_GooglePlayStoreExtensions.RestoreTransactions(OnRestorePurchases);
                break;
        }
        if (Application.isEditor)
            Debug.LogError("Can't restore purchases in editor!");

    }

    void OnRestorePurchases(bool success, string error)
    {
        var restoreMessage = "";
        if (success)
        {
            // This does not mean anything was restored,
            // merely that the restoration process succeeded.
            restoreMessage = "Restore Successful (this does not mean anything was restored)";
        }
        else
        {
            // Restoration failed.
            restoreMessage = $"Restore Failed with error: {error}";
        }

        Debug.Log(restoreMessage);
    }

    #region BuyProduct Method for each product
    private bool isShabat()
    {
        if (shabatDetector.IsShabbat)
        {
            Debug.Log("Can't buy on shabbat");
            // Show warning popup
            WarningPopup warningPopup = Instantiate(warningPopupPf,warningPopupFather.transform);
            warningPopup.Show(CANT_BUY_ON_SHABAT_WARNING);
            return true;
        }
        return false;
    }

    public void Buy3Hearts()
    {
        if (isShabat()) // check if it is shabat
        {
            OnPurchasing3HeartsFailed?.Invoke();
            print("Please try again on motzash!");
            return;
        };
        if (!(m_StoreController == null)) // check whteher first initialization didn't work
            m_StoreController.InitiatePurchase(get3HeartsProductId);
        else
        {
            solveProblemAndBuy(get3HeartsProductId);
        }

    }

    public void Buy8Hearts()
    {
        if (isShabat()) // check if it is shabat
        {
            OnPurchasing8HeartsFailed?.Invoke();
            return;
        }
        if (!(m_StoreController == null)) // check whteher first initialization didn't work
            m_StoreController.InitiatePurchase(get8HeartsProductId);
        else solveProblemAndBuy(get8HeartsProductId);
    }

    public void BuyPlus2HeartsRegular()
    {
        if (isShabat()) // check if it is shabat
            return;
        if (!(m_StoreController == null)) // check whteher first initialization didn't work
            m_StoreController.InitiatePurchase(getPlus2HeartsRegularProductId);
        else solveProblemAndBuy(getPlus2HeartsRegularProductId);
    }

    public void BuyPlus2HeartsPremium()
    {
        if (isShabat()) // check if it is shabat
            return;
        if (!(m_StoreController == null)) // check whteher first initialization didn't work
            m_StoreController.InitiatePurchase(getPlus2HeartsPremiumProductId);
        else solveProblemAndBuy(getPlus2HeartsPremiumProductId);
    }

    void solveProblemAndBuy(string prodString)
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            Debug.Log("IAP was not initialized Because UnityServices is Uninitialized." +
                "\n" +
                "Trying again to intialize UnityServices...");
            InitalizeUnityServices();
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                Debug.Log("UnityServices initialization didn't succeed. Purchase canceled.");
            }
            else if (UnityServices.State == ServicesInitializationState.Initialized)
            {
                Debug.Log("Now trying again to intialize UnityServices...");
                InitializeIAP(); //try again initializing
                if ((m_StoreController == null))
                    Debug.Log("IAP initialization didn't succeed. Purchase canceled.");
                else m_StoreController.InitiatePurchase(prodString);

            }
        }
        else if ((m_StoreController == null)) // probably there is no internet connection
        {
            Debug.Log("IAP initialization didn't succeed at the start of the game. \n" +
                " Trying again to intialize IAP... ");
            InitializeIAP();
            if (!(m_StoreController == null))
                m_StoreController.InitiatePurchase(prodString);
            else
                Debug.Log("IAP initialization didn't succeed. Purchase canceled.");
        }

    }


    #endregion
}
