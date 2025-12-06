using System;
using System.IO;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;
using Newtonsoft.Json;
public class SettingsManager : MonoBehaviour
{
    public Settings settings;
    public PlayerSettings playerSettings;
    public EffectsSettings effectsSettings;
    public ObstacleGameConfig obstacleGameConfig;
    public string SettingsFilename;
    public string PlayerSettingsFilename;
    public string EffectsSettingsFilename;
    public string ObstacleGameConfigFilename;

    public void InitializeSettings()
    {
        if (isExists(SettingsFilename))
        {
            settings = jsonToClass<Settings>(loadStringFromFile(SettingsFilename));
            //Debug.Log("Settings updated from memory");
        }
        else Debug.Log("use editor values for settings");
        if (isExists(PlayerSettingsFilename))
        {
            playerSettings = jsonToClass<PlayerSettings>(loadStringFromFile(PlayerSettingsFilename));
            //Debug.Log("playerSettings updated from memory");
        }
        else Debug.Log("use editor values for player settings");
        if (isExists(EffectsSettingsFilename))
        {
            effectsSettings = jsonToClass<EffectsSettings>(loadStringFromFile(EffectsSettingsFilename));
            //Debug.Log("effectsSettings updated from memory");
        }
        else Debug.Log("use editor values for effects' settings");
        if (isExists(ObstacleGameConfigFilename))
        {
            obstacleGameConfig = jsonToClass<ObstacleGameConfig>(loadStringFromFile(ObstacleGameConfigFilename));
            //Debug.Log("effectsSettings updated from memory");
        }
        else Debug.Log("use editor values for obstacleGameConfig settings");
    }

    private bool isExists(string f)
    {
        if (File.Exists(Application.persistentDataPath + "/" + f + ".json"))
            return true;
        return false;
    }
    private string loadStringFromFile(string filename)
    {
        string filepath = Application.persistentDataPath + "/" + filename + ".json";
        //D/ebug.Log(filename + " loaded from path: " + filepath);
        return System.IO.File.ReadAllText(filepath);
    }
    private T jsonToClass<T>(string json)
    {
        T res = JsonConvert.DeserializeObject<T>(json); ;
        return res;
    }

    public void OnSettingsUpdate(string settingsJson)
    {
        saveStringAsFile(settingsJson, SettingsFilename);
        settings = jsonToClass<Settings>(settingsJson);
    }

    public void OnEffectsSettingsUpdate(string settingsJson)
    {
        saveStringAsFile(settingsJson, EffectsSettingsFilename);
        effectsSettings = jsonToClass<EffectsSettings>(settingsJson);
        /*
        if (settings != null)
        {
            JsonUtility.FromJsonOverwrite(settingsJson, EffetcsSettings);
            Debug.Log("EffetcsSettings updated with the following values: " +
            "\n" + settingsJson);
        }
        else
        {
            Debug.LogError("EffetcsSettingsScirptableObject is null.");
        }*/
    }

    public void OnPlayerSettingsUpdate(string settingsJson)
    {
        saveStringAsFile(settingsJson, PlayerSettingsFilename);
        playerSettings = jsonToClass<PlayerSettings>(settingsJson);
        /*
        if (settings != null)
        {
            JsonUtility.FromJsonOverwrite(settingsJson, PlayerSettings);
            Debug.Log("PlayerSettings updated with the following values: " +
            "\n" + settingsJson);
        }
        else
        {
            Debug.LogError("PlayerSettingsScirptableObject is null.");
        }*/
    }
    private void saveStringAsFile(string str, string filename)
    {
        string filepath = Application.persistentDataPath + "/" + filename + ".json";
        System.IO.File.WriteAllText(filepath, str);
        //D/ebug.Log(filename + " saved on path: " + filepath);
    }











    /*
    public float GetInitialSpace()
    {
        if (hasUpdate) return settings.InitialSpace;
        return defaultSettings.InitialSpace;
    }
    public float GetSpaceIncrease()
    {
        if (hasUpdate) return settings.SpaceIncrease;
        return defaultSettings.SpaceIncrease;
    }
    public float GetMaxLowerDeviation()
    {
        if (hasUpdate) return settings.MaxLowerDeviation;
        return defaultSettings.MaxLowerDeviation;
    }
    public float GetMaxUpperDeviation()
    {
        if (hasUpdate) return settings.MaxUpperDeviation;
        return defaultSettings.MaxUpperDeviation;
    }
    public float GetStopSpeed()
    {
        if (hasUpdate) return settings.StopSpeed;
        return defaultSettings.StopSpeed;
    }
    public float GetLerpDuration()
    {
        if (hasUpdate) return settings.LerpDuration;
        return defaultSettings.LerpDuration;
    }*/
}
