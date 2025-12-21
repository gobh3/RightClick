using UnityEngine;
using UnityEngine.Events;

public class IAPFullfillmentPurchases : MonoBehaviour
{
    [Header("Purchase Events")]
    [SerializeField] private UnityEvent OnPurchase3Hearts;
    [SerializeField] private UnityEvent OnPurchase8Hearts;
    [SerializeField] private UnityEvent OnPurchasePlus2HeartsRegular;
    [SerializeField] private UnityEvent OnPurchasePlus2HeartsPremium;

    // ---- DUPLICATE-PROTECTION ----
    private const string PurchaseHandledPrefix = "IAP_HANDLED_";

    private bool TryHandlePurchase(string purchaseKey)
    {
        string key = PurchaseHandledPrefix + purchaseKey;

        if (PlayerPrefs.HasKey(key))
        {
            Debug.Log($"[IAP] Duplicate purchase ignored: {purchaseKey}");
            return false;
        }

        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
        return true;
    }

    // ---- FULFILLMENT METHODS ----
    // These are called from Codeless IAP Button (Inspector)

    public void FullfillmentPurchasing3Hearts()
    {
        if (!TryHandlePurchase("3_HEARTS"))
            return;

        Debug.Log("[IAP] 3 Hearts fulfilled");
        OnPurchase3Hearts?.Invoke();
    }

    public void FullfillmentPurchasing8Hearts()
    {
        if (!TryHandlePurchase("8_HEARTS"))
            return;

        Debug.Log("[IAP] 8 Hearts fulfilled");
        OnPurchase8Hearts?.Invoke();
    }

    public void FullfillmentPurchasingPlus2HeartsRegular()
    {
        if (!TryHandlePurchase("PLUS_2_HEARTS_REGULAR"))
            return;

        Debug.Log("[IAP] +2 Hearts Regular fulfilled");
        OnPurchasePlus2HeartsRegular?.Invoke();
    }

    public void FullfillmentPurchasingPlus2HeartsPremium()
    {
        if (!TryHandlePurchase("PLUS_2_HEARTS_PREMIUM"))
            return;

        Debug.Log("[IAP] +2 Hearts Premium fulfilled");
        OnPurchasePlus2HeartsPremium?.Invoke();
    }
}
