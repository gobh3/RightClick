using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.RemoteConfig;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RemoteConfig : MonoBehaviour
{
    public struct userAttributes
    {
        // Optionally declare variables for any custom user attributes:
    }

    public struct appAttributes
    {
        // Optionally declare variables for any custom app attributes:
    }


    /*public UnityEvent<int> OnPassLimitOfPointsChallengeChanged;
    public UnityEvent<int> OnPass3TimesChallengeChanged;
    public UnityEvent<float> OnInitilizedSpeedChanged;*/
    public UnityEvent<string> OnSettingsJsonChanged;
    public UnityEvent<string> OnPlayerSettingsJsonChanged;
    public UnityEvent<string> OnEffectsSettingsJsonChanged;
    public UnityEvent<string> OnObstacleTypeConfigJsonChanged;

    // Retrieve and apply the current key-value pairs from the service on Awake:
    public async void Initialize()
    {
        // Add a listener to apply settings when successfully retrieved:
        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
        await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
    }



    void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        // Conditionally update settings, depending on the response's origin:
        switch (configResponse.requestOrigin)
        {
            case ConfigOrigin.Default:
                Debug.Log("No settings loaded this session; using default values.");
                break;
            case ConfigOrigin.Cached:
                Debug.Log("No settings loaded this session; using cached values from a previous session.");
                break;
            case ConfigOrigin.Remote:
                Debug.Log("New settings loaded this session; update values accordingly.");
                string jsonString = "";
                try
                {
                    jsonString = RemoteConfigService.Instance.appConfig.GetJson("settings");
                    OnSettingsJsonChanged?.Invoke(jsonString);
                }
                catch (Exception e)
                {
                    Debug.LogError(e + "\n Json File:" + jsonString);
                }

                jsonString = "";
                try
                {
                    jsonString = RemoteConfigService.Instance.appConfig.GetJson("PlayerSettings");
                    OnPlayerSettingsJsonChanged?.Invoke(jsonString);
                }
                catch (Exception e)
                {
                    Debug.LogError(e + "\n Json File:" + jsonString);
                }

                jsonString = "";
                try
                {
                    jsonString = RemoteConfigService.Instance.appConfig.GetJson("EffectsSettings");
                    OnEffectsSettingsJsonChanged?.Invoke(jsonString);
                }
                catch (Exception e)
                {
                    Debug.LogError(e + "\n Json File:" + jsonString);
                }

                jsonString = "";
                try
                {
                    jsonString = RemoteConfigService.Instance.appConfig.GetJson("ObstacleTypeProportions");
                    OnObstacleTypeConfigJsonChanged?.Invoke(jsonString);
                }
                catch (Exception e)
                {
                    Debug.LogError(e + "\n Json File:" + jsonString);
                }
                break;

        }
    }
}
