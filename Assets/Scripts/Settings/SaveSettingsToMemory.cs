using System;
using System.IO;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;

public class SaveSettingsToMemory : MonoBehaviour
{
    public string SettingsFilename;
    public string PlayerSettingsFilename;
    public string EffectsSettingsFilename;
    public string ObstaclesTypesFilename;
    public void OnSettingsUpdate(string settingsJson)
    {
        saveStringAsFile(settingsJson, SettingsFilename);
    }

    public void OnEffectsSettingsUpdate(string settingsJson)
    {
        saveStringAsFile(settingsJson, EffectsSettingsFilename);
    }

    public void OnPlayerSettingsUpdate(string settingsJson)
    {
        saveStringAsFile(settingsJson, PlayerSettingsFilename);
    }

    public void OnObstaclesTypesFilename(string settingsJson)
    {
        saveStringAsFile(settingsJson, ObstaclesTypesFilename);
    }
    private void saveStringAsFile(string str, string filename)
    {
        string filepath = Application.persistentDataPath + "/" + filename + ".json";
        System.IO.File.WriteAllText(filepath, str);
        //Debug.Log(filename + " saved on path: " + filepath);
    }
}
