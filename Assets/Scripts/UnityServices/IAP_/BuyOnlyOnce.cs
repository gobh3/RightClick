using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuyOnlyOnce : MonoBehaviour
{
    public bool activateAlreadyPurchasedCheckOnEditor;
    public bool activateAlreadyPurchasedCheckOnOther;
    public SavedBool alreadyBought;
    public UnityEvent OnAlreadyBaught;
    public UnityEvent OnBought;
    public virtual void Initialize()
    {
        if ((Application.isEditor && activateAlreadyPurchasedCheckOnEditor) 
            || (!Application.isEditor && activateAlreadyPurchasedCheckOnOther))
            if (alreadyBought.GetValue())
                OnAlreadyBaught?.Invoke();
    }

    public void SetBought()
    {
        OnBought.Invoke();
        alreadyBought.SaveValue(true);
    }



}
