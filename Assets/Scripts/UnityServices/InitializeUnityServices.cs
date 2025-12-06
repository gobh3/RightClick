using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;


public class InitializeUnityServices : MonoBehaviour
{
    public string environment = "production";
    public UnityEvent OnUnityServicesInitilized;
    public UnityEvent OnUnityServicesInitilizedOnLate;

    public async Task StartUnityServices()
    {
        await StartUnityServicesCoroutine();
        /*
        p//rint("Starting UnityServices... Frame Count: " + Time.frameCount);
        var options = new InitializationOptions()
                     .SetEnvironmentName(environment);
        Task unityServicesTask = UnityServices.InitializeAsync(options);
        int i = 0;
        while (!unityServicesTask.IsCompleted)
        {
            i++;
        }
        if (unityServicesTask.IsCompleted)
        {
            p//rint("unityServices Completed Frame Count: " +Time.frameCount + ". i=" + i);
            OnUnityServicesInitilized?.Invoke();
        }
        await unityServicesTask;*/
    }

    public async Task StartAuthentication()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            try
            {
                Task authenTask = AuthenticationService.Instance.SignInAnonymouslyAsync();
                await authenTask;
                if (authenTask.IsFaulted)
                    Debug.LogError("Authentication failed: " + authenTask.Exception);
            }
            catch (Exception e)
            {
                Debug.Log("Didn't Initilize Unity Authentication Because: \n" + e);
            }
        }
        else
        {
            print("Authentication already signed in");
        }
    }

    private async Task StartUnityServicesCoroutine()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            print("UnityServices Already Initalized");
        }
        else
        {
            var options = new InitializationOptions()
                     .SetEnvironmentName(environment);
            try
            {
                Task unityServicesTask = UnityServices.InitializeAsync(options);
                await unityServicesTask;
                OnUnityServicesInitilized?.Invoke();
                if (unityServicesTask.IsFaulted)
                    Debug.LogError("Unity Services initialization failed: " + unityServicesTask.Exception);
            }
            catch (Exception e)
            {
                Debug.Log("Didn't Initilize Unity Services Because: \n" + e);
            }
        }
        
    }
    /*
    private async void StartAuthenticationCoroutine()
    {
        Task authTask = startAuthentication();
        await authTask;
        if (authTask.IsFaulted)
            Debug.LogError("Authentication failed: " + authTask.Exception);
    }
    private async Task startUnityServices()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            print("UnityServices Already Initalized");
        }
        else
        {
            var options = new InitializationOptions()
                     .SetEnvironmentName(environment);
            try
            {
                Task unityServicesTask = UnityServices.InitializeAsync(options);
                await unityServicesTask;
            }
            catch (Exception e)
            {
                Debug.Log("Didn't Initilize Unity Services Because: \n" + e);
            }
        }
    }
    private async Task startAuthentication()
    {
        
    }*/

}