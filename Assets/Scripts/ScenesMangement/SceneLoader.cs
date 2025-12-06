using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Unity.Services.Core;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.Events;

public class SceneLoader : MonoBehaviour
{
    public int sceneToLoad;
    public Slider progressBar;
    public TextMeshProUGUI progressText;
    public InitializeUnityServices initUGS;

    private void Start()
    {
        StartCoroutine(InitializeAndLoadScene());
    }
    private IEnumerator InitializeAndLoadScene()
    {
        progressText.text = "Initializing important stuff...";

        initUGS.StartUnityServices();

        progressText.text = "Initializing another important stuff...";

        initUGS.StartAuthentication();

        yield return StartCoroutine(LoadSceneAsync());
    }


    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(1f); // Optional: Add a short delay before starting to load

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;
        float progress = 0f;
        float lastPRogress = 0f;

        while (!asyncLoad.isDone)
        {
            progress = Mathf.RoundToInt((Mathf.Clamp01( asyncLoad.progress / 0.9f))*100);
            if (progress != lastPRogress)
            {
                lastPRogress = progress;
                progressBar.value = progress;
                progressText.text = $"Loading... {progress:F0}%";
            }
            // Check if the load has finished
            if (asyncLoad.progress >= 0.9f)
            {
                progressBar.value = 1f;
                progressText.text = "Be ready...";
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
