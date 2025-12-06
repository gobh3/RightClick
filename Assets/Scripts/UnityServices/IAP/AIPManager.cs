using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Events;
public class AIPManager : MonoBehaviour
{
    public UnityEvent<int> OnPurchase3Hearts;
    public UnityEvent<int> OnPurchase8Hearts;
    public const string hearts3 = "com.adadi.rightclick.3hearts";
    public const string hearts8 = "com.adadi.rightclick.8hearts";

    public void OnPurchaseComplete(Product product)
    {
        switch (product.definition.id)
        {
            case hearts3:
                {
                    OnPurchase3Hearts?.Invoke(3);
                    //Debug.Log("You've gained 3 hearts");
                    break;
                }
            case hearts8:
                {
                    OnPurchase8Hearts?.Invoke(8);
                    //Debug.Log("You've gained 8 hearts");
                    break;
                }

        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureReason)
    {
        Debug.Log(product.definition.id + " failed because " + failureReason);
    }
}
