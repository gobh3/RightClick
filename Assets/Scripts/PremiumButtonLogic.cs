using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PremiumButtonLogic : MonoBehaviour
{
    private string BALANCE_VAR = "balance";
    //private bool alreadyBought = false;
    //public bool BuyOnlyOnce;
    public UnityEvent OnAddPremiumKey;
    public UnityEvent OnNoPremiumKey;

    private void Awake()
    {
        //TO DO RESTORE "!" to "PlayerPrefs.HasKey(BALANCE_VAR"
        if (!PlayerPrefs.HasKey(BALANCE_VAR))
        {
            PlayerPrefs.SetInt(BALANCE_VAR, 0);
            PlayerPrefs.Save();
        }
    }
    public void AddOnePremiumKey()
    {
        int balance = PlayerPrefs.GetInt(BALANCE_VAR) + 1;
        OnAddPremiumKey?.Invoke();
        PlayerPrefs.SetInt(BALANCE_VAR, balance);
        PlayerPrefs.Save();
        //p/rint("Add Premium Key. Now You have: " + balance);
    }

    public void RemoveOnePremiumKey()
    {
        int balance = PlayerPrefs.GetInt(BALANCE_VAR) - 1;
        if (balance == 0)
            OnNoPremiumKey?.Invoke();
        PlayerPrefs.SetInt(BALANCE_VAR, balance);
        PlayerPrefs.Save();
        //p/rint("Remove Premium Key. Now You have: " + balance);
    }

    public int GetBalance()
    {
        return PlayerPrefs.GetInt(BALANCE_VAR);
    }

    /*public void SetAlreadyBought()
    {
        alreadyBought = true;
    }
    
    public bool IsAlreadyBaught()
    {
        if((BuyOnlyOnce && alreadyBought))
        {
            return true;
        }
        return false;
    }*/

}
