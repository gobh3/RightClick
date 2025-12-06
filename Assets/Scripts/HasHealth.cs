using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HasHealth : MonoBehaviour
{
    public SettingsManager playerSettings;
    public AIPInfoManager aipInfoManager;
    public UnityEvent<int> OnHealthReduced;
    public UnityEvent<int> OnHealthIncreased;
    public UnityEvent<int> OnHealthInitiaized;
    public UnityEvent OnDeath;
    public bool TakeBonusHealthOnEditor = true;
    //[Range(1, 100)]
    //public int BaseHealth;
    //private string BONOUS_HEALTH = "bonusHealth";
    private int health;

    /*public void Awake()
    {
        //create var once on installation.
        if (!PlayerPrefs.HasKey(BONOUS_HEALTH))
        {
            PlayerPrefs.SetInt(BONOUS_HEALTH, 0);
            PlayerPrefs.Save();
        }
    }*/
/*
    private int getBonousHealth()
    {
        if (PlayerPrefs.HasKey(BONOUS_HEALTH))
            return PlayerPrefs.GetInt(BONOUS_HEALTH);
        else
        {
            print("ERROR: didn't find bonusHealth var");
            return 0;
        }
    }*/
    public void Initialize()
    {
        health = playerSettings.playerSettings.BaseHealth;
        if (!Application.isEditor || TakeBonusHealthOnEditor)
        {
            if (aipInfoManager.GetInfo().PLUS_2_HEARTS_REGULAR)
                health += playerSettings.playerSettings.BonusHealthPerGame;
            if (aipInfoManager.GetInfo().PLUS_2_HEARTS_PREMIUM)
                health += playerSettings.playerSettings.BonusHealthPerGame; //TODO
            if (aipInfoManager.GetInfo().PLUS_HEARTS_FROM_SHARING > 0)
                health += aipInfoManager.GetInfo().PLUS_HEARTS_FROM_SHARING; 
        }
        OnHealthInitiaized?.Invoke(health);
    }

    public void ReduceHealth()
    {
        if (health > 0)
        {
            health -= 1;
            OnHealthReduced?.Invoke(health);
        }
        if (health == 0)
            OnDeath?.Invoke();
    }

    public void AddHealth(int toAdd)
    {
        health += toAdd;
        OnHealthIncreased?.Invoke(health);
    }
    /*
    public void AddBonousHealth(int toAdd)
    {
        int t = 0;
        
        if (PlayerPrefs.HasKey(BONOUS_HEALTH))
        {
            PlayerPrefs.SetInt(BONOUS_HEALTH, getBonousHealth() + toAdd);
            PlayerPrefs.Save();
            print("Bonus Health: " + getBonousHealth());
        }
        else
        {
            print("ERROR: didn't find bonusHealth var");
        }
    }*/

    public void AddHealthOnNewLevel()
    {
        int toAdd = 0;
        if (aipInfoManager.GetInfo().PLUS_2_HEARTS_REGULAR)
            toAdd += playerSettings.playerSettings.BonusHealthPerLevel;
        if (aipInfoManager.GetInfo().PLUS_2_HEARTS_PREMIUM)
            toAdd += playerSettings.playerSettings.BonusHealthPerLevel;
        health += toAdd;
        OnHealthIncreased?.Invoke(health);
    }
}
